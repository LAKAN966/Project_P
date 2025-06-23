using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MyUnitList 
{
    
    public List<UnitStats> myList = new();

    public bool AddUnit (int id) // 유닛 추가 메서드. 유닛의 동일한 아이디 확인해서 bool 값 반환.
    {
        if (myList.Exists(unit => unit.ID == id)) return false;

        myList.Add(new UnitStats { ID = id });
        return true;
    }

    public bool HasUnit(int id) // 보유중인지 체크
    {
        return myList.Exists(unit => unit.ID == id);

    }


    public void SaveMyList()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("OwnedCats", json);
    }

    public static MyUnitList LoadMyList() // 리스트 불러오기? 있어야 할듯? 저장을 어떻게 하는지에 따라서 추가 될 내용있음.
    {
        string json = PlayerPrefs.GetString("OwnedCats", "");
        if (string.IsNullOrEmpty(json)) return new MyUnitList();
        return JsonUtility.FromJson<MyUnitList>(json);
    }
}
