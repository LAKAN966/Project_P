using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectButton : MonoBehaviour
{
    public int buildingIndex;

    public GameObject buildConfirmPanel; // 확인용 패널
    public Button confirmButton;         // 패널 안의 확인 버튼
    public GameObject buildListUI;
    public Image buildImg;
    public TMP_Text goldText;
    public TMP_Text blueprintText;

    private Image img;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        img = GetComponent<Image>();
    }

    private void Start()
    {
        img.sprite = BuildManager.Instance.GetBuildingSprite(BuildManager.Instance.buildings[buildingIndex].imageName);
        buildConfirmPanel.SetActive(false); // 처음엔 꺼두기
    }

    private void OnClick()
    {
        buildConfirmPanel.SetActive(true);

        buildImg.sprite = BuildManager.Instance.GetBuildingSprite(BuildManager.Instance.buildings[buildingIndex].imageName);
        goldText.text = $"{PlayerDataManager.Instance.player.gold} / {BuildManager.Instance.buildings[buildingIndex].gold}";
        blueprintText.text = $"{PlayerDataManager.Instance.player.bluePrint} / {BuildManager.Instance.buildings[buildingIndex].blueprint}";

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            //if (PlayerDataManager.Instance.player.gold < BuildManager.Instance.buildings[buildingIndex].gold
            //|| PlayerDataManager.Instance.player.bluePrint < BuildManager.Instance.buildings[buildingIndex].blueprint)
            //{
            //    HQResourceUI.Instance.ShowLackPanel();
            //    buildConfirmPanel.SetActive(false);
            //    return;
            //}
            //자원 모자라면 판넬 띄우는 코드, 추후 주석 삭제 필요

            //자원 감소 코드 추가 필요
            BuildManager.Instance.BuildSelected(buildingIndex);
            buildConfirmPanel.SetActive(false);
            buildListUI.SetActive(false);
        });
    }
}
