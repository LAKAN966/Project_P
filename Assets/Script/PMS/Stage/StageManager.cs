using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform nodeParent;
    [SerializeField] private Button battleBtn;

    private StageNode[] nodes;

    private int selectedStageID = -1;

    public static StageManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        nodes = nodeParent.GetComponentsInChildren<StageNode>();
        SetStageIDs();
        SetupNodes();

        battleBtn.onClick.AddListener(OnClickEnterBattle);
    }

    private void SetStageIDs()
    {
        var stageDatas = StageDataManager.Instance.GetAllStageData(); // 딕셔너리로 만들어진 모든 스테이지 데이터. 딕셔너리 키 값은 스테이지 아이디

        var sortedStages = new List<StageData>(stageDatas.Values);
        sortedStages.Sort((a, b) => a.ID.CompareTo(b.ID)); // 스테이지 리스트 정렬 (a와 b 비교해서 오름차순으로)

        for (int i = 0; i < nodes.Length && i < sortedStages.Count; i++)
        {
            nodes[i].stageID = sortedStages[i].ID; // 노드에 스테이지 매칭
        }
    }

    private void SetupNodes()
    {
        foreach (var node in nodes)
        {
            var data = StageDataManager.Instance.GetStageData(node.stageID);
            if (data == null) continue;

            node.Init(data);
        }
    }

    public void SelectStage(int stageID)
    {
        selectedStageID = stageID;
        Debug.Log($"스테이지 {stageID} 선택됨");

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
