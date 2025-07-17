using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ItemListLoader : MonoBehaviour 
{
    private static ItemListLoader _instance;

    public static ItemListLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemListLoader();
            }
            return _instance;
        }
        
    }
    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            LoadItemData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private ItemListLoader GetInstance()
    {
        return Instance;
    }

    private Dictionary<int, Item> itemListsDict = new();
    private void LoadItemData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/MarketItemData");
        if (csvFile == null)
        {
            Debug.LogError("MarketItemData.csv 파일이 Resources/Data 폴더에 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            Item item = new Item
            {
                ID = int.Parse(tokens[0]),
                Name = tokens[1],
                Cost = int.Parse(tokens[2]),
                Description = tokens[3] 
            };

            itemListsDict[item.ID] = item;
        }

        Debug.Log($"아이템 데이터 로딩 완료: {itemListsDict.Count}개");
    }

    public Dictionary<int, Item> GetAllList()
    { return itemListsDict; }
}