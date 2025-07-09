using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int gold = 999999; // 골드
    public int ticket = 0; // 뽑기 재화
    public int bluePrint = 99999; // 설계도
    public int tribute = 999999; // 공물

    public List<int> myUnitIDs = new(); // 보유 유닛 정보
    public DeckData currentDeck = new(); // 덱 정보

    public List<int> clearedStageIDs = new(); // 클리어 스테이지 정보

    public int actionPoint = 100; // 현재 행동력
    public long lastActionPointTime; // 행동력 사용한 마지막 시간

    public List<PlayerQuestData> playerQuest = new(); // 플레이어의 퀘스트 리스트
}

public class PlayerQuestData
{
    public int QuestID; // 퀘스트 아이디
    public int CurrentValue; // 퀘스트 데이터의 컨디션 벨류
    public bool IsCompleted; // 퀘스트 완료 여부
    public bool IsReward; // 퀘스트 보상 획득 여부
}
