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

    [Header("리더 유닛 이미지/정보")]
    [SerializeField] private Image leaderIMG;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image costIcon;
    

    public static UIUnitInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowInfo(UnitStats stats)
    {
        if(stats != null)
        {
            infoImage.gameObject.SetActive(true);
            infoText.gameObject.SetActive(true);
            
            infoImage.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
            infoText.text = ($"{stats.Name}\n{stats.MaxHP}\n{stats.Damage}"); // 어떤 정보가 표시 되는지 아직 다 안넣었음. 확인 후 수정.
        }

        else
        {
            infoImage.gameObject.SetActive(false);
            infoText.gameObject.SetActive(false);
        }
    }

    public void ShowleaderInfo(UnitStats stats)
    {
        if(stats != null)
        {
            leaderIMG.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);
            costIcon.gameObject.SetActive(true);

            leaderIMG.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
            costText.text = stats.Cost.ToString();
        }

        else
        {
            leaderIMG.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            costIcon.gameObject.SetActive(false);
        }
    }

    public void ClearInfo()
    {
        infoImage.sprite = null;
        infoText.text = "";
    }


   
}
