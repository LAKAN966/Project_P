using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleData
{
    public int ID;
    public string Name;
}

public class TagManager
{
    private static Dictionary<int, string> dataMap;

    static TagManager()
    {
        LoadCSV();
    }

    private static void LoadCSV()
    {
        dataMap = new Dictionary<int, string>();

        TextAsset csvFile = Resources.Load<TextAsset>("Data/EnumID");
        if (csvFile == null)
        {
            Debug.LogError("EnumID.csv 파일을 찾을 수 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string[] tokens = lines[i].Split(',');
            if (tokens.Length < 2) continue;

            if (int.TryParse(tokens[0], out int id))
            {
                string name = tokens[1].Trim();
                dataMap[id] = name;
            }
        }
    }

    public static string GetNameByID(int id)
    {
        return dataMap.TryGetValue(id, out var name) ? name : null;
    }

    public static Dictionary<int, string> GetAll()
    {
        return dataMap;
    }
}
