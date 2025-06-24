using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;

    private UnitStats myStats;

    public void Setup(UnitStats stats)
    {
        myStats = stats;
        costText.text = stats.Cost.ToString();
        //iconImage.sprite = // 모델명에 따라 스프라이트 로딩
    }

    public UnitStats GetStats()
    {
        return myStats;
    }
}
