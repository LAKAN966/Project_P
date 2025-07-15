using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CertiSlotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CertiSlot;
    [SerializeField] private Transform Content;


    private void Start()
    {
        ShowCertiUnitCard();
    }


    //public void ShowCertiUnitCard() 
    //{
    //    List<PickInfo> Alliance = new();
    //    var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
    //    //foreach (Transform Child in Content)
    //    //{
    //    //    Destroy(Child.gameObject);
    //    //}
    //    foreach (PickInfo pickInfo in PicklistDo)
    //    {
    //        if (pickInfo.IsEnemy == false)
    //        {
    //            Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
    //            Alliance.Add(pickInfo);
    //        }
    //    }
    //    List<PickInfo> picks = new List<PickInfo>();
    //    //for (int i = 0; i < Alliance.Count; i++)

    //    while(Alliance.Count > 0) 
    //    {
    //        picks.Add(CreateCard(Alliance, Content));
    //        Debug.Log(Alliance + "존재");
    //    }
    //    Debug.Log (CreateCard(PicklistDo, Content) + "카드생성");
    //}
    //private PickInfo CreateCard(List<PickInfo> Alliance, Transform parent) //카드 슬롯 생성
    //{
    //    CertiSlot slot = new CertiSlot();
    //    PickInfo RanResult;

    //    int ranindex = UnityEngine.Random.Range(0, Alliance.Count);
    //    RanResult = Alliance[ranindex];
    //    Alliance.Remove(RanResult);

    //    GameObject go = Instantiate(CertiSlot, parent); //1
    //    slot = go.GetComponent<CertiSlot>(); //2
    //    slot.init(RanResult); //3

    //    return RanResult;
    //}

    public void ShowCertiUnitCard()
    {
        var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();

        List<PickInfo> Alliance = new();
        foreach (PickInfo pickInfo in PicklistDo)
        {
            if (!pickInfo.IsEnemy)
            {
                Alliance.Add(pickInfo);
            }
        }
                                                                                                                                                                                                
        foreach (var pick in Alliance)
        {
            CreateCard(pick, Content);
        }
    }

    private void CreateCard(PickInfo pickInfo, Transform parent)
    {
        GameObject go = Instantiate(CertiSlot, parent);
        CertiSlot slot = go.GetComponent<CertiSlot>();
        slot.init(pickInfo);
    }
}
