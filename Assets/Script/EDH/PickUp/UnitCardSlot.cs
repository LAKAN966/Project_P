using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;
using System;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        // 유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    Dictionary<int, PickInfo> PickInfo = PickUpListLoader.Instance.GetAllPickList();
    public void init(PickInfo pickInfo)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);


        pickInfo.Name = UnitICardNametext.text;    // 이름

        pickInfo.Uniticon = UnitIcon.sprite;       // 이미지
    }
}
