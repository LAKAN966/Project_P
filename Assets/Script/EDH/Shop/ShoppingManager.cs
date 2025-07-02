using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using static UnityEditor.Progress;


public class ShoppingManager : MonoBehaviour
{
    public static ShoppingManager Instance { get; private set; }

    private Dictionary<int, Item> Item = new();

    
    
    public GameObject Merchandise;
    public Transform contentParent;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadItemData();
    }
    private void LoadItemData()
    {
        string path = Path.Combine(Application.dataPath, "Data/MarketItemData.csv");

        if (!File.Exists(path))
        {
            Debug.LogError($"MarketItemData.csv 파일이 없습니다: {path}");
            return;
        }
    }
    void PopulateShop()
    {
        // 기존 슬롯 제거
        //foreach (Transform child in contentParent)
        //{
        //    Destroy(child.gameObject);
        //}

        //// 아이템 수만큼 슬롯 생성
        //foreach (ItemData item in shopItems)
        //{
        //    GameObject slot = Instantiate(Merchandise, contentParent);
        //    // 슬롯 내부 UI 갱신
        //    Slot slotScript = slot.GetComponent<Slot>();
        //    slotScript.Setup(item);
        //}
    }
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public Sprite itemIcon;
        public int price;
    }
}
