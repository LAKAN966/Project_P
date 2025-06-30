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
        infoImage.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
        infoText.text = ($"{stats.Name}\n{stats.MaxHP}\n{stats.Damage}"); // 어떤 정보가 표시 되는지 아직 다 안넣었음. 확인 후 수정.
    }

    public void ShowUnitIcon(UnitStats stats)
    {
        unitIcon.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
    }


   
}
