//Onclick 으로 받아준다.

//구매처리


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Button = UnityEngine.UI.Button;
using Unity.VisualScripting;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UIElements;


public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text ItemCost;             // 아이템 가격
    [SerializeField] private TMP_Text ItemNameText;         // 아이템 이름
    [SerializeField] private PurchaseSync purchaseSync;
    [SerializeField] private Button itemSlot;               // 아이템 슬롯



    //private PurchaseSync purchaseSync;
    private Item _Item;
    private ItemListLoader ItemListLoader; // 아이템 리스트 로더
    private void Awake()
    {
        if (purchaseSync == null)
        {
            purchaseSync = FindObjectOfType<PurchaseSync>();
        }
    }

    public void init(Item item)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        _Item = item;
        ItemNameText.text = item.Name;
        ItemCost.text = item.Cost.ToString();


        itemSlot.onClick.RemoveAllListeners();

        itemSlot.onClick.AddListener(() =>
        {
            purchaseSync.Init(_Item, this);
            UIController.Instance.PurchaseUIBox.SetActive(true);
        });
    }
}
