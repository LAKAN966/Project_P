using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSlotSpanwer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    public int count = 3;
    public ScrollRect scrollRect;

    public GameObject confirmPanel;
    public Button confirmButton;
    public GameObject buildListUI;
    public Image buildImg;
    public TMP_Text goldText;
    public TMP_Text blueprintText;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = Instantiate(prefabToSpawn, parentObject);
            newObj.name = $"{prefabToSpawn.name}_{i}";
            newObj.GetComponentInChildren<BuildSelectButton>().buildingIndex = i;
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
