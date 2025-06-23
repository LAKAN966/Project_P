using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager
{
    //public DeckData currentDeck = new();

    //public bool TryAddUnitToDeck()
    //{

    //}
  

    
    public bool AddToDeck() // 덱 리스트에 추가
    {
        return true;
    }

    public bool RemoveFromDeck() // 덱 리스트에서 제거
    {
        return true;
    }

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
