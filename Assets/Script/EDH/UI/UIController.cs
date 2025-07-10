using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : Singleton<UIController>
{
    //기본 Ui
    public GameObject Main;         //메인
    public GameObject Stage;        //스테이지
    public GameObject DeckBuild;    //덱빌드
    public GameObject Shop;         //상점
    public GameObject HQ;           //내실
    public GameObject Gotta;        //뽑기

    //Pannal
    public GameObject PurchaseUIBox;         //구매시 상자창
    public GameObject DescriptionBox;        //아이템 설명창
    public TMP_Text MainGoldAmount;              //골드 표기
    public TMP_Text StoreGoldAmount;

    //버튼
    public Button UnitManagementButton;  //덱빌딩
    public Button SelectStageButton;     //스테이지 선택
    public Button UnitGotchaBotton;      //유닛 뽑기
    public Button SacredPlaceButton;     //내실
    public Button StoreButton;           //상점


    private void Start()
    {
        Main.SetActive(true);
        SetButton();
        ShowNowGold();
        PlayerCurrencyEvent.OnGoldChange += value => ShowNowGold();
    }
    private void Update()
    {
        
    }

    public void SetButton()
    {
        UnitManagementButton.onClick.AddListener(UnitManageActive);
        SelectStageButton.onClick.AddListener(OpenStage);
        UnitGotchaBotton.onClick.AddListener(OpenGottcha);
        SacredPlaceButton.onClick.AddListener(OpenSacredPlace);
        StoreButton.onClick.AddListener(OpenShop);
    }

    public void UnitManageActive()
    {
        DeckBuild.SetActive(true);
        UIDeckBuildManager.instance.Init();
    }
    public void OpenStage()
    {
        Stage.SetActive(true);
        StageManager.instance.Init();
    }
    public void OpenGottcha()
    {
        Gotta.SetActive(true);
    }
    public void OpenSacredPlace()
    {
        HQ.SetActive(true);
    }
    public void OpenShop()
    {
        Shop.SetActive(true);
    }

    public void ShowNowGold()
    {
        MainGoldAmount.text = PlayerDataManager.Instance.player.gold.ToString();
        StoreGoldAmount.text = PlayerDataManager.Instance.player.gold.ToString();
    }
}