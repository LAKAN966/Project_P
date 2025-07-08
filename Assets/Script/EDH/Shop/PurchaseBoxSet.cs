using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PurchaseBoxSet : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public TMP_Text ItemDescriptionText; // 아이템 설명 텍스트

    public GameObject NotEnoughBox;    // 재화 부족 경고
    public GameObject purchaseUIBox;   // 구매UI 상자
    public GameObject DescriptionBox;  // 아이템 설명 창



    public Button PurchaseItemIcon; // 상품 아이콘
    public Button CancelButton;     // 구매 취소 버튼



    public Item _Item;

    private void Start()
    {
        ItemDescriptionText.text = _Item.Description.ToString(); // 아이템 설명 동기화.
        CancelButton.onClick.AddListener(TabClose);
    }

    public void NotEnough()
    {
        if (_Item.Cost > PlayerDataManager.Instance.player.gold)
        {
            NotEnoughBoxText.text = "골드가 부족합니다.";
            NotEnoughBox.SetActive(true);
            StartCoroutine(HideNotEnoughBox());
        }

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(3f); // 3초 대기
            NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }

    public void TabClose()
    {
        purchaseUIBox.SetActive(false);
        DescriptionBox.SetActive(false);
    }
}
