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
        //goldText.text =
        //blueprintText.text = 추가 필요

        // 이전 리스너 제거 후 새 리스너 추가 (중복 방지)
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            BuildManager.Instance.BuildSelected(buildingIndex);
            buildConfirmPanel.SetActive(false);
            buildListUI.SetActive(false);
        });
    }
}
