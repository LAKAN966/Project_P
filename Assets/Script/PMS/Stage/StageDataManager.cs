using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageDataManager
{
    private static StageDataManager instance;
    public static StageDataManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = new StageDataManager();
            }
            return instance;
        }
    }
    private Dictionary<int, StageData> stageDic = new();

    void LoadStageData()
    {
        string path = Path.Combine(Application.dataPath, "Data/StageData.csv");

        if (File.Exists(path))
        {
            Debug.Log($"StageData.csv is null");
            return;
        }
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            StageData stage = new StageData();
            {
                
            };

            stageDic[stage.ID] = stage;
        }

        Debug.Log($"유닛 데이터 로딩 완료: {stageDic.Count}개");
    }
}
