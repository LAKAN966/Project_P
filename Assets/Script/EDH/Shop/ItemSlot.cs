using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] public Image ItemIcon;                 // 아이템 이미지

    [SerializeField] private TMP_Text ItemCost;             // 아이템 가격
    [SerializeField] private TMP_Text ItemNameText;         // 아이템 이름
    [SerializeField] public TMP_Text NowAttempt;           // 남은 아이템 구매 횟수
    [SerializeField] public TMP_Text TotalAtempt;          // 아이템 구매 가능 횟수

    [SerializeField] private PurchaseSync purchaseSync;
    [SerializeField] private Button itemSlot;               // 아이템 슬롯


    private Item _Item;

    public void init(Item item)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        var items = GetItems(item);               // 아이콘 나오면 사용할 예정
        Debug.Log(items +"정보 들어옴");

        if(ItemIcon == null)
        {
            Debug.Log("ItemIcon이 비었음");
        }

        //if (item == null)
        //{
        //    Debug.Log("item이 비었음");
        //}

        if (ItemIcon.sprite == null)
        {
            Debug.Log("ItemIcon.sprite 비었음");
        }
        ItemIcon.sprite = Resources.Load<Sprite>($"Currency/{item.ItemIcon}");          // 아이콘 나오면 사용할 예정
        Debug.Log($"로드하려는 경로: Currency/{item.ItemIcon}");
        Debug.Log(GetItems(item).Count +"들어온 갯수");
        _Item = item;
        _Item.Name = item.Name;
        _Item.ItemIcon = item.ItemIcon;
        _Item.DailyBuy = item.DailyBuy;
        ItemCost.text = _Item.Cost.ToString();
        TotalAtempt.text = _Item.DailyBuy.ToString();

        ItemIcon.sprite = Resources.Load<Sprite>($"Currency/{item.ItemIcon}");

        if (ItemIcon != null)
        {
            Debug.Log("ItemIcon이  들어옴");
        }
        int parsedAmount = int.Parse(purchaseSync.InputAmount.text);
        NowAttempt.text = (_Item.DailyBuy - parsedAmount).ToString();


        itemSlot.onClick.RemoveAllListeners();

        itemSlot.onClick.AddListener(() =>
        {
            Debug.Log("a");

            if (purchaseSync == null)
            {
                purchaseSync = FindObjectOfType<PurchaseSync>();
            }
            purchaseSync.Init(_Item, this);

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
    }
}
