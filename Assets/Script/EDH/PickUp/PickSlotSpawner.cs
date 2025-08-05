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
    public GotchaInit gotchaInit;

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
        PickInfo pick = CreateCard(Grid1, gotchaInit.state);
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
            picks.Add(CreateCard(Grid2, gotchaInit.state));
        }
    }
    private PickInfo CreateCard(Transform parent, int state) //카드 슬롯 생성
    {
        UnitCardSlot slot = new UnitCardSlot();
        PickInfo RanResult;
        List<PickInfo> heroCandidates = heroes;
        List<PickInfo> nonHeroCandidates = nonHeroes;

        if (state != -1)
        {
            heroCandidates = heroes.Where(h => h.raceId == state).ToList();
            nonHeroCandidates = nonHeroes.Where(n => n.raceId == state).ToList();
        }

        int total = heroCandidates.Count + nonHeroCandidates.Count;
        if (total == 0)
        {
            Debug.LogWarning($"state {state}에 해당하는 카드가 없습니다.");
            return null;
        }

        int rand = Random.Range(0, total);
        if (rand < heroCandidates.Count)
            RanResult = heroCandidates[Random.Range(0, heroCandidates.Count)];
        else
            RanResult = nonHeroCandidates[Random.Range(0, nonHeroCandidates.Count)];

        GameObject go = Instantiate(UnitICard, parent);
        slot = go.GetComponent<UnitCardSlot>();
        slot.init(RanResult);

        return RanResult;
    }
    public void ShowProbabilityTable()
    {
        int state = gotchaInit.state;

        // 기존 텍스트 제거
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        var heroCandidates = heroes;
        var nonHeroCandidates = nonHeroes;
        if (state != -1)
        {
            heroCandidates = heroes.Where(h => h.raceId == state).ToList();
            nonHeroCandidates = nonHeroes.Where(n => n.raceId == state).ToList();
        }

        int totalPie = heroCandidates.Count > 0 ? heroPie : 0;
        totalPie += nonHeroCandidates.Count > 0 ? normPie : 0;

        if (heroCandidates.Count > 0 && heroPie > 0)
        {
            float groupRate = heroPie / (float)totalPie;
            float unitRate = groupRate / heroCandidates.Count * 100f;

            foreach (var hero in heroCandidates)
                CreateText($"[영웅] {hero.Name} → {unitRate:F2}%");
        }

        if (nonHeroCandidates.Count > 0 && normPie > 0)
        {
            float groupRate = normPie / (float)totalPie;
            float unitRate = groupRate / nonHeroCandidates.Count * 100f;

            foreach (var norm in nonHeroCandidates)
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