using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UITowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerParent;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private TextMeshProUGUI entryCounts;

    public void SetTower()
    {
        foreach (Transform child in towerParent.transform)
            Destroy(child.gameObject);

        List<int> raceIDs = StageDataManager.Instance.GetAllTowerStageData()
            .Values
            .Select(s => s.RaceID)
            .Distinct()
            .ToList();

        foreach(int raceID in raceIDs)
        {
            GameObject go = Instantiate(towerPrefab, towerParent.transform);
            UITowerSlot slot = go.GetComponent<UITowerSlot>();
            slot.Setup(raceID);
        }
    }
}
