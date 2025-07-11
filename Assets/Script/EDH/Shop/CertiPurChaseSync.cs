using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CertiPurChaseSync : Singleton<CertiPurChaseSync>
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button PurchaseButton;   // 아이템 구매 버튼

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
        int Cost = Info.warrant;
        if (Amount > Cost)
        {
            PlayerDataManager.UseCerti(Cost);
            PlayerDataManager.AddUnit(Info.ID);
        }
        else NotEnough();
    }

    public void NotEnough()
    {
        int Amount = int.Parse(pick.PityCount.text);
        int Cost = Info.warrant;
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
