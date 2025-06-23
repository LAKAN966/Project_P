using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckList 
{
    public int myUnitID;
}
public class DeckData
{
    public List<DeckList> deckList = new(); // 덱 리스트 만들기. 일반 유닛.
    public DeckList leaderUnit; // 리더 유닛.

    public bool Contains(int myUnitID) // 덱 리스트에서 아이디 값으로 확인 함수.
    {
        return deckList.Any(unit => unit.myUnitID == myUnitID) || (leaderUnit != null && leaderUnit.myUnitID == myUnitID);
    }

    public bool AddNormalUnit(int myUnitID) // 일반 덱 구성 함수.
    {
        if(deckList.Count >= 6 || Contains(myUnitID)) // 일반 덱 구성인원 최대 6명.
        {
            return false;
        }

        deckList.Add(new DeckList { myUnitID = myUnitID });
        return true;
    }

    public bool SetLeaderUnit(int myUnitID) // 리더 유닛 세팅 함수.
    {
        if (Contains(myUnitID)) return false;

        UnitStats stat = DataManager.Instance.GetStats(myUnitID); // 스텟에서 IsHero bool 값으로 확인.
        if (stat == null || !stat.IsHero)
        {
            Debug.LogWarning($"유닛 {myUnitID}는 리더로 설정할 수 없습니다 (IsHero = false)");
            return false;
        }

        leaderUnit = new DeckList { myUnitID = myUnitID };
        return true;
    }

    public void RemovUnit(int myUnitID) // 덱 리스트에서 유닛 제거 함수.
    {
        deckList.RemoveAll(unit => unit.myUnitID == myUnitID);

        if(leaderUnit != null && leaderUnit.myUnitID == myUnitID)
        {
            leaderUnit = null;
        }
    }

    public void SaveDeck() // 덱 리스트 설정 저장. 키 값만 저장.
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("DeckData", json);
    }

    public static DeckData LoadDeck() // 덱 리스트 설정 불러오기.
    {
        string json = PlayerPrefs.GetString("DeckData", "");
        return string.IsNullOrEmpty(json) ? new DeckData() : JsonUtility.FromJson<DeckData>(json);
    }
}
