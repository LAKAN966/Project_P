using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInfo : MonoBehaviour
{
    [Header("이미지/정보")]
    [SerializeField] private Image infoImage;
    [SerializeField] private GameObject infoPannel;
    [SerializeField] private TextMeshProUGUI nameValueText;
    [SerializeField] private TextMeshProUGUI hpValueText;
    [SerializeField] private TextMeshProUGUI damageValueText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private Image hpIcon;
    [SerializeField] private Image damageIcon;
    [SerializeField] private Image typeIcon;
    [SerializeField] private Sprite hpSprite;
    [SerializeField] private Sprite damageSprite;
    [SerializeField] private Sprite typeSprite;

    [Header("리더 유닛 이미지/정보")]
    [SerializeField] private Image leaderIMG;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image costIcon;

    [SerializeField] private GameObject skillBox;

    public static UIUnitInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowInfo(UnitStats stats)
    {
        if (stats != null)
        {
            infoImage.gameObject.SetActive(true);
            infoPannel.gameObject.SetActive(true);

            infoImage.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
            if (stats == null)
            {
                nameValueText.text = "";
                hpValueText.text = "";
                damageValueText.text = "";
                return;
            }

            nameValueText.text = $"{stats.Name}";
            hpValueText.text = $"{stats.MaxHP.ToString()}";
            damageValueText.text = $"{stats.Damage.ToString()}";
            typeText.text = $"{TagManager.GetNameByID(stats.RaceID)}";
            hpIcon.sprite = hpSprite;
            damageIcon.sprite = damageSprite;
            typeIcon.sprite = typeSprite;

            skillBox.SetActive(stats.IsHero);
        }

        else
        {
            infoImage.gameObject.SetActive(false);
            infoPannel.gameObject.SetActive(false);
        }
    }

    public void ShowleaderInfo(UnitStats stats)
    {
        if (stats != null)
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
        infoPannel.gameObject.SetActive(false);

        infoImage.gameObject.SetActive(false);
    }



}
