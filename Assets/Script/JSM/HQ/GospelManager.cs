using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GospelManager : MonoBehaviour
{
    private readonly string gospelCsvPath = "Assets/Data/GospelData.csv";
    public Dictionary<int, List<List<GospelData>>> gospelMap = new();//id별 건물의 레이어별 데이터
    private Dictionary<int, HashSet<int>> selectedGospelIDsByBuildID = new();//id별 선택된 교리 데이터

    public static GospelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        LoadGospels();
    }

    public void LoadGospels()
    {
        gospelMap.Clear();
        string[] lines = File.ReadAllLines(gospelCsvPath);

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

            if (!gospelMap.ContainsKey(buildID))
                gospelMap[buildID] = new List<List<GospelData>>();

            var layers = gospelMap[buildID];
            if (layers.Count == 0 || layers[^1][0].order != order)
                layers.Add(new List<GospelData>());

            layers[^1].Add(new GospelData(id, buildID, order, cost, desc, name, statIndex, effectValue));
        }
    }

    public List<List<GospelData>> GetGospelsByBuildID(int buildID)
    {
        return gospelMap.ContainsKey(buildID) ? gospelMap[buildID] : new List<List<GospelData>>();
    }

    public bool IsSelected(int buildID, int gospelID)
    {
        return selectedGospelIDsByBuildID.ContainsKey(buildID) && selectedGospelIDsByBuildID[buildID].Contains(gospelID);
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
        foreach (var buildEntry in gospelMap.Values)
        {
            foreach (var layer in buildEntry)
            {
                foreach (var gospel in layer)
                {
                    if (gospel.id == gospelID)
                        return gospel;
                }
            }
        }

        Debug.LogWarning($"Gospel ID {gospelID}에 해당하는 데이터를 찾을 수 없습니다.");
        return null;
    }

    public void SelectGospel(int buildID, int gospelID)
    {
        if (!selectedGospelIDsByBuildID.ContainsKey(buildID))
            selectedGospelIDsByBuildID[buildID] = new HashSet<int>();
        selectedGospelIDsByBuildID[buildID].Add(gospelID);
        GospelData data = GetGospelByID(gospelID);
        BuffManager.UpdateBuffStat(BuildManager.Instance.GetBuildingRaceID(buildID), data.statIndex, data.effectValue);
    }
}
