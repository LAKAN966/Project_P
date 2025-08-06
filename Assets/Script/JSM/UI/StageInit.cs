using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInit : MonoBehaviour
{
    public Button goldBtn;
    public Button mainBtn;
    public Button towerBtn;

    public GameObject goldStage;
    public GameObject mainStage;
    public GameObject infoPanel;
    public GameObject goldinfoPanel;
    public GameObject towerinfoPanel;
    public GameObject towerPanel;

    public static StageInit instance;
    private void Awake()
    {
        instance = this;
    }

    public void OnDisable()
    {
        OnMainBtn();
    }
    public void OnMainBtn()
    {
        goldStage.SetActive(false);
        mainStage.SetActive(true);
        infoPanel.SetActive(false);
        towerPanel.SetActive(false);
        goldinfoPanel.SetActive(false);
        towerinfoPanel.SetActive(false);
    }
    public void OnGoldBtn()
    {
        goldStage.SetActive(true);
        mainStage.SetActive(false);
        infoPanel.SetActive(false);
        towerPanel.SetActive(false);
        goldinfoPanel.SetActive(false);
        towerinfoPanel.SetActive(false);
    }
    public void OnTowerBtn()
    {
        goldStage.SetActive(false);
        mainStage.SetActive(false);
        infoPanel.SetActive(false);
        towerPanel.SetActive(true);
        UITowerManager.instance.Init();
        goldinfoPanel.SetActive(false);
        towerinfoPanel.SetActive(false);
    }

}
