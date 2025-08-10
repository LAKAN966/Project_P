using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitState
{
    Idle,
    Moving,
    Fighting,
    Hitback,
    Dead
}

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public bool isEnemy;

    [SerializeField] private Transform spriteRoot;

    private Transform target;
    private float currentHP;
    private bool isAttacking = false;

    private UnitState state;
    private PlayerState currentAnimState = PlayerState.OTHER;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SPUM_Prefabs spumController;
    private readonly HashSet<int> triggeredHitbackZones = new();
    private Coroutine attackCoroutine;

    public GameObject effectParticle;

    [SerializeField] private Transform projectileSpawnPoint; // 없으면 transform.position 사용
    private GameObject projectilePrefab; // Resources/Projectiles/{stats.projectile} 캐시


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (stats == null) return;

        switch (state)
        {
            case UnitState.Moving:
                TryDetectEnemy();
                break;
            case UnitState.Idle:
                TryAttack();
                break;
        }
    }

    public virtual void Initialize()
    {
        currentHP = stats.MaxHP;
        triggeredHitbackZones.Clear();
        SetState(UnitState.Moving);
        gameObject.SetActive(true);
        spriteRoot.localScale = new Vector3(stats.Size * 1.5f, stats.Size * 1.5f, 0);

        SetLayerRecursively(gameObject, LayerMask.NameToLayer(isEnemy ? "Enemy" : "Ally"));
        LoadModel();

        LoadProjectilePrefab(); // << 추가
    }
    private void LoadProjectilePrefab()
    {
        projectilePrefab = null;
        if (stats == null) return;

        // 이름 정리
        string projName = (stats.projectile ?? string.Empty).Trim();

        // 비었거나 "-"면 투사체 없음
        if (string.IsNullOrEmpty(projName) || projName == "-")
            return;

        string path = $"Projectiles/{projName}";
        projectilePrefab = Resources.Load<GameObject>(path);
        if (projectilePrefab == null)
            Debug.LogWarning($"Projectile prefab not found at Resources/{path}");

        // 스폰 포인트 없으면 모델 하위에서 찾아보기(선택)
        if (projectileSpawnPoint == null && spriteRoot != null)
        {
            var t = spriteRoot.Find("ProjectileSocket");
            if (t != null) projectileSpawnPoint = t;
        }
    }


    public virtual void OnSpawned()
    {
    }
    private void LoadModel()
    {
        if (spriteRoot == null)
        {
            Debug.LogWarning("SpriteRoot가 설정되지 않았습니다.");
            return;
        }

        string path = $"Units/{stats.ModelName}";
        var modelPrefab = Resources.Load<GameObject>(path);

        if (modelPrefab == null)
        {
            Debug.LogWarning($"모델 프리팹을 찾을 수 없습니다: {path}");
            return;
        }

        foreach (Transform child in spriteRoot)
            Destroy(child.gameObject);

        GameObject instance = Instantiate(modelPrefab, spriteRoot);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;

        if (!isEnemy)
        {
            var scale = instance.transform.localScale;
            scale.x *= -1f;
            instance.transform.localScale = scale;
        }

        spumController = instance.GetComponent<SPUM_Prefabs>();
        spumController.OverrideControllerInit();
        spumController.PopulateAnimationLists();

        animator = spumController.GetComponentInChildren<Animator>();
    }

    private void MoveForward()
    {
        float dir = isEnemy ? -1f : 1f;
        transform.position += new Vector3(dir * stats.MoveSpeed * Time.deltaTime, 0, 0);

        if (spriteRenderer != null)
            spriteRenderer.flipX = isEnemy;
    }

    private void TryDetectEnemy()
    {
        Vector2 origin = transform.position;
        Vector2 direction = isEnemy ? Vector2.left : Vector2.right;
        var layerMask = LayerMask.GetMask(isEnemy ? "Ally" : "Enemy");

        var hit = Physics2D.Raycast(origin, direction, stats.AttackRange, layerMask);
        Debug.DrawRay(origin, direction * stats.AttackRange, Color.red);

        if (hit.collider == null)
            MoveForward();
        else if (hit.collider.TryGetComponent<BaseCastle>(out var castle))
        {
            if (castle.isEnemy == this.isEnemy) MoveForward();
            else
            {
                target = castle.transform;
                SetState(UnitState.Idle);
            }
        }
        else if (hit.collider?.GetComponent<Unit>() is Unit enemy)
        {
            if (enemy.state == UnitState.Hitback || enemy.state == UnitState.Dead)
                return;
            target = enemy.transform;
            SetState(UnitState.Idle);
        }
        else
            target = null;
    }

    private void TryAttack()
    {
        if (isAttacking) return;
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = null;
            SetState(UnitState.Moving);
            return;
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 공격 애니 길이
        float attackAnimLength = spumController.GetAnimationLength(PlayerState.ATTACK);

        // 공격 상태로 진입(애니 시작)
        SetState(UnitState.Fighting);

        // 리소스에서 읽은 투사체 설정(없으면 null)
        Projectile projCfgOnPrefab = projectilePrefab != null ? projectilePrefab.GetComponent<Projectile>() : null;

        // PreDelay: 애니가 이미 재생 중인 상태에서 추가 리드타임
        if (stats.PreDelay > 0f) yield return new WaitForSeconds(stats.PreDelay);

        // 투사체 소환 시점(공격 애니 내 비율 기반). 프리팹 없으면 애니 중간으로.
        float spawnWait = projCfgOnPrefab != null ? projCfgOnPrefab.SpawnTimeInAttack(attackAnimLength)
                                                  : 0.5f * attackAnimLength;
        if (spawnWait > 0f) yield return new WaitForSeconds(spawnWait);

        // 투사체 스폰(비주얼용)
        GameObject fx = null;
        Projectile projCfgOnInstance = null;
        if (projectilePrefab != null)
        {
            Vector3 pos = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
            fx = Instantiate(projectilePrefab, pos, Quaternion.identity);
            projCfgOnInstance = fx.GetComponent<Projectile>();
        }

        // 데미지 정산 시점(투사체 애니 내 비율 기반). 프리팹/인스턴스 정보가 없으면 0.25초 기본값.
        float damageWait =
            (projCfgOnInstance != null ? projCfgOnInstance.DamageTimeInFx()
             : projCfgOnPrefab != null ? projCfgOnPrefab.DamageTimeInFx()
             : 0.25f);

        if (damageWait > 0f) yield return new WaitForSeconds(damageWait);

        // 타겟이 사라졌을 수도 있으니, '그 시점'의 범위 판정으로 데미지
        DoDamage();

        // 보이는 것들 끝까지(공격 애니 남은 구간 vs 투사체 남은 구간)
        float remainingAttack = Mathf.Max(attackAnimLength - spawnWait, 0f);
        float fxLen = projCfgOnInstance?.FxLength ?? projCfgOnPrefab?.FxLength ?? 0.5f;
        float remainingFx = Mathf.Max(fxLen - damageWait, 0f);
        float tail = Mathf.Max(remainingAttack, remainingFx);
        if (tail > 0f) yield return new WaitForSeconds(tail);

        // PostDelay
        if (stats.PostDelay > 0f) yield return new WaitForSeconds(stats.PostDelay);

        SetState(UnitState.Moving);
        isAttacking = false;
    }

    private void DoDamage()
    {
        Vector2 center = transform.position;
        float radius = stats.AttackRange;
        int enemyLayer = LayerMask.NameToLayer(isEnemy ? "Ally" : "Enemy");
        int mask = 1 << enemyLayer;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, mask);

        if (stats.IsAOE)
        {
            foreach (var h in hits) ApplyDamage(h);
        }
        else
        {
            Collider2D closest = null;
            float minDist = float.MaxValue;
            foreach (var h in hits)
            {
                float d = Vector2.Distance(center, h.transform.position);
                if (d < minDist) { minDist = d; closest = h; }
            }
            if (closest != null) ApplyDamage(closest);
        }
    }

    private void ApplyDamage(Collider2D hit)
    {
        var unit = hit.GetComponent<Unit>();
        if (unit != null)
        {
            unit.TakeDamage(stats.Damage);
        }
        else
        {
            var castle = hit.GetComponent<BaseCastle>();
            if (castle != null)
            {
                castle.TakeDamage((int)stats.Damage);
            }
        }
    }

    private void HitEffect()
    {
        if(effectParticle == null)
        {
            return;
        }
        SFXManager.Instance.PlaySFX(1);
        Vector3 pos = transform.position + new Vector3(0, 1f, 0);
        GameObject obj = Instantiate(effectParticle, pos, Quaternion.Euler(90,0,0));
        Destroy(obj, 1f);
    }

    public void TakeDamage(float amount)
    {
        if (stats == null || state is UnitState.Hitback or UnitState.Dead) return;

        float oldHP = currentHP;
        currentHP -= amount;

        HitEffect();

        if (currentHP <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            float slice = stats.MaxHP / Mathf.Max(stats.Hitback, 1);
            for (int i = stats.Hitback; i >= 1; i--)
            {
                float threshold = slice * (i - 1);
                if (oldHP > threshold && currentHP <= threshold && !triggeredHitbackZones.Contains(i))
                {
                    triggeredHitbackZones.Add(i);
                    StartCoroutine(DoHitback());
                    break;
                }
            }
        }
    }


    private IEnumerator Die()
    {
        SkillManager.Instance?.OnUnitDeath(this);
        if (isAttacking && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            isAttacking = false;
            attackCoroutine = null;
        }
        SetState(UnitState.Dead);

        if (isEnemy) BattleResourceManager.Instance.Add(stats.Cost);
        stats = null;
        target = null;

        float deathAnimTime = 0f;
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            deathAnimTime = stateInfo.length;
        }
        yield return new WaitForSeconds(deathAnimTime > 0 ? deathAnimTime : 1f);
        spriteRoot.localScale = Vector3.one;


        var pool = GetComponentInParent<UnitPool>();
        if (pool != null) pool.ReturnUnit(this);
        else gameObject.SetActive(false);
    }

    private IEnumerator DoHitback()
    {
        if (isAttacking && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            isAttacking = false;
            attackCoroutine = null;
        }

        SetState(UnitState.Hitback);

        spumController.PlayAnimation(PlayerState.DAMAGED, 0);

        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        float duration = 0.5f;
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(isEnemy ? 1f : -1f, 0f, 0f);

        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gameObject.layer = originalLayer;

        SetState(UnitState.Moving);
    }


    private void SetState(UnitState newState)
    {
        if (state == newState && currentAnimState != PlayerState.OTHER) return;
        state = newState;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (spumController == null || !spumController.allListsHaveItemsExist()) return;

        PlayerState newAnim = ConvertToPlayerState(state);
        if (newAnim != currentAnimState)
        {
            currentAnimState = newAnim;
            spumController.PlayAnimation(newAnim, state == UnitState.Fighting ? stats.AttackType : 0);
        }
    }

    private PlayerState ConvertToPlayerState(UnitState unitState) => unitState switch
    {
        UnitState.Idle => PlayerState.IDLE,
        UnitState.Moving => PlayerState.MOVE,
        UnitState.Fighting => PlayerState.ATTACK,
        UnitState.Hitback => PlayerState.DAMAGED,
        UnitState.Dead => PlayerState.DEATH,
        _ => PlayerState.OTHER
    };

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
