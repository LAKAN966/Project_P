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

    //private int Gold = 999999; // 골드
    //private int Ticket = 0; // 뽑기 재화
    //private int BluePrint = 99999; // 설계도
    //private int Tribute = 999999; // 공물
    //private int Certi = 0; // 뽑기 천장재화

    //public int gold { get => Gold; set => Gold = value; }
    //public int ticket { get => Ticket; set => Ticket = value; }
    //public int bluePrint { get => BluePrint; set => BluePrint = value; }
    //public int tribute { get => Tribute; set => Tribute = value; }
    //public int certi { get => Certi; set => Certi = value; }


    //private List<int> MyUnitIDs = new(); //보유 유닛 정보
    //private DeckData CurrentDeck = new(); // 덱 정보

    //public List<int> myUnitIDs => MyUnitIDs;
    //public DeckData currentDeck { get => CurrentDeck; set => CurrentDeck = value; }


    //private List<int> ClearedStageIDs = new(); // 클리어 스테이지 정보
    //public List<int> clearedStageIDs => ClearedStageIDs;

    //private int ActionPoint = 100; // 현재 행동력
    //private long LastActionPointTime; // 행동력 사용한 마지막 시간
    //public int actionPoint { get => ActionPoint; set => ActionPoint = value; }
    //public long lastActionPointTime { get => LastActionPointTime; set => LastActionPointTime = value; }


    //private List<PlayerQuestData> PlayerQuest = new(); // 플레이어의 퀘스트 리스트
    //private List<BuildingState> BuildingsList = new();
    //private Dictionary<int, HashSet<int>> SelectedGospelIDsByBuildID = new();//id별 선택된 교리 데이터

    //public List<PlayerQuestData> playerQuest => PlayerQuest;
    //public List<BuildingState> buildingsList => BuildingsList;
    //public Dictionary<int, HashSet<int>> selectedGospelIDsByBuildID => SelectedGospelIDsByBuildID;
}

public class PlayerQuestData
{
    public int QuestID; // 퀘스트 아이디
    public int CurrentValue; // 퀘스트 데이터의 컨디션 벨류
    public bool IsCompleted; // 퀘스트 완료 여부
    public bool IsReward; // 퀘스트 보상 획득 여부
}
