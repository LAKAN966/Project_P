using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    public static GimmickManager Instance;

    // 기믹 데이터 저장용
    public Dictionary<int, GimmickData> gimmickDict = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void ApplyGimmick(int id)
    {
        if (!GimmickDataManager.gimmickDict.TryGetValue(id, out var data))
        {
            Debug.LogWarning($"[GimmickManager] ID {id} 기믹을 찾을 수 없습니다.");
            return;
        }

        switch (data.Name)
        {
            case "TimeLimit": SetTimeLimit(data.EffectValue); break;
            case "UnitResummonCooldownUp": SetUnitResummonCooldownUp(data.EffectValue); break;
            case "UnitSummonCostUp": SetUnitSummonCostUp(data.EffectValue); break;
            case "LeaderUnitSummonBan": SetLeaderUnitSummonBan(); break;
        }
    }

    private static void SetTimeLimit(float value)
    {
        WaveManager.Instance.Timer.SetActive(true);
        GameManager.Instance.timer.timeLimit = value;
    }

    private static void SetUnitResummonCooldownUp(float value)
    {
        // SpawnButton에서 적용
    }

    private static void SetUnitSummonCostUp(float value)
    {
        // SpawnButton에서 적용
    }

    private static void SetLeaderUnitSummonBan()
    {
        // 제한 처리 로직 필요 시 여기에 구현
    }
}
