using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickLogic : MonoBehaviour
{

    public PickSlotSpawner pickSlotSpawner;
    public void DrawOne() //1뽑
    {
        PickInfo pick = PickRandom();
        pickSlotSpawner.SpawnCardOne(pick);
    }
    public void DrawTen()//10뽑
    {

        List<PickInfo> picks = new List<PickInfo>();
        for (int i = 0; i < 10; i++)
        {
            picks.Add(PickRandom());
        }
        pickSlotSpawner.SpawnCardTen(picks);
        
    }
    public PickInfo PickRandom()// 랜덤뽑기 로직
    {
        //Dictionary<int, PickInfo> pickInfoDict = PickUpListLoader.Instance.GetAllPickList();
        //List<int> keys = pickInfoDict.Keys.ToList();
       var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
 
        PickInfo randomPick = PicklistDo[(PicklistDo.Count -1)];

        PickInfo Result = new PickInfo
        {
            ID = randomPick.ID,
            Name = randomPick.Name,
            IsHero = UnityEngine.Random.value < 0.1f,
            IsEnemy = randomPick.IsEnemy
        };

        Debug.Log($"뽑기 결과: {(Result.IsHero ? "영웅!" : "일반")}");
        return Result;
    }
}

