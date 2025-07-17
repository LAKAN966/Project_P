using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UITowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerParent;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerInfoPanel;
    

    public static UITowerManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        this.gameObject.SetActive(true);
        SetTower();
    }
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

    public void SelectTower(int stageID)
    {
        towerInfoPanel.SetActive(true);
        towerInfoPanel.GetComponent<UITowerInfo>().SetTowerInfo(stageID);
    }
    
}
