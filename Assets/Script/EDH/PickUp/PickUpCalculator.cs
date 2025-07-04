using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class PickUpCalculator : MonoBehaviour
{
    UnitDataManager unitDataManager;

    private void Start()
    {
        unitDataManager = UnitDataManager.Instance;
    }

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
        }

    }
}
//리더인지 아닌지
//리더일 경우 리스트에서 하나 뽑는다.
//리더가 아닐경우 일반에서 모집.
//PlayerDataManager.AddUnit에 데이터 패싱 필요.


