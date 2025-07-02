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


public class ItemSlot : MonoBehaviour
{
    
    [SerializeField] private TMP_Text MerchandiseCost;   // 아이템 가격
    [SerializeField] private Button   SelfButton;        // 자기자신 
    
    private Item _Item;
    private PurchaseSync _PurchaseSync;
    public ItemListLoader ItemListLoader;
    public void init(Item item)
    {
        ItemListLoader.GetAllList();
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);
        _Item = item;

        SelfButton.onClick.RemoveAllListeners();
        SelfButton.onClick.AddListener(() => _PurchaseSync.Open(_Item.Cost));
    }
}
