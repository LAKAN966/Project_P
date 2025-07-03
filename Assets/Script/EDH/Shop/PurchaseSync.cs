using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Button = UnityEngine.UI.Button;
using System.ComponentModel;


public class PurchaseSync : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_InputField InputAmount; //수량 입력칸

    public Button AddButton; //수량 추가 버튼
    public Button SubtractButton; // 수량 빼기버튼

    public GameObject PurchaseUIBox;

    private ItemListLoader itemListLoader;
    public Item _Item;

    public void Start()
    {
        InputAmount.text = "1";
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
    }
    public void Purchase()
    {
        int Cost = _Item.Cost;//
        int amount = int.Parse(InputAmount.text);
        if (_Item.Cost < PlayerDataManager.Instance.player.gold)
        {
            if(_Item.ID == 101)
            {
                PlayerDataManager.Instance.UseGold(Cost*amount);
                PlayerDataManager.Instance.AddTicket(amount);
            }
            if(_Item.ID == 102)
            {
                PlayerDataManager.Instance.UseGold(Cost * amount);
                PlayerDataManager.Instance.AddTicket(amount);
            }
        }
        else return;
    }
}

