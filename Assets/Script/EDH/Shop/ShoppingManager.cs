using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    public GameObject Merchandise;
    public Transform Content;
    public TMP_Text GoldAmount;
    public ItemListLoader ItemListLoader;

    public static ShoppingManager Instance { get; private set; }
    void Start()
    {
        ItemListLoader.GetAllList();
        ItemSlot.Instantiate(Content);
        if (ItemListLoader.Instance == null)
        {
            Instantiate(ItemListLoader);
        }
        UIController.Instance.DescriptionBox.SetActive(false);
        UIController.Instance.PurchaseUIBox.SetActive(false);
    }

    public void ShowNowGold()
    {
        GoldAmount.text = PlayerDataManager.Instance.player.gold.ToString(); 
    }
}


   
