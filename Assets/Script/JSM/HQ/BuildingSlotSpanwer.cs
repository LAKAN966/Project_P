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
        int index = 0;

        foreach (var pair in BuildManager.Instance.buildingDict.OrderBy(p => p.Value.id)) // id순 정렬
        {
            int buildID = pair.Key;

            // 이미 설치된 건물이면 스킵
            if (PlayerDataManager.Instance.player.buildingsList
                .Any(b => b.buildingData != null && b.buildingData.id == buildID))
            {
                Debug.Log(index);
                index++;
                continue;
            }

            GameObject newObj = Instantiate(prefabToSpawn, parentObject);
            newObj.name = $"{prefabToSpawn.name}_{index}";

            var button = newObj.GetComponentInChildren<BuildSelectButton>();
            button.buildingIndex = buildID; // 고유 ID
            button.buildConfirmPanel = confirmPanel;
            button.confirmButton = confirmButton;
            button.buildListUI = buildListUI;
            button.buildImg = buildImg;
            button.goldText = goldText;
            button.blueprintText = blueprintText;

            index++;
        }

        scrollRect.verticalNormalizedPosition = 0f;
    }

}
