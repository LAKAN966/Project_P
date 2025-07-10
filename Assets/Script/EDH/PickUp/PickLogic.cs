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
        Dictionary<int, PickInfo> pickInfoDict = PickUpListLoader.Instance.GetAllPickList();
        List<int> keys = pickInfoDict.Keys.ToList();

        int randomKey = keys[Random.Range(0, keys.Count)];

        PickInfo originalPick = pickInfoDict[randomKey];

        Debug.Log(originalPick.ID);
        Debug.Log(originalPick.Name);

        PickInfo randomPick = new PickInfo
        {
            ID = originalPick.ID,
            Name = originalPick.Name,
            IsHero = Random.value < 0.1f,
            IsEnemy = false,
        };

        Debug.Log($"뽑기 결과: {(randomPick.IsHero ? "영웅!" : "일반")}");
        return randomPick;
    }
}

