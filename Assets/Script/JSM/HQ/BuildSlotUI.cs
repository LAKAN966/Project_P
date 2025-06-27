using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSlotUI : MonoBehaviour
{
    public int buildingID;
    public Image slotImage;
    public TMP_Text nameText;
    public TMP_Text levelText;
    public GameObject plusText;
    public GameObject buildListUI;
    public GameObject buildGospelUI;

    public float maxHeight = 300f; // ✅ 최대 높이 제한

    public int Level { get; private set; } = 0;

    public void Select()
    {
        BuildManager.Instance.SelectSlot(this);
        if (Level > 0)
        {
            buildGospelUI.GetComponentInChildren<GospelSpawner>().buildID = buildingID;
            buildGospelUI.SetActive(true);
        }
        else
        {
            buildListUI.SetActive(true);
        }
    }

    public void Build(BuildingData building)
    {
        if (Level < 0)
        {
            Debug.Log("최대 레벨");
            return;
        }

        buildingID = building.id;
        Level++;
        if (plusText != null) Destroy(plusText);

        // Building Image Changed
        if (slotImage != null)
        {
            Sprite newSprite = BuildManager.Instance.GetBuildingSprite(building.imageName);
            slotImage.sprite = newSprite;

            // ✅ 비율 유지 + 최대 높이 제한
            AspectRatioFitter fitter = slotImage.GetComponent<AspectRatioFitter>();
            RectTransform rt = slotImage.GetComponent<RectTransform>();

            if (newSprite != null && rt != null)
            {
                float w = newSprite.rect.width;
                float h = newSprite.rect.height;
                float aspect = w / h;

                float targetWidth = rt.rect.width;
                float targetHeight = targetWidth / aspect;

                if (targetHeight > maxHeight)
                {
                    targetHeight = maxHeight;
                    targetWidth = targetHeight * aspect;
                }

                if (fitter != null) fitter.aspectMode = AspectRatioFitter.AspectMode.None;

                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
            }
        }

        if (nameText != null)
        {
            if (!nameText.gameObject.activeSelf)
                nameText.gameObject.SetActive(true);

            nameText.text = BuildManager.Instance.buildings[buildingID - 1].displayName;
        }

        if (levelText != null)
        {
            if (!levelText.gameObject.activeSelf)
                levelText.gameObject.SetActive(true);

            levelText.text = $"Lv.{Level}";
        }
    }
}
