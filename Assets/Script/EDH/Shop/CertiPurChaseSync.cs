using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CertiPurChaseSync : Singleton<CertiPurChaseSync>
{
    private UILoader uILoader;
    private UIManager uiManager;

    public Button PurchaseButton;   // 아이템 구매 버튼

    PlayerDataManager PlayerDataManager;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    public GameObject PurchaseCertiUnitBox;   // 구매UI 상자

    public CertiSlot cSlot;
    public PickInfo Info;

    public void Start()
    {
        PurchaseButton.onClick.AddListener(PurchaseCertiUnit);
    }

    public void PurchaseCertiUnit()
    {

        int Amount = PlayerDataManager.Instance.player.certi;
        int Cost = Info.warrant;

        if (Amount >= Cost)
        {
            PlayerDataManager.Instance.UseCerti(Cost);
            PlayerDataManager.Instance.AddUnit(Info.ID);
            Debug.Log(PlayerDataManager.Instance.AddUnit(Info.ID) + "이 유닛을 구매");
            ShoppingManager.Instance.ShowNowCertificate();
            PurchaseCertiUnitBox.SetActive(false);
        }
        else 
        {
            NotEnough();
        }
    }

    public void NotEnough()
    {
        UIController.Instance.NotEnoughBox.SetActive(true);
        NotEnoughBoxText.text = "증명서가 부족합니다.";
        StartCoroutine(HideNotEnoughBox());

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(1f); // 3초 대기
            UIController.Instance.NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }
    public void Init(PickInfo pickInfo, CertiSlot certiSlot)
    {
        Debug.Log("c");
        Info = pickInfo;
        cSlot = certiSlot;
    }
}
