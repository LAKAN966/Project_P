using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private Dictionary<int, UnitStats> unitStatsDict = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadUnitData();
    }

    private void LoadUnitData()
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

            UnitStats stat = new UnitStats
            {
                //ID,Name,RaceID,IsHero,IsAOE,AttackRange,Damage,MaxHP,MoveSpeed,SpawnInterval,Cost,Hitback,PreDelay,PostDelay,ActiveSkillID,PassiveSkillID
                ID = int.Parse(tokens[0]),
                Name = tokens[1],
                RaceID = int.Parse(tokens[2]),
                IsHero = bool.Parse(tokens[3]),
                IsAOE = bool.Parse(tokens[4]),
                AttackRange = float.Parse(tokens[5]),
                Damage = float.Parse(tokens[6]),
                MaxHP = float.Parse(tokens[7]),
                MoveSpeed = float.Parse(tokens[8]),
                SpawnInterval = float.Parse(tokens[9]),
                Cost = int.Parse(tokens[10]),
                Hitback = int.Parse(tokens[11]),
                PreDelay = int.Parse(tokens[12]),
                PostDelay = int.Parse(tokens[13]),
            };

            unitStatsDict[stat.ID] = stat;
        }

        Debug.Log($"유닛 데이터 로딩 완료: {unitStatsDict.Count}개");
    }

    public UnitStats GetStats(int id)
    {
        if (unitStatsDict.TryGetValue(id, out var stats)) return stats;
        Debug.LogWarning($"ID {id}에 해당하는 유닛 데이터를 찾을 수 없습니다.");
        return null;
    }
}
