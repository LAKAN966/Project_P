using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DeckManager
{
    public DeckData currentDeck = new();

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

    public bool TryAddUnitToDeck(int myUnitID) // 덱에 배치할 유닛이 리더유닛인지 아닌지 확인 후 맞는 함수 호출.
    {
        UnitStats stats = DataManager.Instance.GetStats(myUnitID);

        if (stats == null)
        {
            return false;
        }

        if (stats.IsHero)
        {
            return currentDeck.SetLeaderUnit(myUnitID); // 리더 유닛이라면 리더 칸에 배치
        }
        else
        {
            return currentDeck.AddNormalUnit(myUnitID); // 일반 유닛이라면 일반 칸에 배치
        }

    }

    public void RemoveFromDeck(int myUnitID) // 덱 리스트에서 제거
    {
        currentDeck.RemovUnit(myUnitID);
    }

    public bool CheckInDeck(int myUnitID) // 덱 포함 여부 확인 용. UI에서 유닛 표현을 다르게 하기 위해서?
    {
        return currentDeck.Contains(myUnitID);
    }

    public List<int> GetAllNormalUnit() // 현재 덱에 있는 일반 유닛 아이디 리스트로 가지고 오기. UI 참조 용.
    {
        return currentDeck.deckList.Select(unit => unit.myUnitID).ToList();
    }

    public int? GetLeaderUnit() // 현재 덱에 있는 리더 유닛 아이디 가지고 오기. UI 참조 용.
    {
        return currentDeck.leaderUnit?.myUnitID;
    }

    

    /// <summary>
    /// 현재 게임을 껐다가 켰을 때 이 전의 데이터를 저장해야 할 필요가 있을까? 굳이? 없어도 되는 함수긴 함. 마찬가지.
    /// </summary>
    public void SaveDeck() // 덱 리스트 설정 저장. 키 값만 저장. 동일한 키 값으로 저장하기 때문에 저장할 때 마다 덮어 씌워짐.
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
