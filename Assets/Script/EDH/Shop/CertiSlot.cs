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
public class CertiSlot : MonoBehaviour
{
    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    [SerializeField] private Button certiSlot;                  // 증명서 유닛 슬롯
    

    public void init(PickInfo pickInfo)
    {
        var stats = UnitDataManager.Instance.GetStats(pickInfo.ID);

        UnitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");

        certiSlot.onClick.RemoveAllListeners();

        certiSlot.onClick.AddListener(() =>
        {
            Debug.Log("a");

            CertiPurChaseSync.Instance.Init(pickInfo, this);

            UIController.Instance.PurchaseUIBox.SetActive(true);

        });
    }
}
