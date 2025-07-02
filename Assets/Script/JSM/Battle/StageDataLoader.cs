using UnityEngine;

public static class StageDataLoader
{
    public static StageData LoadByID(TextAsset csv, int stageID)
    {
        var lines = csv.text.Split('\n');
        var headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var values = lines[i].Split(',');

            int parsedID = int.Parse(values[0]);
            if (parsedID == stageID)
            {
                var data = new StageData
                {
                    ID = parsedID,
                    BaseDistance = float.Parse(values[1]),
                    EnemyBaseHP = int.Parse(values[2]),
                    //DropGold = int.Parse(values[6]),
                    //DropUnit = int.Parse(values[7]),
                    StageName = values[8],
                    TeaTime = float.Parse(values[9]),
                    ResetTime = float.Parse(values[10]),
                    EnemyHeroID = int.Parse(values[11]),
                    StageBG = values[12],
                };
                return data;
            }
        }
        Debug.LogError($"Stage ID {stageID} not found in CSV.");
        return null;
    }
}
