using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI chapterText;

    [Header("스테이지")]
    [SerializeField] private Transform nodeParent;
    [SerializeField] private StageNode stageNodePrefab;
    [SerializeField] private Button battleBtn;
    [SerializeField] private GameObject stageInfo;

    public Action<int> SetStageInfo;

    private int currentChapter = 1;
    private int selectedStageID = -1;

    
    public static StageManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        prevBtn.onClick.AddListener(() => ChangeChapter(-1));
        nextBtn.onClick.AddListener(() => ChangeChapter(1));
        battleBtn.onClick.AddListener(OnClickEnterBattle);

        battleBtn.gameObject.SetActive(false);

        SetStageInfo = (stageID) =>
        {
            stageInfo.SetActive(true);
        };

        UpdateStageUI();
    }

    public void ChangeChapter(int delta)
    {
        currentChapter += delta;

        var stageDataDic = StageDataManager.Instance.GetAllStageData();
        int minChapter = stageDataDic.Values.Min(x => x.Chapter);
        int maxChapter = stageDataDic.Values.Max(x => x.Chapter);

        currentChapter = Mathf.Clamp(currentChapter, minChapter, maxChapter);

        UpdateStageUI();
    }

    private void UpdateStageUI()
    {
        foreach(Transform child in nodeParent)
        {
            Destroy(child.gameObject);
        }

        chapterText.text = $"Chater_{currentChapter}";
        var stageDataDic = StageDataManager.Instance.GetAllStageData(); // 딕셔너리로 만들어진 모든 스테이지 데이터. 딕셔너리 키 값은 스테이지 아이디

        var chapterStages = stageDataDic.Values
            .Where(x => x.Chapter == currentChapter)
            .OrderBy(x => x.ID)
            .ToList();

        int nodeCount = chapterStages.Count;

        // 부모 영역의 크기
        RectTransform parentRect = nodeParent.GetComponent<RectTransform>();
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float marginX = 50f; // 왼쪽/오른쪽 여유
        float availableWidth = parentWidth - marginX * 2;

        float spacingX = availableWidth / (nodeCount - 1); // 노드 수 - 1 로 나누면 균등

        float topY = parentHeight * 0.6f;
        float bottomY = parentHeight * 0.1f;

        for (int i = 0; i < nodeCount; i++)
        {
            var stage = chapterStages[i];
            var node = Instantiate(stageNodePrefab, nodeParent);
            node.Init(stage);

            RectTransform nodePosition = node.GetComponent<RectTransform>();
            if (nodePosition != null)
            {
                float x = marginX + spacingX * i;
                float y = (i % 2 == 0) ? topY : bottomY;

                nodePosition.anchoredPosition = new Vector2(x, y);
            }
        }
    }
    

    public void SelectStage(int stageID)
    {
        selectedStageID = stageID;
        Debug.Log($"스테이지 {stageID} 선택됨");
        SetStageInfo?.Invoke(stageID);

        battleBtn.gameObject.SetActive(true);
    }

    private void OnClickEnterBattle()
    {
        if (selectedStageID == -1) return;

        int stageAP = StageDataManager.Instance.GetStageData(selectedStageID).ActionPoint;

        if (!PlayerDataManager.Instance.UseActionPoint(stageAP))
            return;

        SceneManager.sceneLoaded += OnBattleSceneLoaded;//씬 로드 후에 실행되게 설정
        SceneManager.LoadScene("BattleScene");
        Debug.Log($"{selectedStageID} 입장");
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            var normalDeck = DeckManager.Instance.GetAllDataInDeck();
            var leaderDeck = DeckManager.Instance.GetLeaderDataInDeck();
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
            BattleManager.Instance.StartBattle(selectedStageID, normalDeck, leaderDeck);
        }
    }

    public void ClearStage() // 클리어 스테이지 플레이어에 추가. 배틀 끝나고 불러오기.
    {
        PlayerDataManager.Instance.ClearStage(selectedStageID);
    }

    public void AddReward() // 스테이지 클리어 보상
    {
        var stageData = StageDataManager.Instance.GetStageData(selectedStageID);

        bool firstClear = !PlayerDataManager.Instance.HasClearedStage(selectedStageID);

        if (firstClear)
        {
            for (int i = 0; i < stageData.firstRewardItemIDs.Count; i++)
            {
                int itemID = stageData.firstRewardItemIDs[i];
                int amount = stageData.firstRewardAmounts[i];
                GiveReward(itemID, amount);
            }
        }

        else
        {
            for (int i = 0; i < stageData.repeatRewardItemIDs.Count; i++)
            {
                int itemID = stageData.repeatRewardItemIDs[i];
                int amount = stageData.repeatRewardAmounts[i];
                GiveReward(itemID, amount);
            }
        }

    }

    private void GiveReward(int itemID, int amount)
    {
        switch (itemID)
        {
            case 101:
                PlayerDataManager.Instance.AddGold(amount);
                break;

            case 102:
                PlayerDataManager.Instance.AddTicket(amount);
                break;

            case 103:
                PlayerDataManager.Instance.AddBluePrint(amount);
                break;
        }


    }
}
