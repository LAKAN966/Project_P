using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;
using System.ComponentModel;
using static UnityEditor.Progress;


public class PurchaseSync : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_InputField InputAmount;   // 수량 입력칸


    public Button AddButton;        // 수량 추가 버튼
    public Button SubtractButton;   // 수량 빼기버튼

    public Button PurchaseButton;   // 아이템 구매 버튼

    private GameObject PurchaseUIBox; // 구매 

    public Item _Item;
    public ItemSlot iSlot;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    public PurchaseBoxSet purchaseBoxSet;
 

    public void Start()
    {
        InputAmount.text = "1"; // 기본 세팅.

        //버튼 누적 호출 스책 초기화
        AddButton.onClick.RemoveAllListeners();
        SubtractButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.RemoveAllListeners();

        AddButton.onClick.AddListener(() =>
        {
            int amount = int.Parse(InputAmount.text);
            Debug.Log(amount);
            if (amount >= 1)
            {
                amount += 1;
                InputAmount.text = amount.ToString();
            }
            else
            { Debug.Log("s"); }
        }
        );
        SubtractButton.onClick.AddListener(() =>
        {
            int amount = int.Parse(InputAmount.text);
            Debug.Log(amount);
            if (amount > 1)
            {
                amount -= 1;
                InputAmount.text = amount.ToString();
            }
            else { Debug.Log("s"); }

            if (amount < 0)
            {
                Debug.Log("f");
            }
        }
        );
        PurchaseButton.onClick.AddListener (PurchaseItem); // 구매
    }
    public void PurchaseItem()
    {
        Debug.Log(PlayerDataManager.Instance.player.gold + "이건 플레이어 골드");
        Debug.Log(_Item.Cost + "아이템 가격");
        int Cost = _Item.Cost;
        int Amount = int.Parse(InputAmount.text);
        if (_Item.Cost <= PlayerDataManager.Instance.player.gold)
        {
                PlayerDataManager.Instance.UseGold(Cost * Amount);
                if(iSlot.name == "Ticket")
                {
                    PlayerDataManager.Instance.AddTicket(Amount);
                    Debug.Log(PlayerDataManager.Instance.player.ticket);
                }
                if (iSlot.name == "UpgradeTool")
                {
                    PlayerDataManager.Instance.AddTribute(Amount);
                }
                if (iSlot.name == "BluePrint")
                {
                    PlayerDataManager.Instance.AddBluePrint(Amount);
                }
                ShoppingManager.Instance.ShowNowGold();
                purchaseBoxSet.TabClose();
        }
        else
        {
            NotEnough();
        }
    }
    public void NotEnough()
    {
        if (_Item.Cost > PlayerDataManager.Instance.player.gold)
        {
            NotEnoughBoxText.text = "골드가 부족합니다.";
            NotEnoughBox.SetActive(true);
            StartCoroutine(HideNotEnoughBox());
        }

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(3f); // 3초 대기
            NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }

    public void Init(Item item, ItemSlot slot)
    {
        _Item = item;
        iSlot = slot;
    }

}

