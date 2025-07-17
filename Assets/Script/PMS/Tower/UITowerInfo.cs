using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITowerInfo : MonoBehaviour
{
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject rewardParent;
    [SerializeField] private GameObject rewardPrefab;
    
    private List<int> firstRewardIDs;
    private List<int> firstRewardAmounts;

    public static UITowerInfo instance;

    public void Awake()
    {
        instance = this;
    }

    public void SetTowerInfo(int stageID)
    {
        var stage = StageDataManager.Instance.GetStageData(stageID);

        foreach (Transform child in enemyParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in rewardParent.transform)
        {
            Destroy(child.gameObject);
        }

        List<int> enemyIDs = new List<int>();
        if (stage.EnemyUnit1 != 0) enemyIDs.Add(stage.EnemyUnit1);
        if (stage.EnemyUnit2 != 0) enemyIDs.Add(stage.EnemyUnit2);
        if (stage.EnemyUnit3 != 0) enemyIDs.Add(stage.EnemyUnit3);

        foreach (int enemyID in enemyIDs)
        {
            var enemyStats = UnitDataManager.Instance.GetStats(enemyID);
            if (enemyStats == null) continue;

            GameObject enemySlot = Instantiate(enemyPrefab, enemyParent.transform);
            Image image = enemySlot.GetComponentInChildren<Image>();
            if (image != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"SPUMImg/{enemyStats.ModelName}");
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning($"적 스프라이트 없음: SPUMImg/{enemyStats.ModelName}");
                }
            }
        }

        firstRewardIDs = stage.firstRewardItemIDs;
        firstRewardAmounts = stage.firstRewardAmounts;

        firstRewardIDs = stage.firstRewardItemIDs;
        firstRewardAmounts = stage.firstRewardAmounts;

        for (int i = 0; i < firstRewardIDs.Count; i++)
        {
            CreateRewardSlot(firstRewardIDs[i], firstRewardAmounts[i], rewardParent.transform, rewardPrefab);
        }
    }
    private void CreateRewardSlot(int itemID, int amount, Transform parent, GameObject slotPrefab)
    {
        GameObject slot = Instantiate(slotPrefab, parent);
        Sprite icon = Resources.Load<Sprite>($"Rewards/{itemID}");
        if (icon == null)
        {
            Debug.LogWarning($"스프라이트를 찾을 수 없습니다: Rewards/{itemID}");
            return;
        }

        Image iconImage = slot.GetComponentInChildren<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        }

        TextMeshProUGUI[] texts = slot.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var t in texts)
        {
            if (t.name == "Amount")
            {
                t.text = amount.ToString();
                break;
            }
        }
    }
}
