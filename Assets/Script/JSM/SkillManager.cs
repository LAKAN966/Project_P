using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [Header("공통 스킬")]
    public GraveSpawnSkill graveSpawnSkill;

    [Header("영웅 스킬 ID")]
    public int allyHeroSkillID_1;
    public int allyHeroSkillID_2;
    public int enemyHeroSkillID_1;
    public int enemyHeroSkillID_2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnUnitDeath(Unit unit)
    {
        if (unit == null) return;
        graveSpawnSkill?.TrySpawnGrave(unit);
    }

    public void UseHeroSkill(int skillID, Unit unit)
    {
        switch (skillID)
        {
            case 0:
                Debug.Log("좀비 부활!");
                graveSpawnSkill.ActivateAllZombies();
                break;
            default:
                Debug.LogWarning($"정의되지 않은 스킬 ID: {skillID}");
                break;
        }
    }
}
