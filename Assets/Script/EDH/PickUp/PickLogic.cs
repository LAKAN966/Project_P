using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickLogic : MonoBehaviour
{

    public PickSlotSpawner pickSlotSpawner;
    public void DrawOne()
    {
        PickInfo pick = PickRandom();
        pickSlotSpawner.SpawnCardOne(pick);
    }
    public void DrawTen()
    {

        List<PickInfo> picks = new List<PickInfo>();
        for (int i = 0; i < 10; i++)
        {
            picks.Add(PickRandom());
        }
            pickSlotSpawner.SpawnCardTen(picks);
    }
    public PickInfo PickRandom()
    {
        Dictionary<int, PickInfo> pickInfoDict = PickUpListLoader.Instance.GetAllPickList();
        List<int> keys = pickInfoDict.Keys.ToList();

        int randomKey = keys[Random.Range(0, keys.Count)];

        PickInfo originalPick = pickInfoDict[randomKey];

        PickInfo randomPick = new PickInfo
        {
            ID = originalPick.ID,
            Name = originalPick.Name,
            Description = originalPick.Description,
            IsHero = Random.value < 0.1f
        };

        Debug.Log($"뽑기 결과: {(randomPick.IsHero ? "영웅!" : "일반")}");
        return randomPick;
    }
}

