using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInfo : MonoBehaviour
{
    [Header("이미지/정보")]
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("보유 유닛 이미지/정보")]
    [SerializeField] private Image unitIcon;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("덱 슬롯 유닛 이미지/정보")]
    [SerializeField] private Image deckImage;


    public static UIUnitInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowInfo(UnitStats stats)
    {
        infoText.text = stats.ID.ToString();
    }

    public void ShowUnitIcon()
    {

    }
   
}
