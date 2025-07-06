using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSlotSpawner : MonoBehaviour
{
    public PickInfo PickInfo;

   
    [SerializeField] private GameObject UnitCardSlot;

    [SerializeField] private Transform PickOneTime;
    [SerializeField] private Transform PickTenTimes;

    public void SpawnCardOne(PickInfo pick) //1개 뽑기 결과 생성
    {
        Debug.Log(pick + "총아이템의 개수");
        CreateCard(PickInfo, PickOneTime);
    }
    public void SpawnCardTen(List<PickInfo> picks) //10개 뽑기 결과생성
    {
        foreach (PickInfo pickInfo in picks)
        {
            CreateCard(PickInfo, PickTenTimes);
        }
    }
    private void CreateCard(PickInfo pick, Transform parent) //카드 슬롯 생성
    {
        GameObject go = Instantiate(UnitCardSlot, parent);
        UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
        slot.init(pick);
    }
}