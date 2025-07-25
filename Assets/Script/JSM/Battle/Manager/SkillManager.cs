using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public GameObject skillPanel;

    [Header("스킬 ID")]
    public int passiveSkillID = 0;
    public int activeSkillID = 0;
    public SkillData passiveSkillData;
    public SkillData activeSkillData;

    [Header("스킬 참조")]
    public GraveSpawnSkill graveSpawnSkill;

    public GameObject unitpool;
    public GameObject heroUnitpool;
    public GameObject spawnBtn;
    public SpawnButton heroUnitBtn;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetSkillID(int a, int b)
    {
        passiveSkillID = a;
        activeSkillID = b;
        passiveSkillData = SkillDataManager.Instance.GetSkillData(a);
        activeSkillData = SkillDataManager.Instance.GetSkillData(b);
        Debug.Log($"{passiveSkillData.Name}!\n{activeSkillData.Name}!");
    }

    public void OnUnitDeath(Unit unit)
    {
        switch (unit.isEnemy ? 0 : passiveSkillID)
        {
                case 1:
                {
                    if(unit.stats.RaceID == passiveSkillData.TargetRaceID) graveSpawnSkill?.TrySpawnGrave(unit, passiveSkillData.EffectValue[0], passiveSkillData.EffectValue[1]);
                    break;
                }
        }
    }

    public void UseSkill(int skillID, bool isEnemy)
    {
        Debug.Log($"스킬 발동 요청: ID={skillID}, isEnemy={isEnemy}");

        switch (skillID)
        {
            case 2:
                if (graveSpawnSkill != null)
                    graveSpawnSkill.ActivateGraves(isEnemy, activeSkillData.EffectValue[0]);
                break;
            case 4:
                BuffSkill();
                break;
            default:
                Debug.LogWarning($"정의되지 않은 스킬 ID: {skillID}");
                break;
        }
    }
    public UnitStats OnStartBuff(UnitStats stat)
    {
        switch (passiveSkillID)
        {
            case 3:
                if (stat.RaceID == passiveSkillData.TargetRaceID)
                    stat.Cost = Mathf.FloorToInt(stat.Cost * (100 - passiveSkillData.EffectValue[0])/100);
                break;
            default:
                Debug.LogWarning($"정의되지 않은 스킬 ID: {passiveSkillID}");
                break;
        }
        SetSkillPanel();
        return stat;
    }

    public void BuffSkill()
    {
        StartCoroutine(ApplyBuffCoroutine());
    }

    private IEnumerator ApplyBuffCoroutine()
    {
        Unit[] targets = unitpool.GetComponentsInChildren<Unit>();
        SpawnButton[] buttons = spawnBtn.GetComponentsInChildren<SpawnButton>();
        Debug.Log($"{activeSkillData.EffectValue[0]}");
        Debug.Log($"{activeSkillData.EffectValue[1]}");
        float moveBuff = (100 + activeSkillData.EffectValue[0]) / 100f;
        float delayBuff = (100 - activeSkillData.EffectValue[1]) / 100f;

        // 원래 값 저장
        List<(Unit unit, float moveSpeed, float preDelay, float postDelay)> unitBackups = new();
        foreach (Unit target in targets)
        {
            unitBackups.Add((target, target.stats.MoveSpeed, target.stats.PreDelay, target.stats.PostDelay));

            target.stats.MoveSpeed *= moveBuff;
            target.stats.PreDelay *= delayBuff;
            target.stats.PostDelay *= delayBuff;
        }

        List<(SpawnButton button, float moveSpeed, float preDelay, float postDelay)> buttonBackups = new();
        foreach (SpawnButton button in buttons)
        {
            buttonBackups.Add((button, button.stats.MoveSpeed, button.stats.PreDelay, button.stats.PostDelay));

            button.stats.MoveSpeed *= moveBuff;
            button.stats.PreDelay *= delayBuff;
            button.stats.PostDelay *= delayBuff;
        }
        // 15초 대기
        yield return new WaitForSeconds(15f);
        Debug.Log("버프 해제");
        // 원래 값 복원
        foreach (var (unit, moveSpeed, preDelay, postDelay) in unitBackups)
        {
            unit.stats.MoveSpeed = moveSpeed;
            unit.stats.PreDelay = preDelay;
            unit.stats.PostDelay = postDelay;
        }

        foreach (var (button, moveSpeed, preDelay, postDelay) in buttonBackups)
        {
            button.stats.MoveSpeed = moveSpeed;
            button.stats.PreDelay = preDelay;
            button.stats.PostDelay = postDelay;
        }
    }

    private void SetSkillPanel()
    {
        skillPanel.SetActive(passiveSkillData != null);
        var skillcs = skillPanel.GetComponent<SkillPanel>();
        skillcs.skillName.text = passiveSkillData.Name;
        skillcs.skillDescription.text = passiveSkillData.Description;
    }
}
