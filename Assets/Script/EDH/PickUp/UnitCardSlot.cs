using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitICardNametext;        //유닛 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
    
    public void init(PickInfo pickInfo)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        pickInfo = (pickInfo != null) ? pickInfo : new PickInfo();

        UnitICardNametext.text = pickInfo.Name;      // 이름
        // UnitIcon.sprite = pickInfo.image     // 이미지
       }
}
