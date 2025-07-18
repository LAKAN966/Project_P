using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class GoldStage : MonoBehaviour
{
    public TextMeshProUGUI goldRewardCount;
    public TextMeshProUGUI goldStageLevel;
    public Button leftButton;
    public Button rightButton;
    public Button startButton;
    public Dictionary<int, StageData> goldStageData;
    private int currentGoldStage = 1;

    private void Start()
    {
        startButton.onClick.AddListener(OnClickEnterBattle);
        leftButton.onClick.AddListener(OnClickLeft);
        rightButton.onClick.AddListener(OnClickRight);
        goldRewardCount.text = PlayerDataManager.Instance.player.goldDungeonData.entryCounts.ToString()+" / 3";
        var goldStageDataByType = StageDataManager.Instance.GetAllGoldStageData();
        goldStageData = goldStageDataByType.Values
            .ToDictionary(data => data.Chapter, data => data);
        UpdateStageText();
        OnChangeLevel();
    }

    private void OnClickLeft()
    {
        if (currentGoldStage > 1)
        {
            currentGoldStage--;
            UpdateStageText();
            OnChangeLevel();
        }
    }

    private void OnClickRight()
    {
        if (currentGoldStage < goldStageData.Count)
        {
            currentGoldStage++;
            UpdateStageText();
            OnChangeLevel();
        }
    }

    private void UpdateStageText()
    {
        goldStageLevel.text = $"약탈 레벨 {currentGoldStage}";
    }
    private void OnClickEnterBattle()
    {
        int selectedStageID = goldStageData[currentGoldStage].ID;
        if (selectedStageID == -1) return;

        SceneManager.sceneLoaded += OnBattleSceneLoaded;//씬 로드 후에 실행되게 설정
        SceneManager.LoadScene("BattleScene");
        Debug.Log($"{selectedStageID} 입장");
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int selectedStageID = goldStageData[currentGoldStage].ID;
        if (scene.name == "BattleScene")
        {
            var normalDeck = PlayerDataManager.Instance.player.currentDeck.GetAllNormalUnit();
            var leaderDeck = PlayerDataManager.Instance.player.currentDeck.GetLeaderUnitInDeck();
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
            BattleManager.Instance.StartBattle(selectedStageID, normalDeck, leaderDeck, 2);
        }
    }

    private void OnChangeLevel()
    {
        startButton.interactable = (goldStageData[currentGoldStage].ID <= PlayerDataManager.Instance.player.goldDungeonData.lastClearStage + 1) || currentGoldStage == 1 ? true : false;
    }
}
