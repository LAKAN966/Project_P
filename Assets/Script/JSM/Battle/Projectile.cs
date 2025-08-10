using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    [Header("정규화 타이밍(0~1)")]
    [Range(0f, 1f)] public float attackFxSpawnPoint = 0.5f;    // 공격 애니 내 소환 시점
    [Range(0f, 1f)] public float projectileDamagePoint = 0.5f; // 투사체 애니 내 데미지 시점

    [Header("길이/수명")]
    [Tooltip("0이면 Animator/ParticleSystem에서 추정")]
    public float overrideFxLength = 0f;
    public bool destroyOnEnd = true;

    private Animator _anim;
    private ParticleSystem _ps;
    private float _fxLength;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        _fxLength = CalcFxLength();
        if (destroyOnEnd && _fxLength > 0f) Destroy(gameObject, _fxLength + 0.03f);
    }

    public float FxLength => (_fxLength > 0f) ? _fxLength : CalcFxLength();
    public float SpawnTimeInAttack(float attackAnimLength)
        => Mathf.Clamp01(attackFxSpawnPoint) * Mathf.Max(attackAnimLength, 0f);
    public float DamageTimeInFx()
        => Mathf.Clamp01(projectileDamagePoint) * Mathf.Max(FxLength, 0f);

    private float CalcFxLength()
    {
        if (overrideFxLength > 0f) return overrideFxLength;

        if (_anim != null && _anim.runtimeAnimatorController != null)
        {
            var infos = _anim.GetCurrentAnimatorClipInfo(0);
            if (infos != null && infos.Length > 0 && infos[0].clip != null)
                return infos[0].clip.length;

            var clips = _anim.runtimeAnimatorController.animationClips;
            if (clips != null && clips.Length > 0 && clips[0] != null)
                return clips[0].length;
        }

        if (_ps != null)
        {
            var m = _ps.main;
            float life = m.startLifetime.mode == ParticleSystemCurveMode.TwoConstants
                ? Mathf.Max(m.startLifetime.constantMin, m.startLifetime.constantMax)
                : m.startLifetime.constantMax;
            return Mathf.Max(m.duration, m.duration + life);
        }

        return 0.5f; // 기본값
    }
}
