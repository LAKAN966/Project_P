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
    public List<int> clearedStageIDs = new();

    public int actionPoint = 100;
    public int maxActionPoint = 100;
    public long lastActionPointTime;

    public long lastDailyQuestTime = 0; // 일일 퀘스트 초기화 시간
    public long lastWeeklyQuestTime = 0; // 주간 퀘스트 초기화 시간

    public List<PlayerQuestData> playerQuest = new();

    public List<BuildingState> buildingsList = new();
    public Dictionary<int, HashSet<int>> selectedGospelIDsByBuildID = new();

    public int pickPoint = 0;

    public Dictionary<string, int> lastClearFloor = new(); // 마지막 클리어 층
    public Dictionary<string, int> entryCounts = new(); // 입장 횟수
    public long towerResetTime = 0; // 입장 초기화 시간

}

public class PlayerQuestData
{
    public int QuestID; // 퀘스트 아이디
    public int CurrentValue; // 퀘스트 데이터의 컨디션 벨류
    public bool IsCompleted; // 퀘스트 완료 여부
    public bool IsReward; // 퀘스트 보상 획득 여부
}
