using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private readonly string buildingCsvPath = "Assets/Data/BuildingData.csv";
    public List<BuildingData> buildings = new();

    private BuildSlotUI selectedSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        LoadBuildings();
    }
    public void SelectSlot(BuildSlotUI slot)
    {
        selectedSlot = slot;
    }

    public void BuildSelected(int buildingIndex)
    {
        if (selectedSlot == null) return;
        if (buildingIndex < 0 || buildingIndex >= buildings.Count) return;

        BuildingData building = buildings[buildingIndex];
        selectedSlot.Build(building);
        selectedSlot = null;
    }

    private void LoadBuildings()
    {
        buildings.Clear();

        if (!File.Exists(buildingCsvPath))
        {
            Debug.LogError($"[BuildManager] CSV not found at path: {buildingCsvPath}");
            return;
        }

        string[] lines = File.ReadAllLines(buildingCsvPath);
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            var parts = lines[i].Split(',');
            int id = int.Parse(parts[0].Trim());
            string displayName = parts[1].Trim();
            string imageName = parts[2].Trim();
            int raceId = int.Parse(parts[3].Trim());
            int gold = int.Parse(parts[4].Trim());
            int blueprint = int.Parse(parts[5].Trim());

            buildings.Add(new BuildingData(id, displayName, imageName, raceId, gold, blueprint));
        }
    }
    public Sprite GetBuildingSprite(string imageName)
    {
        return Resources.Load<Sprite>($"Sprites/{imageName}");
    }
    public int GetBuildingCount()
    {
        return buildings.Count;
    }
}
