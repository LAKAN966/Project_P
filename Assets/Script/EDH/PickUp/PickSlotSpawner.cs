using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSlotSpawner : MonoBehaviour
{
    public PickInfo PickInfo;
    [SerializeField] private GameObject PickOne;

    [SerializeField] private Transform PickOneTime;
    [SerializeField] private Transform PickTenTimes;

    [SerializeField] private GameObject UnitCardSlot;


    public void SpawnCardOne(PickInfo pick)
    {
        Debug.Log(pick + "총아이템의 개수");
        CreateCard(PickInfo, PickOneTime);
    }
    public void SpawnCardTen(List<PickInfo> picks)
    {
        CreateCard(PickInfo, PickTenTimes);
    }
    private void CreateCard(PickInfo pick, Transform parent)
    {
        GameObject go = Instantiate(UnitCardSlot, parent);
        UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
        slot.init(pick);
    }
}