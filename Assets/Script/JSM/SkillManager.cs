using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [Header("스킬 ID")]
    public int allySkillID;
    public int enemySkillID;

    [Header("스킬 참조")]
    public GraveSpawnSkill graveSpawnSkill;

    private void Awake()
    {
        if (Instance != null)
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

    public void UseSkill(int skillID, bool isEnemy)
    {
        Debug.Log($"스킬 발동 요청: ID={skillID}, isEnemy={isEnemy}");

        switch (skillID)
        {
            case 0:
                if (graveSpawnSkill != null)
                    graveSpawnSkill.ActivateGraves(isEnemy);
                break;
            default:
                Debug.LogWarning($"정의되지 않은 스킬 ID: {skillID}");
                break;
        }
    }

    public void UseAllySkill()
    {
        UseSkill(allySkillID, isEnemy: false);
    }

    public void UseEnemySkill()
    {
        UseSkill(enemySkillID, isEnemy: true);
    }
}
