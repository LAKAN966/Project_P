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

    public void ShowCertiUnitCard()
    {
        var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();

        List<PickInfo> Alliance = new();
        foreach (PickInfo pickInfo in PicklistDo)
        {
            if (!pickInfo.IsEnemy)
            {
                Alliance.Add(pickInfo);
                Debug.Log(Alliance + "존재");
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
