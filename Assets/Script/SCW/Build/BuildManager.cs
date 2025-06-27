using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject buildListUI;
    [SerializeField] private List<BuildSlotUI> allSlots;
    public List<BuildingData> buildings;

    private BuildSlotUI selectedSlot;

    public static BuildManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void SelectSlot(BuildSlotUI slot)
    {
        selectedSlot = slot;
        buildListUI.SetActive(true);
    }

    public void BuildSelected(int buildingIndex)
    {
        if (selectedSlot == null) return;
        if (buildingIndex < 0 || buildingIndex >= buildings.Count) return;

        BuildingData building = buildings[buildingIndex];

        selectedSlot.SetBuilding(building);
        selectedSlot.Build();

        selectedSlot = null;
        buildListUI.SetActive(false);
    }
}
