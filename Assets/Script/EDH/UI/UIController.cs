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
    public TMP_Text GoldAmount;              //골드 표기

    //버튼
    public Button UnitManagementButton;  //덱빌딩
    

    private void Start()
    {
        Main.SetActive(true);
        SetButton();
    }


    public void UnitManageActive()
    {
        DeckBuild.SetActive(true);
        UIDeckBuildManager.instance.Init();
    }

    public void SetButton()
    {
        UnitManagementButton.onClick.AddListener(UnitManageActive);
    }

    public void SetDesecription(bool open)
    {
        DescriptionBox.SetActive(open);
    }
    public void ShowNowGold()
    {
        GoldAmount.text = PlayerDataManager.Instance.player.gold.ToString();
    }
}
