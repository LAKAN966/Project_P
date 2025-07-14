using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInit : MonoBehaviour
{
    public Button goldBtn;
    public Button mainBtn;

    public GameObject goldStage;
    public GameObject mainStage;
    public GameObject infoPanel;

    public void OnDisable()
    {
        OnMainBtn();
    }
    public void OnMainBtn()
    {
        goldStage.SetActive(false);
        mainStage.SetActive(true);
        infoPanel.SetActive(false);
    }
    public void OnGoldBtn()
    {
        goldStage.SetActive(true);
        mainStage.SetActive(false);
        infoPanel.SetActive(false);
    }

}
