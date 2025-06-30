using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DeckManager
{
    //public DeckData currentDeck = new();

    private static DeckManager instance;
    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DeckManager();
            }
            return instance;
        }
    }

    private DeckData CurrentDeck => PlayerDataManager.Instance.player.currentDeck;

    public bool TryAddUnitToDeck(int myUnitID) // 덱에 배치할 유닛이 리더유닛인지 아닌지 확인 후 맞는 함수 호출.
    {
        if (!PlayerDataManager.Instance.player.myUnitIDs.Contains(myUnitID))
        {
            Debug.Log($"유닛 {myUnitID}는 보유 중이 아니므로 덱에 배치할 수 없습니다.");
            return false;
        }

        UnitStats stats = UnitDataManager.Instance.GetStats(myUnitID);

        if (stats == null)
        {
            return false;
        }

        if (stats.IsHero)
        {
            Debug.Log($"{myUnitID} 리더 유닛 배치 완료");
            return CurrentDeck.SetLeaderUnit(myUnitID); // 리더 유닛이라면 리더 칸에 배치
        }
        else
        {
            Debug.Log($"{myUnitID} 일반 유닛 배치 완료");
            return CurrentDeck.AddNormalUnit(myUnitID); // 일반 유닛이라면 일반 칸에 배치
        }

    }

    public void RemoveFromDeck(int myUnitID) // 덱 리스트에서 제거
    {
        CurrentDeck.RemoveUnit(myUnitID);
    }

    public bool CheckInDeck(int myUnitID) // 덱 포함 여부 확인 용. UI에서 유닛 표현을 다르게 하기 위해서?
    {
        return CurrentDeck.Contains(myUnitID);
    }

    public List<UnitStats> GetAllDataInDeck() 
    {
        if(CurrentDeck != null)
        {
            return CurrentDeck.GetAllUnitInDeck();
        }

        Debug.Log($"{CurrentDeck} is null");
        return null;
    }

    public UnitStats GetLeaderDataInDeck()
    {
        return CurrentDeck.GetLeaderUnitInDeck();
    }

    public List<int> GetAllNormalUnit() // 현재 덱에 있는 일반 유닛 아이디 리스트로 가지고 오기. UI 참조 용.
    {
        return CurrentDeck.deckList.Select(unit => unit.myUnitID).ToList();
    }

    public int? GetLeaderUnit() // 현재 덱에 있는 리더 유닛 아이디 가지고 오기. UI 참조 용.
    {
        return CurrentDeck.leaderUnit?.myUnitID;
    }

}
