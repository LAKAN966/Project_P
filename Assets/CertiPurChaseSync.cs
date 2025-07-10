using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CertiPurChaseSync : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button PurchaseButton;   // 아이템 구매 버튼

    public TMP_Text CertiCost;      // 증명서 슬롯 가격 텍스트
    public TMP_Text PityCount;      // 증명서 양

    PlayerDataManager PlayerDataManager;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고



    public void Start()
    {
        PurchaseButton.onClick.AddListener(PurchaseCertiUnit);
    }

    public void PurchaseCertiUnit()
    {
        int Count = int.Parse(PityCount.text);
        int Amount = int.Parse(CertiCost.text);
        if (Count > Amount)
        {
            PityCount.text = (Count - Amount).ToString();

            PlayerDataManager.AddUnit();
        }
    }

    public void NotEnough()
    {
        int Count = int.Parse(PityCount.text);
        int Amount = int.Parse(CertiCost.text);
        if (Count < Amount)
        {
            NotEnoughBoxText.text = "증명서가 부족합니다.";
            NotEnoughBox.SetActive(true);
            StartCoroutine(HideNotEnoughBox());
        }

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(3f); // 3초 대기
            NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }
}
