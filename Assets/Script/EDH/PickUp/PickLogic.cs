using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickLogic : MonoBehaviour
{

    public PickSlotSpawner pickSlotSpawner;
    public void DrawOne() //1뽑
    {
        pickSlotSpawner.SpawnCardOne();
    }
    public void DrawTen()//10뽑
    {
        pickSlotSpawner.SpawnCardTen();
    }
    public PickInfo PickRandom()// 랜덤뽑기 로직
    {

        List<PickInfo> PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();

        PickInfo randomPick = PicklistDo[(PicklistDo.Count - 1)]; // -1은 최상단 밸류 명칭 라인.

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

