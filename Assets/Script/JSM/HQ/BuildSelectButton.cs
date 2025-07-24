using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectButton : MonoBehaviour
{
    public int buildingIndex;
    public List<int> allIDs = new(); // 이 빌딩이 갖는 모든 ID들

    public GameObject buildConfirmPanel; // 확인용 패널
    public Button confirmButton;         // 패널 안의 확인 버튼
    public GameObject buildListUI;
    public Image buildImg;
    public TMP_Text goldText;
    public TMP_Text blueprintText;
    private BuildingData building;

    public Image img;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Start()
    {
        building = BuildManager.Instance.GetBuildingData(buildingIndex);
        img.sprite = BuildManager.Instance.GetBuildingSprite(building.imageName);
        buildConfirmPanel.SetActive(false); // 처음엔 꺼두기

        allIDs.Clear();
        allIDs.Add(building.raceId);
        if (building.raceIDList != null)
            allIDs.AddRange(building.raceIDList);
    }

    private void OnClick()
    {
        buildConfirmPanel.SetActive(true);

        buildImg.sprite = BuildManager.Instance.GetBuildingSprite(building.imageName);
        goldText.text = $"{NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.gold)} / {NumberFormatter.FormatNumber(building.gold)}";
        blueprintText.text = $"{NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.bluePrint)} / {NumberFormatter.FormatNumber(building.blueprint)}";

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            if (PlayerDataManager.Instance.player.gold < building.gold
            || PlayerDataManager.Instance.player.bluePrint < building.blueprint)
            {
                HQResourceUI.Instance.ShowLackPanel();
                buildConfirmPanel.SetActive(false);
                return;
            }
            PlayerDataManager.Instance.UseGold(building.gold);
            PlayerDataManager.Instance.UseBluePrint(building.blueprint);
            HQResourceUI.Instance.UpdateUI();
            BuildManager.Instance.BuildSelected(buildingIndex);
            buildConfirmPanel.SetActive(false);
            buildListUI.SetActive(false);
        });
    }
}
