using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager
{
    private static TowerManager instance;
    public static TowerManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new TowerManager();
            }
            return instance;
        }
    }

    private const int maxEntryCounts = 3;
    private PlayerTowerData data => PlayerDataManager.Instance.player.towerData;

    public int GetClearedFloor(int raceID)
    {
        return PlayerDataManager.Instance.player.towerData.lastClearFloor.TryGetValue(raceID, out var floor) ? floor : 0;
    }

    public StageData GetCurrentFloorStage(int raceID)
    {
        int floor = GetClearedFloor(raceID);

        // 해당 종족의 모든 타워 스테이지 중에서
        var stage = StageDataManager.Instance.GetAllTowerStageData()
            .Values
            .Where(s => s.RaceID == raceID)
            .OrderBy(s => s.Chapter)
            .FirstOrDefault(s => s.Chapter == (floor > 0 ? floor : 1)); // 클리어 층, 없으면 1층

        return stage;
    }
    public bool CanEnterTower(int raceID)
    {
        if (!data.entryCounts.ContainsKey(raceID))
        {
            data.entryCounts[raceID] = maxEntryCounts;
        }
        return data.entryCounts[raceID] > 0;
    }

    public bool EnterTower(int raceID)
    {
        if (!CanEnterTower(raceID))
        {
            return false;
        }

        data.entryCounts[raceID]--;
        return true;
    }
}
