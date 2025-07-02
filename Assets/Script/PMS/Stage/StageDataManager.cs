using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 스테이지 데이터 로드용
/// </summary>
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

    public void LoadStageData()
    {
        string path = Path.Combine(Application.dataPath, "Data/StageData.csv");

        if (!File.Exists(path))
        {
            Debug.Log($"StageData.csv is null");
            return;
        }
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            StageData stage = new StageData
            {
                ID = int.Parse(tokens[0]),
                Chapter = int.Parse(tokens[1]),
                BaseDistance = float.Parse(tokens[2]),
                EnemyBaseHP = int.Parse(tokens[3]),
                StageName = tokens[11],
                TeaTime = float.Parse(tokens[12]),
                ResetTime = float.Parse(tokens[13]),
                EnemyHeroID = int.Parse(tokens[14]),
                StageBG = tokens[15],
                ActionPoint = int.Parse(tokens[16])
            };

            if (!string.IsNullOrWhiteSpace(tokens[7]))
            {
                stage.firstRewardItemIDs = tokens[7].Split(';').Select(int.Parse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(tokens[8]))
            {
                stage.firstRewardAmounts = tokens[8].Split(';').Select(int.Parse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(tokens[9]))
            {
                stage.repeatRewardItemIDs = tokens[9].Split(';').Select(int.Parse).ToList();
            }

            if (!string.IsNullOrWhiteSpace(tokens[10]))
            {
                stage.repeatRewardAmounts = tokens[10].Split(';').Select(int.Parse).ToList();
            }


            stageDic[stage.ID] = stage;

        }

        Debug.Log($"스테이지 데이터 로딩 완료: {stageDic.Count}개");
    }

    public StageData GetStageData(int id)
    {
        if (stageDic.ContainsKey(id)) return stageDic[id];
        Debug.Log($"스테이지ID {id}에 해당하는 정보를 찾을 올 수 없습니다.");
        return null;
    }

    public Dictionary<int, StageData> GetAllStageData()
    {
        return stageDic;
    }
}
