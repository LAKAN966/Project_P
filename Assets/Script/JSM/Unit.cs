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
    public UnitStats stats;     // null이면 비활성화 상태
    public bool isEnemy;

    private Transform target;
    private float currentHP;
    private float attackCooldown;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private UnitState state;

    private HashSet<int> triggeredHitbackZones = new();
    private Queue<IEnumerator> hitbackQueue = new();
    private bool isHitbackRunning = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (stats != null)
            Initialize();
        else
            gameObject.SetActive(false);
    }

    void Update()
    {
        Debug.Log(state);
        if (stats == null || state == UnitState.Dead || state == UnitState.Hitback) return;

        switch (state)
        {
            case UnitState.Moving:
                MoveForward();
                TryDetectEnemy();
                break;
            case UnitState.Fighting:
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

        int layer = LayerMask.NameToLayer(isEnemy ? "Enemy" : "Ally");
        SetLayerRecursively(gameObject, layer);
    }

    private void MoveForward()
    {
        float dir = isEnemy ? -1f : 1f;
        transform.position += new Vector3(dir * stats.MoveSpeed * Time.deltaTime, 0, 0);

        if (spriteRenderer != null)
            spriteRenderer.flipX = isEnemy;
    }

    void TryDetectEnemy()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = isEnemy ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            stats.AttackRange,
            LayerMask.GetMask(isEnemy ? "Ally" : "Enemy")
        );

        Debug.DrawRay(origin, direction * stats.AttackRange, Color.red);

        if (hit.collider != null)
        {
            Unit enemy = hit.collider.GetComponent<Unit>();
            if (enemy != null)
            {
                target = enemy.transform;
                SetState(UnitState.Fighting);
            }
        }
    }


    private void TryAttack()
    {
        if (target == null || !target.gameObject.activeInHierarchy || Vector2.Distance(transform.position, target.position) > stats.AttackRange)
        {
            SetState(UnitState.Moving);
            return;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown > 0) return;

        if (stats.IsAOE)
        {
            // 범위 내 모든 적 유닛 공격
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
            // 단일 대상 공격
            var unit = target.GetComponent<Unit>();
            if (unit != null)
                unit.TakeDamage(stats.Damage);
        }
        attackCooldown = 1f;
    }

    public void TakeDamage(float amount)
    {
        if (stats == null || state == UnitState.Hitback) return;

        float oldHP = currentHP;
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        if (stats.Hitback > 0)
        {
            float slice = stats.MaxHP / stats.Hitback;

            for (int i = stats.Hitback; i >= 1; i--)
            {
                float lowerBound = slice * (i - 1);

                if (oldHP > lowerBound && currentHP <= lowerBound && !triggeredHitbackZones.Contains(i))
                {
                    triggeredHitbackZones.Add(i);
                    hitbackQueue.Enqueue(DoHitback());
                    break;
                }
            }

            if (!isHitbackRunning && hitbackQueue.Count > 0)
                StartCoroutine(ProcessHitbackQueue());
        }
    }


    private void Die()
    {
        SetState(UnitState.Dead);
        stats = null;
        target = null;
        attackCooldown = 0f;
        
        var pool = GetComponentInParent<UnitPool>();
        if (pool != null)
        {
            pool.ReturnUnit(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetState(UnitState newState)
    {
        if (state == newState) return;
        state = newState;

        if (animator != null)
            animator.SetInteger("State", (int)state);
    }
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
    private IEnumerator ProcessHitbackQueue()
    {
        isHitbackRunning = true;

        while (hitbackQueue.Count > 0)
            yield return StartCoroutine(hitbackQueue.Dequeue());

        isHitbackRunning = false;
    }

    private IEnumerator DoHitback()
    {
        SetState(UnitState.Hitback);

        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        float duration = 0.5f;
        float knockbackDistance = 1f;
        float elapsed = 0f;

        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(isEnemy ? 1f : -1f, 0f, 0f) * knockbackDistance;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gameObject.layer = originalLayer;

        SetState(UnitState.Moving);
    }

}