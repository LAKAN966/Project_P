using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSlotUI : MonoBehaviour
{
    public Image slotImage;
    public TMP_Text nameText;
    public TMP_Text levelText;
    public List<Sprite> upgSprites;

    public int Level { get; private set; } = 0;

    public void Select()
    {
        BuildManager.Instance.SelectSlot(this);
    }

    public void Build()
    {
        if(Level < 0 || Level >= upgSprites.Count)
        {
            Debug.Log("최대 레벨");
            return;
        }

        Level++;

        // Building Image Changed
        if(slotImage != null && Level - 1 < upgSprites.Count)
            slotImage.sprite = upgSprites[Level - 1];

        // Level Text Changed
        if (levelText != null)
        {
            if (!levelText.gameObject.activeSelf)
                levelText.gameObject.SetActive(true);

            levelText.text = $"Lv.{Level}";
        }
    }

    public void SetBuilding(BuildingData data)
    {
        upgSprites = data.upgradeSprites;
        Level = 0;

        if (slotImage != null && upgSprites.Count > 0)
            slotImage.sprite = upgSprites[0];

        if (nameText != null)
        {
            nameText.text = data.name;
        }

        if (levelText != null)
        {
            levelText.gameObject.SetActive(false);
            levelText.text = "";
        }

    }
}
