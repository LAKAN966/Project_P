using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PickUpListLoader : Singleton<PickUpListLoader>
{
    private void Start()
    {
        PickUps();
    }

    private Dictionary<int, PickInfo> PickListsDict = new();
    public void PickUps()
    {
        string path = Path.Combine(Application.dataPath, "Data/UnitData.csv");

        if (!File.Exists(path))
        {
            Debug.LogError($"UnitData.csv 파일이 없습니다: {path}");
            return;
        }
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            PickInfo pickinfo = new PickInfo
            {
                ID          =  int.Parse(tokens[0]),
                Name        = tokens[1],
                Description = tokens[2],
               IsHero      = bool.Parse(tokens[4])
            }
            ;
            PickListsDict[pickinfo.ID] = pickinfo;
        }
        Debug.Log($"픽업 데이터 로딩 완료: {PickListsDict.Count}개");
    }

    public Dictionary<int, PickInfo> GetAllPickList()
    { return PickListsDict; }
}



