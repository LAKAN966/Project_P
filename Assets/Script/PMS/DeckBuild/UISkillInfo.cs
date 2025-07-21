using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISkillInfo : MonoBehaviour
{
    [SerializeField] private GameObject skillInfoPanel;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void ShowSkillInfo(UnitStats data)
    {
        skillInfoPanel.SetActive(true);
    }

    public void HideSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }
}
