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


    public void SaveMyList() // 나의 보유 유닛 리스트 저장. 동일한 키값으로 이전의 값을 계속 덮어 씌우는 것이기 때문에, 뽑기 후에 진행.
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("MyUnitList", json);
    }

    public static MyUnitList LoadMyList() 
    {
        string json = PlayerPrefs.GetString("MyUnitList", "");
        if (string.IsNullOrEmpty(json)) return new MyUnitList();
        return JsonUtility.FromJson<MyUnitList>(json);
    }
}
