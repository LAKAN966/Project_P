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
    [SerializeField] private TMP_Text ItemDescriptionText;  // 아이템 설명

    [SerializeField] private Button itemSlot;               // 아이템 슬롯
    [SerializeField] private Button PurchaseItemIcon;       // 아이템 아이콘
    [SerializeField] private Button PurchaseButton;         // 아이템 구매 버튼
    
    [SerializeField] private GameObject DescriptionBox;     // 아이템 설명 UI

    private PurchaseSync purchaseSync;
    private Item _Item;
    private ItemListLoader ItemListLoader; // 아이템 리스트 로더
  

    public void init(Item item)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        _Item = item;
        ItemNameText.text = item.Name;
        ItemCost.text = item.Cost.ToString();
        //ItemDescriptionText.text = item.Description.ToString();

        itemSlot.onClick.RemoveAllListeners();
        PurchaseItemIcon.onClick.RemoveAllListeners();
        PurchaseButton.onClick.RemoveAllListeners();

        itemSlot.onClick.AddListener(() =>  UIController.Instance.PurchaseUIBox.SetActive(true));
        PurchaseItemIcon.onClick.AddListener(() => UIController.Instance.DescriptionBox.SetActive(!DescriptionBox.activeSelf));
        PurchaseButton.onClick.AddListener(() => purchaseSync.Purchase());
    }
}