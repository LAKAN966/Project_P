using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int gold = 0; // 골드
    public int ticket = 0; // 뽑기 재화

    public List<int> myUnitIDs = new(); // 보유 유닛 정보
    public DeckData currentDeck = new(); // 덱 정보

    public List<int> clearedStageIDs = new(); // 클리어 스테이지 정보

}
