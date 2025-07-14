using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GospelManager : MonoBehaviour
{
    public Dictionary<int, List<List<GospelData>>> gospelMap = new(); // buildID별 레이어 구조
    private Dictionary<int, GospelData> gospelByID = new(); // gospelID 기준 접근용

    public static GospelManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGospels();
    }

    public void LoadGospels()
    {
        gospelMap.Clear();
        gospelByID.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>("Data/GospelData");
        if (csvFile == null)
        {
            Debug.LogError("Resources/Data/GospelData.csv 파일이 존재하지 않습니다.");
            return;
        }

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var parts = lines[i].Split(',');

            int id = int.Parse(parts[0].Trim());
            int buildID = int.Parse(parts[1].Trim());
            int order = int.Parse(parts[2].Trim());
            int cost = int.Parse(parts[3].Trim());
            string desc = parts[4].Trim();
            string name = parts[5].Trim();
            int statIndex = int.Parse(parts[6].Trim());
            float effectValue = float.Parse(parts[7].Trim());

            var gospel = new GospelData(id, buildID, order, cost, desc, name, statIndex, effectValue);

            if (!gospelMap.ContainsKey(buildID))
                gospelMap[buildID] = new List<List<GospelData>>();

            var layers = gospelMap[buildID];
            if (layers.Count == 0 || layers[^1][0].order != order)
                layers.Add(new List<GospelData>());

            layers[^1].Add(gospel);
            gospelByID[id] = gospel;
        }

        Debug.Log($"복음 데이터 로딩 완료: {gospelMap.Count}개 빌딩에 대해 로딩됨");
    }

    public List<List<GospelData>> GetGospelsByBuildID(int buildID)
    {
        return gospelMap.ContainsKey(buildID) ? gospelMap[buildID] : new List<List<GospelData>>();
    }

    public bool IsSelected(int buildID, int gospelID)
    {
        return PlayerDataManager.Instance.player.selectedGospelIDsByBuildID.ContainsKey(buildID) &&
               PlayerDataManager.Instance.player.selectedGospelIDsByBuildID[buildID].Contains(gospelID);
    }

    public int GetCurrentSelectableOrder(int buildID)
    {
        if (!gospelMap.ContainsKey(buildID)) return 0;

        var layers = gospelMap[buildID];
        for (int order = 0; order < layers.Count; order++)
        {
            var layer = layers[order];
            bool anySelected = layer.Exists(g => IsSelected(buildID, g.id));
            if (!anySelected)
                return order;
        }
        return layers.Count;
    }

    public GospelData GetGospelByID(int gospelID)
    {
        if (gospelByID.TryGetValue(gospelID, out var data))
            return data;

        Debug.LogWarning($"Gospel ID {gospelID}에 해당하는 데이터를 찾을 수 없습니다.");
        return null;
    }

    public void SelectGospel(int buildID, int gospelID)
    {
        if (!PlayerDataManager.Instance.player.selectedGospelIDsByBuildID.ContainsKey(buildID))
            PlayerDataManager.Instance.player.selectedGospelIDsByBuildID[buildID] = new HashSet<int>();

        PlayerDataManager.Instance.player.selectedGospelIDsByBuildID[buildID].Add(gospelID);

        GospelData data = GetGospelByID(gospelID);
        if (data != null)
        {
            BuffManager.UpdateBuffStat(BuildManager.Instance.GetBuildingRaceID(buildID), data.statIndex, data.effectValue);
        }
    }
}
