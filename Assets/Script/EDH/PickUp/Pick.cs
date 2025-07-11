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

   private int PickPoint;    // 뽑은 횟수


    private void Awake()
    {
        PickPoint = 0;
        PickOnce.onClick.AddListener(PickOneTime);       //  1회 뽑기
        PickTen.onClick.AddListener(PickTenTimes);      // 10회 뽑기
        RePickOne.onClick.AddListener(() => PickOneTime());      //  1회 다시 뽑기
        RePickTen.onClick.AddListener(() => PickTenTimes());     // 10회 다시 뽑기
        PlayerCurrencyEvent.OnTicketChange += value => ShowTicketAmountText.text = value.ToString();
        Debug.Log(PlayerDataManager.Instance.player.ticket.ToString() + "티켓 보유수");
    }

    private void OnEnable()
    {

        ShowTicketAmountText.text = PlayerDataManager.Instance.player.ticket.ToString();
        PityCount.text = PickPoint.ToString();                      // 현재 마일리지

        //외부에서 데이터 가져와야함. 플레이어에서 데이터 가져와야함.(완료)
    }


    public void PickOneTime()
    {
        Dictionary<int, PickInfo> PickInfo = PickUpListLoader.Instance.GetAllPickList();
        if (PlayerDataManager.Instance.player.ticket >= 1)
        {
            PlayerDataManager.Instance.UseTicket(1);

            PlayerDataManager.Instance.player.ticket = Math.Max(PlayerDataManager.Instance.player.ticket, 0); // 0검사
            Debug.Log(PlayerDataManager.Instance.player.ticket.ToString() + "티켓 보유수");
            PlayerCurrencyEvent.OnTicketChange -= value => ShowTicketAmountText.text = value.ToString();
            PickPoint++;

            ShowTicketAmountText.text = PlayerDataManager.Instance.player.ticket.ToString();
            PityCount.text = PickPoint.ToString();

            PickOnePage.SetActive(true);
            pickLogic.DrawOne();
        }
        else
        {
            Debug.Log("티켓이 부족합니다");
            Debug.Log(PlayerDataManager.Instance.player.ticket.ToString() + "티켓 보유수");
        }
    }

    public void PickTenTimes()
    {
        var pickTable = PickUpListLoader.Instance.GetAllPickList();
        if (PlayerDataManager.Instance.player.ticket >= 10)
        {
            PlayerDataManager.Instance.UseTicket(10);
            PlayerDataManager.Instance.player.ticket = Math.Max(PlayerDataManager.Instance.player.ticket, 0); // 예외처리
            Debug.Log(PlayerDataManager.Instance.player.ticket.ToString() + "티켓 보유수");
            PlayerCurrencyEvent.OnTicketChange -= value => ShowTicketAmountText.text = value.ToString();
            PickPoint += 10;

            ShowTicketAmountText.text = PlayerDataManager.Instance.player.ticket.ToString();
            PityCount.text = PickPoint.ToString();

            PickTenPage.SetActive(true);
            pickLogic.DrawTen();
        }
        else
        {
            Debug.Log("티켓이 부족합니다");
            Debug.Log(PlayerDataManager.Instance.player.ticket.ToString() + "티켓 보유수");
        }
    }
}
