using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PickSlotSpawner : MonoBehaviour
{
    public PickInfo PickInfo;

    [SerializeField] private GameObject UnitICard;

    [SerializeField] private Transform Grid1; //1
    [SerializeField] private Transform Grid2; //10

    
    
    public void SpawnCardOne(PickInfo pick) //1개 뽑기 결과 생성
    {
       foreach(Transform Child in Grid1)
        {
            Destroy(Child.gameObject);
        }
        Debug.Log(pick + "총아이템의 개수");
        CreateCard(PickInfo, Grid1);
    }
    public void SpawnCardTen(List<PickInfo> picks) //10개 뽑기 결과생성
    {
        foreach (Transform Child in Grid2)
        {
            Destroy(Child.gameObject);
        }
        foreach (PickInfo pickInfo in picks)
        {
            CreateCard(PickInfo, Grid2);
        }
    }
    private void CreateCard(PickInfo pick, Transform parent) //카드 슬롯 생성
    {
        GameObject go = Instantiate(UnitICard, parent);
        UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
        slot.init(pick);
    }
  
}