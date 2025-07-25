using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    private static UIController instance;

    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIController();
            }
            return instance;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //기본 Ui
    public GameObject Main;         //메인
    public GameObject Stage;        //스테이지
    public GameObject DeckBuild;    //덱빌드
    public GameObject Shop;         //상점
    public GameObject HQ;           //내실
    public GameObject Gotta;        //뽑기

    //Pannal
    public GameObject PurchaseUIBox;         // 구매시 상자창
    public GameObject PurchaseCertiUnitBox;  // 유닛 교환시 상자창
    public GameObject DescriptionBox;        // 아이템 설명창
    public GameObject CertiDescriptionBox;   // 증명서 유닛 설명창
    public GameObject NotEnoughBox;          // 재화 부족 경고 창
    public TMP_Text MainGoldAmount;          // 골드 표기
    public TMP_Text StoreGoldAmount;         // 상점에서 골드 표기
    public TMP_Text NotEnoughBoxText;        // 재화 부족 경고 텍스트                                             

    //열기버튼
    public Button UnitManagementButton;  // 덱빌딩
    public Button SelectStageButton;     // 스테이지 선택
    public Button UnitGotchaBotton;      // 유닛 뽑기
    public Button SacredPlaceButton;     // 내실
    public Button StoreButton;           // 상점

    // 닫기 버튼
    public Button CloseGottcha;         // 뽑기 닫기
    public Button CloseUnitManage;      // 유닛관리 닫기
    public Button CloseShop;            // 상점닫기 
    public Button CloseHQ;              // 전초기지 닫기
    public Button Closestage;           // 침략 닫기


    private void Start()
    {
        Main.SetActive(true);
        SetButton();
        ShowNowGold();
        PlayerCurrencyEvent.OnGoldChange += value => ShowNowGold();
        SoundManager.Instance.PlayBGM(0);
    }

    public void SetButton()
    {
        UnitManagementButton.onClick.AddListener(UnitManageActive);
        SelectStageButton.onClick.AddListener(OpenStage);
        UnitGotchaBotton.onClick.AddListener(OpenGottcha);
        SacredPlaceButton.onClick.AddListener(OpenSacredPlace);
        StoreButton.onClick.AddListener(OpenShop);

        CloseGottcha.onClick.AddListener(OnExitBtn);
        CloseUnitManage.onClick.AddListener(OnExitBtn);
        CloseShop.onClick.AddListener(OnExitBtn);
        CloseHQ.onClick.AddListener(OnExitBtn);
        Closestage.onClick.AddListener(OnExitBtn);
    }

    public void UnitManageActive()
    {
        DeckBuild.SetActive(true);
        Main.SetActive(false);
        UIDeckBuildManager.instance.Init();
        SoundManager.Instance.PlayBGM(3);
    }
    public void OpenStage()
    {
        Stage.SetActive(true);
        Main.SetActive(false);
        StageManager.instance.Init();
    }
    public void OpenGottcha()
    {
        Gotta.SetActive(true);
        Main.SetActive(false);
        SoundManager.Instance.PlayBGM(2);
    }
    public void OpenSacredPlace()
    {
        HQ.SetActive(true);
        Main.SetActive(false);
        SoundManager.Instance.PlayBGM(4);
    }
    public void OpenShop()
    {
        Shop.SetActive(true);
        Main.SetActive(false);
        ShoppingManager.Instance.ShowNowCertificate();
        SoundManager.Instance.PlayBGM(5);
    }
    public void OnExitBtn()
    {
        if (Stage.activeSelf && DeckBuild.activeSelf)
        {
            DeckBuild.SetActive(false);
            return;
        }

        Main.SetActive(true);
        Stage.SetActive(false);
        DeckBuild.SetActive(false);
        Shop.SetActive(false);
        HQ.SetActive(false);
        Gotta.SetActive(false);
        SoundManager.Instance.PlayBGM(0);
    }
    public void ShowNowGold()
    {
        MainGoldAmount.text = NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.gold);
        StoreGoldAmount.text = NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.gold);
    }

    public void CloseGottaTab()
    {
        Gotta.SetActive(false);
        Main.SetActive(true);
    }
    public void CloseShopTab()
    {
        Shop.SetActive(false);
        Main.SetActive(true);
    }
    public void CloseUnitTab()
    {
        DeckBuild.SetActive(false);
        Main.SetActive(true);
    }
    public void CloseHQTab()
    {
        HQ.SetActive(false);
        Main.SetActive(true);
    }
    public void CloseStageTab()
    {
        Stage.SetActive(false);
        Main.SetActive(true);
    }
    public void AtemptNotEnoungh()
    {
        UIController.Instance.NotEnoughBox.SetActive(true);
        NotEnoughBoxText.text = "모든 구매 횟수를 모두 사용하셨습니다.";
        StartCoroutine(HideNotEnoughBox());

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(1f); // 3초 대기
            UIController.Instance.NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }
}
