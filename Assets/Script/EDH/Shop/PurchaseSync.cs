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

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    private PurchaseBoxSet purchaseBoxSet;
    //public ItemSlot ItemSlot;

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
        PurchaseButton.onClick.AddListener(() => Purchase()); // 구매
    }
    public void Purchase()
    {
        int Cost = _Item.Cost;//
        int Amount = int.Parse(InputAmount.text);
        if (_Item.Cost < PlayerDataManager.Instance.player.gold)
        {
            if (_Item.ID == 101)
            {
                PlayerDataManager.Instance.UseGold(Cost * Amount);
                PlayerDataManager.Instance.AddTicket(Amount);
                ShoppingManager.Instance.ShowNowGold();
                purchaseBoxSet.TabClose();
            }
            if (_Item.ID == 102)
            {
                PlayerDataManager.Instance.UseGold(Cost * Amount);
                PlayerDataManager.Instance.AddTribute(Amount);
                ShoppingManager.Instance.ShowNowGold();
                purchaseBoxSet.TabClose();
            }
            if (_Item.ID == 103)
            {
                PlayerDataManager.Instance.UseGold(Cost * Amount);
                PlayerDataManager.Instance.AddBluePrint(Amount);
                ShoppingManager.Instance.ShowNowGold();
                purchaseBoxSet.TabClose();
            }
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
}

