using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        // 유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    
    public void init(PickInfo pickInfo)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        UnitICardNametext.text = pickInfo.Name;

        Debug.Log(pickInfo.Name);

        UnitIcon.sprite = pickInfo.Uniticon;
    }
}
