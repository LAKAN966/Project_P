using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class UnitCardSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text ItemNameText;             // 아이템 이름

    [SerializeField] public Image UnitIcon;                     // 유닛 아이콘
   // [SerializeField] private GameObject PickCardSlot;           // 유닛 카드 슬롯
    private TMP_Text ID;                                        // 유닛 정보

    //private PickInfo _PickInfo;
    //private PickUpListLoader _PickUpListLoader;
    
    public void init(PickInfo pickInfo)
    {
        GameObject root = transform.root.gameObject;
        Debug.Log(root.name);

        pickInfo = (pickInfo != null) ? pickInfo : new PickInfo();
        
        ItemNameText.text = pickInfo.Name;      // 이름
        // UnitIcon.sprite = pickInfo.image     // 이미지
       }
}
