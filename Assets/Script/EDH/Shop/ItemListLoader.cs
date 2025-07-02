using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using static UnityEditor.Progress;
using Unity.VisualScripting;


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

    private ItemListLoader GetInstance()
    {
        return Instance;
    }

    private Dictionary<int, ItemList> itemListsDict = new();
    private void LoadItemData()
    {
        string path = Path.Combine(Application.dataPath, "Data/MarketItemData.csv");

        if (!File.Exists(path))
        {
            Debug.LogError($"MarketItemData.csv 파일이 없습니다: {path}");
            return;
        }
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            ItemList item = new ItemList
            {
                ID = int.Parse(tokens[0]),
                Name = tokens[1],
                Cost = int.Parse(tokens[2]),
            };
            itemListsDict[item.ID] = item;
        }
        Debug.Log($"유닛 데이터 로딩 완료: {itemListsDict.Count}개");
    }
    public Dictionary<int, ItemList> GetAllList()
    { return itemListsDict; }
}

//슬롯스트를 하나 만드렁서 슬롯을 넣어준다. 아이템 리스트 딕트 갯수만큼 만든다

//리스트를 순회하면서 슬롯에 넣어준다.






