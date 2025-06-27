using System.Collections.Generic;
using UnityEngine;

public static class WaveDataLoader
{
    public static List<WaveData> Load(TextAsset csv)
    {
        var result = new List<WaveData>();
        var lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 헤더 제외
        {
            var tokens = lines[i].Split(',');
            if (tokens.Length < 3 || string.IsNullOrWhiteSpace(tokens[0])) continue;

            result.Add(new WaveData
            {
                Time = float.Parse(tokens[0]),
                EnemyID = int.Parse(tokens[1]),
                OnTrigger = bool.Parse(tokens[2])
            });
        }

        return result;
    }
}
