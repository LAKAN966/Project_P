using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CertiPurChaseSync : Singleton<CertiPurChaseSync>
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button PurchaseButton;   // 아이템 구매 버튼

    public TMP_Text CertiCost;      // 증명서 슬롯 가격 텍스트
    public Pick pick;               // 증명서 양

    PlayerDataManager PlayerDataManager;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고

    public CertiSlot cSlot;
    public PickInfo Info;

    public void Start()
    {
        PurchaseButton.onClick.AddListener(PurchaseCertiUnit);
    }
    
    public void PurchaseCertiUnit()
    {
        int Amount = int.Parse(pick.PityCount.text);
        int Cost =  int.Parse(CertiCost.text);
        if (Amount > Cost ) 
        {
            Amount -= Cost;
            PlayerDataManager.AddUnit(Info.ID );
        }
    }

    public void NotEnough()
    {
        int Amount = int.Parse(pick.PityCount.text);
        int Cost = int.Parse(CertiCost.text);
        if (Amount < Cost)
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
    public void Init(PickInfo pickInfo,CertiSlot certiSlot)
    { Debug.Log("c");
        Info = pickInfo;
        cSlot = certiSlot;
    }
}
