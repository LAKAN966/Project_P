using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    public int count = 5;
    public ScrollRect scrollRect;
    public GameObject buildListUI;
    public GameObject buildGospelUI;

    private void Start()
    {
        for (int i = 0; i < count; i++)
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
            newObj.GetComponentInChildren<BuildSlotUI>().buildListUI = buildListUI;
            newObj.GetComponentInChildren<BuildSlotUI>().buildGospelUI = buildGospelUI;
        }
    }
}
