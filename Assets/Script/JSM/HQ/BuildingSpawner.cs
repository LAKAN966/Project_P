using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    public ScrollRect scrollRect;
    public GameObject buildListUI;
    public GameObject buildGospelUI;
    public GameObject levelUpPanel;

    private void Start()
    {
        for (int i = 0; i < BuildManager.Instance.count; i++)
        {
            GameObject newObj = Instantiate(prefabToSpawn, parentObject);
            newObj.name = $"{prefabToSpawn.name}_{i}";

            Button button = newObj.GetComponentInChildren<Button>();
            BuildSlotUI buildSlotUI = newObj.GetComponentInChildren<BuildSlotUI>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    buildSlotUI.Select();
                });
            }
            buildSlotUI.Level = BuildManager.Instance.buildingsList[i].level;
            newObj.GetComponentInChildren<BuildSlotUI>().buildListUI = buildListUI;
            newObj.GetComponentInChildren<BuildSlotUI>().buildGospelUI = buildGospelUI;
            newObj.GetComponentInChildren<BuildSlotUI>().levelUpPanel = levelUpPanel;
            newObj.GetComponentInChildren<BuildSlotUI>().slotID = i;
            if (BuildManager.Instance.buildingsList[i].buildingData != null)
            {
                buildSlotUI.Build(BuildManager.Instance.buildingsList[i].buildingData);
            }
        }
    }
}
