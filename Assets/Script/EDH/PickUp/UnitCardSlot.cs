using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        // 유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘

    public void init(PickInfo Alliance)
    {
        UnitICardNametext.text = Alliance.Name;

        var stats = UnitDataManager.Instance.GetStats(Alliance.ID);

        Debug.Log(stats.ModelName);

        UnitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
    }
}
