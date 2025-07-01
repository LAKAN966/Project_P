using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitDataManager
{
    private static UnitDataManager instance;
    public static UnitDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UnitDataManager();
                instance.LoadUnitData();
            }
            return instance;
        }
    }

    private Dictionary<int, UnitStats> unitStatsDict = new();

    private UnitDataManager() { }

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
                ID = int.Parse(tokens[0]),
                Name = tokens[1],
                Description = tokens[2],
                RaceID = int.Parse(tokens[3]),
                IsHero = bool.Parse(tokens[4]),
                IsAOE = bool.Parse(tokens[5]),
                AttackRange = float.Parse(tokens[6]),
                Damage = float.Parse(tokens[7]),
                MaxHP = float.Parse(tokens[8]),
                MoveSpeed = float.Parse(tokens[9]),
                SpawnInterval = float.Parse(tokens[10]),
                Cost = int.Parse(tokens[11]),
                Hitback = int.Parse(tokens[12]),
                PreDelay = float.Parse(tokens[13]),
                PostDelay = float.Parse(tokens[14]),
                ModelName = tokens[15],
                AttackType = int.Parse(tokens[16]),
                Size = float.Parse(tokens[17]),
                SkillID = int.Parse(tokens[18]),
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
