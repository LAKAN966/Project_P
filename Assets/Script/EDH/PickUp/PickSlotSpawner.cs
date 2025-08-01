using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PickSlotSpawner : MonoBehaviour
{

    [SerializeField] private GameObject UnitICard;

    [SerializeField] private Transform Grid1; //1
    [SerializeField] private Transform Grid2; //10
    [SerializeField] private int heroPie = 2;
    [SerializeField] private int normPie = 99;
    List<PickInfo> heroes;
    List<PickInfo> nonHeroes;
    List<PickInfo> Alliance = new List<PickInfo>();
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject textPrefab;
    private bool showPercent=false;


    private void Start()
    {
        foreach (PickInfo pickInfo in PickUpListLoader.Instance.GetAllPickList().Values.ToList())
        {
            if (pickInfo.IsEnemy == false)
            {
                Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
                Alliance.Add(pickInfo);
            }
        }
        heroes = Alliance.Where(p => p.IsHero).ToList();
        nonHeroes = Alliance.Where(p => !p.IsHero).ToList();
        
    }
    public void SpawnCardOne() //1개 뽑기 결과 생성
    {
        List<PickInfo> Alliance = new();
        var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
        foreach (Transform Child in Grid1)
        {
            Destroy(Child.gameObject);
        }

        //foreach (PickInfo pickInfo in PicklistDo)
        //{
        //    if (pickInfo.IsEnemy == false)
        //    {
        //        Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
        //        Alliance.Add(pickInfo);
        //    }
        //}
        PickInfo pick = CreateCard(Grid1);
        PlayerDataManager.Instance.AddUnit(pick.ID);
    }

    public void SpawnCardTen() //10개 뽑기 결과생성
    {
        List<PickInfo> Alliance = new();
        foreach (Transform Child in Grid2)
        {
            Destroy(Child.gameObject);
        }
        //var PicklistDo = PickUpListLoader.Instance.GetAllPickList().Values.ToList();
        //foreach (PickInfo pickInfo in PicklistDo )
        //{
        //    if(pickInfo.IsEnemy == false)
        //    {
        //        Debug.Log($"{pickInfo.IsEnemy}, {pickInfo.Name}");
        //        Alliance.Add(pickInfo);
        //    }
        //}
        List<PickInfo> picks = new List<PickInfo>();
        for (int i = 0; i < 10; i++)
        {
            picks.Add(CreateCard(Grid2));
        }
        foreach (PickInfo pick in picks)
        {
            PlayerDataManager.Instance.AddUnit(pick.ID);
        }
    }
    private PickInfo CreateCard(Transform parent) //카드 슬롯 생성
    {
        UnitCardSlot slot = new UnitCardSlot();
        PickInfo RanResult;

        int rand = Random.Range(0, heroPie + normPie);
        if(rand < heroPie-1)
            RanResult = heroes[Random.Range(0, heroes.Count)];
        else
            RanResult = nonHeroes[Random.Range(0, nonHeroes.Count)];

        GameObject go = Instantiate(UnitICard, parent); //1
        slot = go.GetComponent<UnitCardSlot>(); //2
        slot.init(RanResult); //3
        
        return RanResult;
    }
    public void ShowProbabilityTable()
    {
        if (showPercent) return;
        showPercent = true;

        // 기존 텍스트 제거
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        int totalPie = heroPie + normPie;

        if (heroes.Count > 0 && heroPie > 0)
        {
            float groupRate = heroPie / (float)totalPie;
            float unitRate = groupRate / heroes.Count * 100f;

            foreach (var hero in heroes)
                CreateText($"[영웅] {hero.Name} → {unitRate:F2}%");
        }

        if (nonHeroes.Count > 0 && normPie > 0)
        {
            float groupRate = normPie / (float)totalPie;
            float unitRate = groupRate / nonHeroes.Count * 100f;

            foreach (var norm in nonHeroes)
                CreateText($"[일반] {norm.Name} → {unitRate:F2}%");
        }
    }


    private void CreateText(string content)
    {
        GameObject textObj = Instantiate(textPrefab, contentParent);
        var text = textObj.GetComponent<TMP_Text>();
        if (text != null)
            text.text = content;
    }
}