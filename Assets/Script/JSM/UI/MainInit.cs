using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInit : MonoBehaviour
{
    public GameObject questPanel;
    public GameObject settingPanel;

    private void OnDisable()
    {
        questPanel.SetActive(false);
        settingPanel.SetActive(false);
    }
}
