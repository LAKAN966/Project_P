using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (stats != null) Initialize();
        else gameObject.SetActive(false);
    }

    void Update()
    {
        if (stats == null) return;

        switch (state)
        {
            case UnitState.Moving:
                TryDetectEnemy();
                MoveForward();
                break;
            case UnitState.Idle:
                TryAttack();
                break;
        }
    }

    public void Initialize()
    {
        currentHP = stats.MaxHP;
        triggeredHitbackZones.Clear();
        SetState(UnitState.Moving);
        gameObject.SetActive(true);

        SetLayerRecursively(gameObject, LayerMask.NameToLayer(isEnemy ? "Enemy" : "Ally"));
        LoadModel();
    }

    private void LoadModel()
    {
        if (spriteRoot == null)
        {
            Debug.LogWarning("SpriteRoot가 설정되지 않았습니다.");
            return;
        }

        string path = $"SPUM/{stats.ModelName}";
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

        if (hit.collider?.GetComponent<Unit>() is Unit enemy)
        {
            if (enemy.state == UnitState.Hitback || enemy.state == UnitState.Dead)
                return;
            target = enemy.transform;
            SetState(UnitState.Idle);
        }
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

        yield return new WaitForSeconds(stats.PreDelay);

        SetState(UnitState.Fighting);

        float attackAnimLength = spumController.GetAnimationLength(PlayerState.ATTACK);
        yield return new WaitForSeconds(attackAnimLength);

        // 데미지 처리
        if (stats.IsAOE)
        {
            Vector2 center = transform.position;
            float radius = stats.AttackRange;
            int enemyLayer = LayerMask.NameToLayer(isEnemy ? "Ally" : "Enemy");
            int mask = 1 << enemyLayer;

            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, mask);
            foreach (var hit in hits)
            {
                var unit = hit.GetComponent<Unit>();
                if (unit != null)
                    unit.TakeDamage(stats.Damage);
            }
        }
        else
        {
            var unit = target?.GetComponent<Unit>();
            if (unit != null)
                unit.TakeDamage(stats.Damage);
        }

        yield return new WaitForSeconds(stats.PostDelay);
        SetState(UnitState.Moving);
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        if (stats == null || state is UnitState.Hitback or UnitState.Dead) return;

        float oldHP = currentHP;
        currentHP -= amount;

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
        if (isAttacking && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            isAttacking = false;
            attackCoroutine = null;
        }
        SetState(UnitState.Dead);

        stats = null;
        target = null;

        float deathAnimTime = 0f;
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            deathAnimTime = stateInfo.length;
        }
        yield return new WaitForSeconds(deathAnimTime > 0 ? deathAnimTime : 1f);

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
        Debug.Log(isEnemy+" : "+state);
    }

    private void UpdateAnimation()
    {
        if (spumController == null || !spumController.allListsHaveItemsExist()) return;

        PlayerState newAnim = ConvertToPlayerState(state);
        if (newAnim != currentAnimState)
        {
            currentAnimState = newAnim;
            spumController.PlayAnimation(newAnim, 0);
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
