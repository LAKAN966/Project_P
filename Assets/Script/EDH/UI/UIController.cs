using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject StageInfo;             //스테이지 정보창
    public GameObject GospelPanel;           //
    public GameObject GospelConfirmPanel;    //
    public GameObject BuildConfirmPanel;     //건설 확인 창
    public GameObject DescriptionBox;        //아이템 설명창


    private void Start()
    {
        Main.SetActive(true);
    }
}
