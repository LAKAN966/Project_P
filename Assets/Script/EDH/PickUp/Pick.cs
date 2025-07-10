using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;

public class Pick : MonoBehaviour
{
    public TextMeshProUGUI ShowTicketAmountText; // 소유하고 있는 티켓 수
    public TextMeshProUGUI PityCount;            // 모집 마일리지


    public Button PickOnce; //  1회 버튼
    public Button PickTen;  // 10회 버튼

    public Button RePickOne; //다시  1회 뽑기 버튼
    public Button RePickTen; //다시 10회 뽑기 버튼

    public GameObject PickOnePage; //  1회 뽑기 화면
    public GameObject PickTenPage; // 10회 뽑기 화면

    [SerializeField]
    private PickLogic pickLogic; // 뽑기 로직

   private int TicketAmount; // 티켓 수
   private int PickPoint;    // 뽑은 횟수

    
    private void Start()
    {
        PickOnce.onClick.AddListener(() => PickOneTime());       //  1회 뽑기
        PickTen.onClick.AddListener(()  => PickTenTimes());      // 10회 뽑기

        RePickOne.onClick.AddListener(() => PickOneTime());      //  1회 다시 뽑기
        RePickTen.onClick.AddListener(() => PickTenTimes());     // 10회 다시 뽑기

        
        PickPoint = 0;

        TicketAmount = PlayerDataManager.Instance.player.ticket;

        ShowTicketAmountText.text = TicketAmount.ToString();        // 현재 티켓 수

        PlayerCurrencyEvent.OnTicketChange += value => ShowTicketAmountText.text = TicketAmount.ToString();
        PityCount.text = PickPoint.ToString();                      // 현재 마일리지

        //외부에서 데이터 가져와야함. 플레이어에서 데이터 가져와야함.(완료)
    }


    public void PickOneTime()
    {
        Dictionary<int, PickInfo> PickInfo = PickUpListLoader.Instance.GetAllPickList();
        if (TicketAmount >= 1)
        {
            TicketAmount--; // 1장 소모
            TicketAmount = Math.Max(TicketAmount, 0); // 0검사

            if (TicketAmount < 0)
                PickPoint += 0;
            else PickPoint++;

            ShowTicketAmountText.text = TicketAmount.ToString();
            PityCount.text = PickPoint.ToString();

            PickOnePage.SetActive(true);
            pickLogic.DrawOne();
        }
        else
        {
            Debug.Log("티켓이 부족합니다");
        }
    }

    public void PickTenTimes()
    {
        var pickTable = PickUpListLoader.Instance.GetAllPickList();
        if (TicketAmount >= 10)
        {
            TicketAmount -= 10;// 10장 소모
            TicketAmount = Math.Max(TicketAmount, 0); // 예외처리

            if (TicketAmount < 0)
                PickPoint += 0;
            else PickPoint += 10;

            ShowTicketAmountText.text = TicketAmount.ToString();
            PityCount.text = PickPoint.ToString();

            PickTenPage.SetActive(true);
            pickLogic.DrawTen();
        }
        else
        {
            Debug.Log("티켓이 부족합니다");
        }
    }
}
