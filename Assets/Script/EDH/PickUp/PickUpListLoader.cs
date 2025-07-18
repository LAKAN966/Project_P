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
    private void Awake()
    {
        PickUps();
    }

    private Dictionary<int, PickInfo> PickListsDict = new();
    public void PickUps()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/UnitData"); // 확장자 생략
        if (csvFile == null)
        {
            Debug.LogError("Resources/Data/UnitData.csv 파일을 찾을 수 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            PickInfo pickinfo = new PickInfo
            {
                ID = int.Parse(tokens[0]),    // 유닛 ID
                Name = tokens[1],               // 유닛 이름
                Description = tokens[2],               // 설명
                IsHero = bool.Parse(tokens[4]),   // 영웅 여부
                Uniticon = tokens[5],               // 유닛 아이콘
                IsEnemy = bool.Parse(tokens[19]),  // 적 여부
            };

            PickListsDict[pickinfo.ID] = pickinfo;
        }

        Debug.Log($"픽업 데이터 로딩 완료: {PickListsDict.Count}개");
    }


    public Dictionary<int, PickInfo> GetAllPickList()
    { return PickListsDict; }
}



