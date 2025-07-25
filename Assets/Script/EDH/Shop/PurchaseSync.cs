using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using Button = UnityEngine.UI.Button;

public enum ItemID
{
    //101 =  일반 모집, 102 = 건설도구, 103 =  설계도, 104 = 특수모집

    NormalRecruit = 101,
    BuildTool = 102,
    Blueprint = 103,
    SpecialRecruit = 104
}
public class PurchaseSync : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_InputField InputAmount;   // 수량 입력칸

    public Button AddButton;        // 수량 추가 버튼
    public Button SubtractButton;   // 수량 빼기버튼

    public Button PurchaseButton;   // 아이템 구매 버튼

    private GameObject PurchaseUIBox; // 아이템 구매 

    public Item _Item;
    public ItemSlot iSlot;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    public PurchaseBoxSet purchaseBoxSet;

    private int AttemptLeft;            //남은 구매 수량
    int click = 0;

    public Action<int> PurchAct { get; set; }

    public void Start()
    {
        InputAmount.text = "1"; // 기본 세팅.

        //버튼 누적 호출 스택 초기화
        AddButton.onClick.RemoveAllListeners();
        SubtractButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.RemoveAllListeners();

        AddButton.onClick.AddListener(() =>
        {
            Add();
            click++;
            if (click >= 6)
            {
                AddButton.interactable = false;
                AddButton.interactable = true;
            }
        });

        SubtractButton.onClick.AddListener(Subttract);

        PurchaseButton.onClick.AddListener(PurchaseItem); // 구매
    }
    public void Add()
    {
        int amount = int.Parse(InputAmount.text);
        int maxAmount = _Item.DailyBuy;

        if (amount < maxAmount)
        {
            amount ++;
            InputAmount.text = amount.ToString();

            if (amount > maxAmount)// 최대치일때
                AddButton.interactable = false;
        }

        if (amount > 1)
            SubtractButton.interactable = true;
    }
    public void Subttract()
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
            Debug.Log("");
        }
    }
    public void PurchaseItem()
    {
        AttemptLeft = _Item.DailyBuy;
        int Attempt = AttemptLeft;


        int Cost = _Item.Cost;
        int Amount = int.Parse(InputAmount.text);

        if ((ItemID)_Item.ID == ItemID.NormalRecruit)
        {
            Debug.Log(Attempt + "일반 모집");
            if (AttemptLeft > 0)
            {
                if (int.Parse(InputAmount.text) > _Item.DailyBuy)
                {
                    Amount = AttemptLeft;
                    InputAmount.text = Amount.ToString();
                }
                if (PlayerDataManager.Instance.player.gold > int.Parse(InputAmount.text) * Cost)
                {
                    PlayerDataManager.Instance.UseGold(Cost * Amount);
                    PlayerDataManager.Instance.AddTicket(Amount);
                    AttemptLeft -= Amount;
                    Debug.Log(AttemptLeft + "남은 일반 모집 구매수량");

                    InputAmount.text = "1";

                    PurchAct.Invoke(Amount);
                    _Item.DailyBuy = AttemptLeft;
                }
                else
                {
                    UIController.Instance.GoldNotEnoungh();
                }
            }
            else
            {
                UIController.Instance.AtemptNotEnoungh();
            }
        }
        if ((ItemID)_Item.ID == ItemID.BuildTool)
        {
            Debug.Log(Attempt + "건설도구");
            if (AttemptLeft > 0)
            {
                if (int.Parse(InputAmount.text) > _Item.DailyBuy)
                {
                    Amount = AttemptLeft;
                    InputAmount.text = Amount.ToString();
                }
                if (PlayerDataManager.Instance.player.gold > int.Parse(InputAmount.text) * Cost)
                {
                    PlayerDataManager.Instance.UseGold(Cost * Amount);
                    PlayerDataManager.Instance.AddTribute(Amount);
                    AttemptLeft -= Amount;
                    InputAmount.text = "1";
                    Debug.Log(AttemptLeft + "남은 건설도구 구매수량");
                    PurchAct.Invoke(Amount);
                    _Item.DailyBuy = AttemptLeft;
                }
                else
                {
                    UIController.Instance.GoldNotEnoungh();
                }

            }
            else
            {
                UIController.Instance.AtemptNotEnoungh();
            }
        }
        if ((ItemID)_Item.ID == ItemID.Blueprint)
        {
            Debug.Log(Attempt + "설계도");
            if (AttemptLeft > 0)
            {
                if (int.Parse(InputAmount.text) > _Item.DailyBuy)
                {
                    Amount = AttemptLeft;
                    InputAmount.text = Amount.ToString();
                }
                if (PlayerDataManager.Instance.player.gold > int.Parse(InputAmount.text) * Cost)
                {
                    PlayerDataManager.Instance.UseGold(Cost * Amount);
                    PlayerDataManager.Instance.AddBluePrint(Amount);
                    AttemptLeft -= Amount;
                    InputAmount.text = "1";
                    Debug.Log(AttemptLeft + "남은 설계도 구매수량");
                    PurchAct.Invoke(Amount);
                    _Item.DailyBuy = AttemptLeft;
                }
                else
                {
                    UIController.Instance.GoldNotEnoungh();
                }
            }
            else
            {
                UIController.Instance.AtemptNotEnoungh();
            }
        }
        if ((ItemID)_Item.ID == ItemID.SpecialRecruit)
        {
            Debug.Log(Attempt + "특수모집");
            if (AttemptLeft > 0)
            {
                if (int.Parse(InputAmount.text) > _Item.DailyBuy)
                {
                    Amount = AttemptLeft;
                    InputAmount.text = Amount.ToString();
                }
                if (PlayerDataManager.Instance.player.gold > int.Parse(InputAmount.text) * Cost)
                {
                    PlayerDataManager.Instance.UseGold(Cost * Amount);
                    PlayerDataManager.Instance.AddTicket(Amount);
                    AttemptLeft -= Amount;
                    InputAmount.text = "1";
                    Debug.Log(AttemptLeft + "남은 특수모집 구매수량");
                    PurchAct.Invoke(Amount);
                    _Item.DailyBuy = AttemptLeft;
                }
                else
                {
                    UIController.Instance.GoldNotEnoungh();
                }
            }
            else
            {
                UIController.Instance.AtemptNotEnoungh();
            }

        }

        if (_Item.ID == (int)ItemID.NormalRecruit)
            Purchase2(Amount, Cost, PlayerDataManager.Instance.AddTicket);
        else if (_Item.ID == (int)ItemID.BuildTool)
            Purchase2(Amount, Cost, PlayerDataManager.Instance.AddTribute);
        else if (_Item.ID == (int)ItemID.Blueprint)
            Purchase2(Amount, Cost, PlayerDataManager.Instance.AddBluePrint);
        else if (_Item.ID == (int)ItemID.SpecialRecruit)
            Purchase2(Amount, Cost, PlayerDataManager.Instance.AddTicket);
        ShoppingManager.Instance.ShowNowGold();
        purchaseBoxSet.TabClose();
    }
    public void Purchase2(int amount, int cost, Action<int> ItemsP)
    {
        AttemptLeft = _Item.DailyBuy;
        int Attempt = AttemptLeft;

        int Cost = _Item.Cost;
        int Amount = int.Parse(InputAmount.text);

        if (AttemptLeft > 0)
        {
            if (int.Parse(InputAmount.text) > _Item.DailyBuy)
            {
                Amount = AttemptLeft;
                InputAmount.text = Amount.ToString();
            }
            if (PlayerDataManager.Instance.player.gold > int.Parse(InputAmount.text) * Cost)
            {
                PlayerDataManager.Instance.UseGold(Cost * Amount);
                PlayerDataManager.Instance.AddTicket(Amount);
                AttemptLeft -= Amount;
                InputAmount.text = "1";
                PurchAct.Invoke(Amount);
                _Item.DailyBuy = AttemptLeft;
            }
            else
            {
                UIController.Instance.GoldNotEnoungh();
            }
        }
        else
        {
            UIController.Instance.AtemptNotEnoungh();
        }
    }
    public void Init(Item item, ItemSlot slot)
    {
        Debug.Log("c");
        _Item = item;
        iSlot = slot;
    }
}