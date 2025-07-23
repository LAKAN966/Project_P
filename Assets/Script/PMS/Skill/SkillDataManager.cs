using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    public static SkillDataManager Instance { get; private set; }

    private Dictionary<int, SkillData> skillDic = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }
    private void Init()
    {
        LoadSkillData();
    }

    private void LoadSkillData()
    {
        try
        {
            TextAsset csvFile = Resources.Load<TextAsset>("Data/HeroSkillData");
            if (csvFile == null)
            {
                throw new FileNotFoundException("HeroSkillData를 찾을 수 없습니다.");
            }

            string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                List<string> tokens = ParseCSVLine(lines[i]);

                SkillData skill = new SkillData()
                {
                    ID = int.Parse(tokens[0]),
                    Name = tokens[1],
                    SkillType = tokens[2],
                    Type = tokens[3],
                    Description = tokens[4],
                    EffectValue = tokens[5].Split(';').Select(int.Parse).ToList(),
                    TargetRaceID = int.Parse(tokens[6]),
                };
                skillDic[skill.ID] = skill;
            }
            Debug.Log($"스킬 데이터 {skillDic.Count}개 로드 성공");
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError(ex.Message);
        }

    }
    private static List<string> ParseCSVLine(string line)
    {
        var matches = Regex.Matches(line, @"(?<field>[^,""]+|""([^""]|"""")*"")(?=,|$)");
        List<string> result = new();

        foreach (Match match in matches)
        {
            string field = match.Groups["field"].Value;
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                field = field[1..^1].Replace("\"\"", "\"");
            }
            result.Add(field);
        }

        return result;
    }

    public SkillData GetSkillData(int id)
    {
        if (skillDic.TryGetValue(id, out var data)) return data;
        return null;
    }
}
