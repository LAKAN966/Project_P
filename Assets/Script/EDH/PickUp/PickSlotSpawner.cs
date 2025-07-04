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

    void Start()
    {
        SpawnCardOne();
        SpawnCardTen();
    }

    public void SpawnCardOne()
    {
        Dictionary<int, PickInfo> _PickInfo = PickUpListLoader.Instance.GetAllPickList();

        Debug.Log(_PickInfo.Count + "총아이템의 개수");

        foreach (var PickInfo in _PickInfo)
        {
            GameObject go = Instantiate(UnitCardSlot, PickOneTime);
            UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
            slot.init(PickInfo.Value);
        }
    }
    public void SpawnCardTen()
    {
        Dictionary<int, PickInfo> _PickInfo = PickUpListLoader.Instance.GetAllPickList();

        Debug.Log(_PickInfo.Count + "총아이템의 개수");

        foreach (var PickInfo in _PickInfo)
        {
            GameObject go = Instantiate(UnitCardSlot, PickTenTimes);
            UnitCardSlot slot = go.GetComponent<UnitCardSlot>();
            slot.init(PickInfo.Value);
        }
    }
}