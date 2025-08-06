using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    private List<GameObject> spawnedCards = new();
    [SerializeField] private Transform centerPoint;
    [SerializeField] private GameObject revealButtonPrefab;
    [SerializeField] private Transform revealButtonParent;

    private GameObject revealButtonInstance;
    private Vector3 revealButtonWorldPos;
    public GameObject BtnList;



    [SerializeField] private GridLayoutGroup gridLayoutGroup;


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

    public void SpawnCard(int num)
    {
        foreach (Transform child in Grid2)
            Destroy(child.gameObject);
        spawnedCards.Clear();

        gridLayoutGroup.enabled = false;

        bool isGoodStuff=false;
        for (int i = 0; i < num; i++)
        {
            PickInfo pick = CreateCard(gotchaInit.state); // ❗ 카드 생성 X, PickInfo만
            if (pick.IsHero) isGoodStuff = true;
            GameObject card = Instantiate(UnitICard, centerPoint.position, Quaternion.identity, Grid2);
            card.GetComponent<UnitCardSlot>().SetPickInfo(pick);
            card.GetComponent<UnitCardSlot>().ShowBack();
            card.SetActive(false);
            spawnedCards.Add(card);
        }

        if (revealButtonInstance != null)
            Destroy(revealButtonInstance);

        revealButtonInstance = Instantiate(revealButtonPrefab, revealButtonParent);
        revealButtonWorldPos = revealButtonInstance.transform.position;

        if (isGoodStuff)
        {
            revealButtonInstance.GetComponent<Image>().color = Color.yellow;
        }

        Button btn = revealButtonInstance.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(RevealCards);
    }



    private PickInfo CreateCard(int state)
    {
        List<PickInfo> heroCandidates = heroes;
        List<PickInfo> nonHeroCandidates = nonHeroes;

        if (state != -1)
        {
            heroCandidates = heroes.Where(h => h.raceId == state).ToList();
            nonHeroCandidates = nonHeroes.Where(n => n.raceId == state).ToList();
        }

        int heroCount = heroCandidates.Count;
        int normCount = nonHeroCandidates.Count;

        int totalPie = (heroCount > 0 ? heroPie : 0) + (normCount > 0 ? normPie : 0);
        if (totalPie == 0)
        {
            Debug.LogWarning($"state {state}에 해당하는 카드가 없습니다.");
            return null;
        }

        // 1. 먼저 그룹(영웅/일반) 선택
        int groupRand = Random.Range(0, totalPie);
        bool isHeroGroup = (groupRand < (heroCount > 0 ? heroPie : 0));


        // 2. 그룹 내에서 무작위 PickInfo 선택
        PickInfo selected = null;
        if (isHeroGroup && heroCount > 0)
        {
            selected = heroCandidates[Random.Range(0, heroCount)];
        }
        else if (normCount > 0)
        {
            selected = nonHeroCandidates[Random.Range(0, normCount)];
        }

        if (selected != null)
        {
            QuestEvent.OnRecruit?.Invoke(1);
        }

        return selected;
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

    private void RevealCards()
    {
        if (revealButtonInstance != null)
        {
            Destroy(revealButtonInstance);
            revealButtonInstance = null;
        }
        foreach (var card in spawnedCards)
        {
            card.SetActive(true);
        }

        // 1. 정렬 위치 계산 (레이아웃 강제 갱신)
        gridLayoutGroup.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Grid2);

        List<Vector3> finalPositions = new();
        foreach (var card in spawnedCards)
            finalPositions.Add(card.transform.position);

        gridLayoutGroup.enabled = false;

        // 2. 카드 위치 초기화 → 버튼 위치
        foreach (var card in spawnedCards)
        {
            card.transform.position = revealButtonWorldPos; // 버튼 위치에서 생성
            card.transform.localScale = Vector3.zero;
        }

        // 3. Reveal 애니메이션
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            GameObject card = spawnedCards[i];
            card.GetComponent<UnitCardSlot>().Reveal();

            Sequence seq = DOTween.Sequence();

            // 1단계: 버튼 위치 → 중심점
            seq.Append(card.transform.DOMove(centerPoint.position, 0.3f).SetEase(Ease.InOutSine));

            // 2단계: 중심점 → 최종 위치
            seq.Append(card.transform.DOMove(finalPositions[i], 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.03f * i));

            // 동시에 크기 키우기
            card.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        // 4. 레이아웃 그룹 다시 켜고 싶으면 지연 호출
        DOVirtual.DelayedCall(1.2f, () =>
        {
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                int index = i;
                DOVirtual.DelayedCall(0.1f * i, () =>
                {
                    spawnedCards[index].GetComponent<UnitCardSlot>().Flip();
                });
            }
            float lastFlipDelay = 0.1f * spawnedCards.Count;
            DOVirtual.DelayedCall(lastFlipDelay + 1f, () => { 
                BtnList.SetActive(true);
                TutorialManager.Instance.OnEventTriggered("GotchaGotcha");
            });
        });
    }
    private void CreateText(string content)
    {
        GameObject textObj = Instantiate(textPrefab, contentParent);
        var text = textObj.GetComponent<TMP_Text>();
        if (text != null)
            text.text = content;
    }
}