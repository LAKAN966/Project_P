using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TextMeshProUGUI totalCost;

    public Button AddButton;        // 수량 추가 버튼
    public Button SubtractButton;   // 수량 빼기버튼

    public Button PurchaseButton;   // 아이템 구매 버튼

    public Item _Item;
    public ItemSlot iSlot;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    public PurchaseBoxSet purchaseBoxSet;

    private int AttemptLeft;            //남은 구매 수량

    public Action<int> PurchAct { get; set; }

    public void Start()
    {
        InputAmount.text = "1"; // 기본 세팅.

        //버튼 누적 호출 스택 초기화
        AddButton.onClick.RemoveAllListeners();
        SubtractButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.RemoveAllListeners();

        AddButton.onClick.AddListener(Add);            // 더하기 
        SubtractButton.onClick.AddListener(Subttract); // 빼기

        PurchaseButton.onClick.AddListener(PurchaseItem); // 구매
    }
    public void Add()
    {
        int amount = int.Parse(InputAmount.text);
        int maxAmount = _Item.DailyBuy;

        if (amount < maxAmount)
        {
            amount++;
            InputAmount.text = amount.ToString();

            if (amount > maxAmount)// 최대치일때
                AddButton.interactable = false;
        }
        if (amount > 1)
            SubtractButton.interactable = true;
        UpdateTotal();
        SFXManager.Instance.PlaySFX(0);
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
        UpdateTotal();
        SFXManager.Instance.PlaySFX(0);
    }
    public void PurchaseItem()
    {
        AttemptLeft = _Item.DailyBuy;
        int Attempt = AttemptLeft;


        int Cost = _Item.Cost;
        int Amount = int.Parse(InputAmount.text);

        if ((ItemID)_Item.ID == ItemID.NormalRecruit)
            PurchaseLogic(Amount, Cost, PlayerDataManager.Instance.AddTicket);
        else if ((ItemID)_Item.ID == ItemID.BuildTool)
            PurchaseLogic(Amount, Cost, PlayerDataManager.Instance.AddTribute);
        else if ((ItemID)_Item.ID == ItemID.Blueprint)
            PurchaseLogic(Amount, Cost, PlayerDataManager.Instance.AddBluePrint);
        else if ((ItemID)_Item.ID == ItemID.SpecialRecruit)
            PurchaseLogic(Amount, Cost, PlayerDataManager.Instance.AddTicket);
        ShoppingManager.Instance.ShowNowGold();
        purchaseBoxSet.TabClose();
        SFXManager.Instance.PlaySFX(0);
    }
    public void PurchaseLogic(int amount, int cost, Action<int> ItemsP)
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
                ItemsP.Invoke(Amount);
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
        UpdateTotal();
    }
    public void UpdateTotal()
    {
        totalCost.text = NumberFormatter.FormatNumber((int.Parse(InputAmount.text)*_Item.Cost));
    }
}