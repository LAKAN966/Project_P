using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlotSpanwer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    public int count = 3;
    public ScrollRect scrollRect;

    public GameObject confirmPanel;
    public Button confirmButton;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = Instantiate(prefabToSpawn, parentObject);
            newObj.name = $"{prefabToSpawn.name}_{i}";
            newObj.GetComponentInChildren<BuildSelectButton>().buildingIndex = i;
            newObj.GetComponentInChildren<BuildSelectButton>().manager = BuildManager.Instance;
            newObj.GetComponentInChildren<BuildSelectButton>().buildConfirmPanel = confirmPanel;
            newObj.GetComponentInChildren<BuildSelectButton>().confirmButton = confirmButton;
        }
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
