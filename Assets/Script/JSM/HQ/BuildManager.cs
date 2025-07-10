using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingState
{
    public BuildingData buildingData;
    public int level;
}
public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    private readonly string buildingCsvPath = "Assets/Data/BuildingData.csv";
    public List<BuildingData> buildings = new();
    public int count = 5;

    private BuildSlotUI selectedSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);

        for (int i = 0; i < count; i++)
        {
            PlayerDataManager.Instance.player.buildingsList.Add(new BuildingState { buildingData = null, level = 0 });
        }
        BuffManager.InitBuffs(UnitDataManager.Instance.GetRaceCount());
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


        BuildingData building = GetBuildingData(buildingIndex);
        selectedSlot.Build(building, 1);
        PlayerDataManager.Instance.player.buildingsList[selectedSlot.slotID].buildingData = building;
        PlayerDataManager.Instance.player.buildingsList[selectedSlot.slotID].level = 1;
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

            List<int> goldList = new List<int>();
            for (int j = 6; j <= 9; j++)
            {
                if (j < parts.Length && int.TryParse(parts[j].Trim(), out int value))
                    goldList.Add(value);
                else
                    goldList.Add(0);
            }
            List<int> costList = new List<int>();
            for (int j = 10; j <= 13; j++)
            {
                if (j < parts.Length && int.TryParse(parts[j].Trim(), out int value))
                    costList.Add(value);
                else
                    costList.Add(0);
            }
            List<int> orderByLevel = new List<int>();
            if (parts.Length > 14 && !string.IsNullOrWhiteSpace(parts[14]))
            {
                string[] rawValues = parts[14].Split(';');
                foreach (var raw in rawValues)
                {
                    if (int.TryParse(raw.Trim(), out int val))
                        orderByLevel.Add(val);
                    else
                        orderByLevel.Add(0);
                }
            }
            buildings.Add(new BuildingData(id, displayName, imageName, raceId, gold, blueprint, goldList, costList, orderByLevel));
        }
    }
    public Sprite GetBuildingSprite(string imageName)
    {
        return Resources.Load<Sprite>($"Sprites/{imageName}");
    }
    public int GetBuildingRaceID(int id)
    {
        return buildings[id].raceId;
    }
    public int GetBuildingCount()
    {
        return buildings.Count;
    }
    public int GetOrderLevel(int id, int level)
    {
        return buildings[id-1].orderByLevel[level-1];
    }
    public int GetNextLevel(int id, int layer)
    {
        var orders = buildings[id-1].orderByLevel;
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i] > layer)
                return i + 1;
        }
        return -1;
    }
    public BuildingData GetBuildingData(int id)
    {
        foreach (var building in buildings)
        {
            if (building != null && building.id == id)
            {
                return building;
            }
        }
        return null;
    }
}
