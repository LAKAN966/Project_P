using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CertiSlotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CertiSlot;
    [SerializeField] private Transform Content;


    public void SpawnCeriUnitCard(List<PickInfo> picks) 
    {
        foreach (PickInfo pickInfo in picks)
        {
            CreateCard(pickInfo, Content);
        }
    }


    private void CreateCard(PickInfo pick, Transform parent) //카드 슬롯 생성
    {
        GameObject go = Instantiate(CertiSlot, parent);
        UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
        slot.init(pick);
    }

    public void DrawCard()
    {
        List<PickInfo> picks = new List<PickInfo>();
        for (int i = 0; i < 10; i++)
        {
            picks.Add(GetAllCertiUnit());
        }
        SpawnCeriUnitCard(picks);
      }
    public PickInfo GetAllCertiUnit()
    {
        Dictionary<int, PickInfo> pickInfoDict = PickUpListLoader.Instance.GetAllPickList();
        List<int> keys = pickInfoDict.Keys.ToList();
        int randomKey = keys[Random.Range(0, keys.Count)];


        PickInfo originalPick = pickInfoDict[randomKey];

        PickInfo randomPick = new PickInfo
        {
            ID = originalPick.ID,
            Name = originalPick.Name,
        };

       
        return randomPick;
    }
}
