using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PickSlotSpawner : MonoBehaviour
{

    [SerializeField] private GameObject UnitICard;

    [SerializeField] private Transform Grid1; //1
    [SerializeField] private Transform Grid2; //10

    
    
    public void SpawnCardOne() //1개 뽑기 결과 생성
    {
        List<PickInfo> Alliance = new();
        var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
        foreach (Transform Child in Grid1)
        {
            Destroy(Child.gameObject);
        }

        foreach (PickInfo pickInfo in PicklistDo)
        {
            if (pickInfo.IsEnemy == false)
            {
                Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
                Alliance.Add(pickInfo);
            }
        }
        PickInfo pick = CreateCard(Alliance, Grid1);
        PlayerDataManager.Instance.AddUnit(pick.ID);
    }

    public void SpawnCardTen() //10개 뽑기 결과생성
    {
        List<PickInfo> Alliance = new();
        var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
        foreach (Transform Child in Grid2)
        {
            Destroy(Child.gameObject);
        }
        foreach (PickInfo pickInfo in PicklistDo )
        {
            if(pickInfo.IsEnemy == false)
            {
                Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
                Alliance.Add(pickInfo);
            }
        }
        List<PickInfo> picks = new List<PickInfo>();
        for (int i = 0; i < 10; i++)
        {
            picks.Add(CreateCard(Alliance, Grid2));
        }
        foreach (PickInfo pick in picks)
        {
            PlayerDataManager.Instance.AddUnit(pick.ID);
        }
    }
    private PickInfo CreateCard(List<PickInfo> Alliance, Transform parent) //카드 슬롯 생성
    {
        UnitCardSlot slot = new UnitCardSlot();
        PickInfo RanResult;

        int ranindex = Random.Range(0, Alliance.Count);
        RanResult = Alliance[ranindex];

        GameObject go = Instantiate(UnitICard, parent); //1
        slot = go.GetComponent<UnitCardSlot>(); //2
        slot.init(RanResult); //3
        
        return RanResult;
    }

}