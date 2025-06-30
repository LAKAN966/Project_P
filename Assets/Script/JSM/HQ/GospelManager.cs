using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GospelManager : MonoBehaviour
{
    private readonly string gospelCsvPath = "Assets/Data/GospelData.csv";
    public Dictionary<int, List<List<GospelData>>> gospelMap = new(); // BuildID → [Layer → Gospels]
    private Dictionary<int, HashSet<int>> selectedGospelIDsByBuildID = new(); // 추가됨

    public static GospelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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

    // ✅ 선택된 교리인지 확인
    public bool IsSelected(int buildID, int gospelID)
    {
        return selectedGospelIDsByBuildID.ContainsKey(buildID) && selectedGospelIDsByBuildID[buildID].Contains(gospelID);
    }

    // ✅ 선택된 order 기준 다음 선택 가능한 order 반환
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

    // ✅ 교리 선택 처리
    public void SelectGospel(int buildID, int gospelID)
    {
        if (!selectedGospelIDsByBuildID.ContainsKey(buildID))
            selectedGospelIDsByBuildID[buildID] = new HashSet<int>();

        selectedGospelIDsByBuildID[buildID].Add(gospelID);
    }
}
