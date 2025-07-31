using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int gold = 999999;
    public int ticket = 0;
    public int bluePrint = 99999;
    public int tribute = 999999;
    public int certi = 0;

    public List<int> myUnitIDs = new();
    public DeckData currentDeck = new();
    public List<DeckData> preset = new List<DeckData>();
    public int currentPresetIndex;
    public List<int> clearedStageIDs = new();

    public int actionPoint = 100;
    public int maxActionPoint = 100;
    public long lastActionPointTime;

    public long lastDailyQuestTime = 0; // 일일 퀘스트 초기화 시간
    public long lastWeeklyQuestTime = 0; // 주간 퀘스트 초기화 시간

    public List<PlayerQuestData> playerQuest = new();
    public GoldDungeonData goldDungeonData = new();

    public List<BuildingState> buildingsList = new();
    public Dictionary<int, HashSet<int>> selectedGospelIDsByBuildID = new();
    public Dictionary<int, UnitStats> buildingBuffs = new();

    //public int pickPoint = 0;

    public PlayerTowerData towerData = new();

    public Player()
    {
        for(int i = 0; i<3; i++)
        {
            preset.Add(new DeckData());
        }

        currentPresetIndex = 0;
        currentDeck = DeckManager.Instance.CloneDeck(preset[0]);
        AddUnit(1001);
        AddUnit(1002);

    }

    public void AddUnit(int unitID)
    {
        if (!myUnitIDs.Contains(unitID))
        {
            myUnitIDs.Add(unitID);
        }
    }
}



public class PlayerQuestData
{
    public int QuestID; // 퀘스트 아이디
    public int CurrentValue; // 퀘스트 데이터의 컨디션 벨류
    public bool IsCompleted; // 퀘스트 완료 여부
    public bool IsReward; // 퀘스트 보상 획득 여부
}

public class GoldDungeonData
{
    public int lastClearStage = 0;  //클리어한 가장 높은 스테이지
    public int entryCounts = 3;     //보상획득가능 횟수
}
