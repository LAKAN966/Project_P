using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingLevelUpUI : MonoBehaviour
{
    private TextMeshProUGUI moneyTxt;
    private TextMeshProUGUI tributeTxt;
    private TextMeshProUGUI currentLevelTxt;
    private TextMeshProUGUI nextLevelTxt;
    public GameObject levelUpPanel;
    private BuildLevelUpConfirmUI buildLevelUpConfirmUI;
    private Button confirmButton;
    private Image buildImg;
    public BuildSlotUI buildSlotUI;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        buildLevelUpConfirmUI = levelUpPanel.GetComponent<BuildLevelUpConfirmUI>();
        moneyTxt = buildLevelUpConfirmUI.gold;
        tributeTxt = buildLevelUpConfirmUI.cost;
        currentLevelTxt = buildLevelUpConfirmUI.currentLevel;
        nextLevelTxt = buildLevelUpConfirmUI.nextLevel;
        confirmButton = buildLevelUpConfirmUI.confirmBtn;
        buildImg = buildLevelUpConfirmUI.buildingImg;
    }
    private void OnClick()
    {
        levelUpPanel.SetActive(true);

        buildImg.sprite = buildSlotUI.slotImage.sprite;
        Debug.Log(buildSlotUI.buildingID);
        moneyTxt.text = $"{PlayerDataManager.Instance.player.gold} / {BuildManager.Instance.buildings[buildSlotUI.buildingID-1].goldList[buildSlotUI.Level-1]}";
        tributeTxt.text = $"{PlayerDataManager.Instance.player.bluePrint} / {BuildManager.Instance.buildings[buildSlotUI.buildingID-1].costList[buildSlotUI.Level-1]}";
        currentLevelTxt.text = buildSlotUI.Level.ToString();
        nextLevelTxt.text = (buildSlotUI.Level+1).ToString();
        

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            //if (PlayerDataManager.Instance.player.gold < BuildManager.Instance.buildings[buildSlotUI.buildingID].GoldList[buildSlotUI.Level-1]}
            //|| PlayerDataManager.Instance.player.bluePrint < BuildManager.Instance.buildings[buildSlotUI.buildingID].CostList[buildSlotUI.Level-1])
            //{
            //    HQResourceUI.Instance.ShowLackPanel();
            //    buildConfirmPanel.SetActive(false);
            //    return;
            //}
            //자원 모자라면 판넬 띄우는 코드, 추후 주석 삭제 필요

            //자원 감소 코드 추가 필요
            buildSlotUI.LevelUp();
            if(buildSlotUI.Level == 5)
            {
                this.GetComponent<Button>().onClick.RemoveAllListeners();
                this.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            }
            levelUpPanel.SetActive(false);
        });
    }
}
