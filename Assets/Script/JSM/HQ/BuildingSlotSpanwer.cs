using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BuildingSlotSpanwer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    private int count;
    public ScrollRect scrollRect;

    public GameObject confirmPanel;
    public Button confirmButton;
    public GameObject buildListUI;
    public Image buildImg;
    public TMP_Text goldText;
    public TMP_Text blueprintText;

    private void OnEnable()
    {
        setBuildingSlots();
    }
    private void OnDisable()
    {
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
    public void setBuildingSlots()
    {
        for (int i = 0; i < BuildManager.Instance.GetBuildingCount(); i++)
        {
            int index = BuildManager.Instance.buildings[i].id;
            if (BuildManager.Instance.buildingsList.Any(b => b.buildingData != null && b.buildingData.id == index))
            {
                Debug.Log(i);
                continue;
            }
            GameObject newObj = Instantiate(prefabToSpawn, parentObject);
            newObj.name = $"{prefabToSpawn.name}_{index}";
            newObj.GetComponentInChildren<BuildSelectButton>().buildingIndex = index;
            newObj.GetComponentInChildren<BuildSelectButton>().buildConfirmPanel = confirmPanel;
            newObj.GetComponentInChildren<BuildSelectButton>().confirmButton = confirmButton;
            newObj.GetComponentInChildren<BuildSelectButton>().buildListUI = buildListUI;
            newObj.GetComponentInChildren<BuildSelectButton>().buildImg = buildImg;
            newObj.GetComponentInChildren<BuildSelectButton>().goldText = goldText;
            newObj.GetComponentInChildren<BuildSelectButton>().blueprintText = blueprintText;
        }
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
