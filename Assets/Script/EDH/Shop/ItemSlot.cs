using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image ItemIcon;                 // 아이템 이미지

    [SerializeField] private TMP_Text ItemCost;             // 아이템 가격
    [SerializeField] private TMP_Text ItemNameText;         // 아이템 이름
    [SerializeField] public TMP_Text NowAttempt;           // 남은 아이템 구매 횟수
    [SerializeField] public TMP_Text TotalAtempt;          // 아이템 구매 가능 횟수

    //[SerializeField] public TMP_InputField InputAmount;
    [SerializeField] private PurchaseSync purchaseSync;
    [SerializeField] private Button itemSlot;               // 아이템 슬롯


    private Item _Item;

    public void init(Item item)
    {
        _Item = item;
        ItemIcon.sprite = Resources.Load<Sprite>($"Currency/{_Item.ItemIcon}");          // 아이콘 나오면 사용할 예정

        Debug.Log(_Item +"정보 들어옴");

       
        Debug.Log($"로드하려는 경로: Currency/{item.ItemIcon}");
        Debug.Log(GetItems(item).Count +"들어온 갯수");

        ItemNameText.text = item.Name;
        ItemCost.text = _Item.Cost.ToString();
        TotalAtempt.text = _Item.DailyBuy.ToString();

        //int parsedAmount = int.Parse(purchaseSync.InputAmount.text);
        NowAttempt.text = _Item.DailyBuy.ToString(); 

        itemSlot.onClick.RemoveAllListeners();

        itemSlot.onClick.AddListener(() =>
        {
            Debug.Log("a");

            if (purchaseSync == null)
            {
                purchaseSync = FindObjectOfType<PurchaseSync>();
            }
            purchaseSync.Init(_Item, this);
            //if (InputAmount == null)
            //{
            //    InputAmount = FindObjectOfType<TMP_InputField>();
            //}

            UIController.Instance.PurchaseUIBox.SetActive(true);
            ItemSlotSet();
        });
    }

    private static Dictionary<int, Item> GetItems(Item item)
    {
        return ItemListLoader.Instance.GetAllList();
    }

    public void ItemSlotSet()
    {
        UIController.Instance.PurchaseUIBox.GetComponent<PurchaseBoxSet>()._Item = _Item;
        UIController.Instance.PurchaseUIBox.GetComponent<PurchaseBoxSet>().SetitemIcon(ItemIcon.sprite);  // 아이콘 나오면 사용할 예정
        //UIController.Instance.PurchaseUIBox.GetComponent<InputField>().text = InputAmount.ToString();
    }

}
