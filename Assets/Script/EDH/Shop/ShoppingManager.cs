using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingManager : Singleton<ShoppingManager>
{
    public GameObject Merchandise;
    public ItemListLoader ItemListLoader;
    public Transform Content;
    
    public TMP_Text GoldAmount; //골드 상점용
    public TMP_Text PityCount; // 뽑기 횟수 =  증명서 갯수



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

    public void ShowNowCertificate()
    {
        int CertificateAmount = int.Parse(PityCount.text);
    }
}