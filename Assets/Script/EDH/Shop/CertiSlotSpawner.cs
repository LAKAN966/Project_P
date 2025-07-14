using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CertiSlotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CertiSlot;
    [SerializeField] private Transform Content;


    private void Start()
    {
        GetCertiUnit();
    }

    private void CreateCard(PickInfo pick, Transform parent) //카드 슬롯 생성
    {
        GameObject go = Instantiate(CertiSlot, parent);
        CertiSlot slot = go.GetComponent<CertiSlot>();
        slot.init(pick);
    }

    public void GetCertiUnit() //함수이름 변ㄱ뎡
    {
        Dictionary<int, PickInfo> pickInfoDict = PickUpListLoader.Instance.GetAllPickList();
        Debug.Log(pickInfoDict.Count + "커티유닛 정보");
        // 딕셔너리에서 포문
        foreach (KeyValuePair<int, PickInfo> item in pickInfoDict)
        {
            Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            CreateCard(item.Value, Content);
        }
    }
}
