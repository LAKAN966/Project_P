using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectButton : MonoBehaviour
{
    public int buildingIndex;
    public BuildManager manager;

    public GameObject buildConfirmPanel; // 확인용 패널
    public Button confirmButton;         // 패널 안의 확인 버튼

    private Image img;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        img = GetComponent<Image>();
    }

    private void Start()
    {
        img.sprite = manager.buildings[buildingIndex].upgradeSprites[0];
        buildConfirmPanel.SetActive(false); // 처음엔 꺼두기
    }

    private void OnClick()
    {
        if (manager == null) return;

        buildConfirmPanel.SetActive(true);

        // 이전 리스너 제거 후 새 리스너 추가 (중복 방지)
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            manager.BuildSelected(buildingIndex);
            buildConfirmPanel.SetActive(false);
        });
    }
}
