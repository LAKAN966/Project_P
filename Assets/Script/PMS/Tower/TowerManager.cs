using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public readonly int maxEntryCounts = 3;
    private PlayerTowerData data => PlayerDataManager.Instance.player.towerData;

    private int currentStageID;

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

    public int GetEnterCount(int raceID)
    {
        if (!data.entryCounts.ContainsKey(raceID))
        {
            data.entryCounts[raceID] = maxEntryCounts;
        }
        return data.entryCounts[raceID];
    }

    public void EnterBattle(int stageID)
    {
        var stage = StageDataManager.Instance.GetStageData(stageID);
        int raceID = stage.RaceID;

        if (!CanEnterTower(raceID))
        {
            Debug.Log("입장 횟수가 부족합니다.");
            return;
        }

        bool entered = EnterTower(raceID);
        if (!entered)
        {
            Debug.Log("입장 실패");
            return;
        }

        currentStageID = stageID;  // 멤버 변수로 저장

        SceneManager.sceneLoaded += OnBattleSceneLoaded;
        SceneManager.LoadScene("BattleScene");
        Debug.Log($"{stageID} 입장");
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            var normalDeck = PlayerDataManager.Instance.player.currentDeck.GetAllNormalUnit();
            var leaderDeck = PlayerDataManager.Instance.player.currentDeck.GetLeaderUnitInDeck();
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;

            BattleManager.Instance.StartBattle(currentStageID, normalDeck, leaderDeck, 1);
        }
    }

}
