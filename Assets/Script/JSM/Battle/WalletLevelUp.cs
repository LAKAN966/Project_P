using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletLevelUp : MonoBehaviour
{
    public Button levelUpButton;
    public TMP_Text levelText;
    public TMP_Text costText;

    void Start()
    {
        levelUpButton.onClick.AddListener(TryLevelUp);
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void TryLevelUp()
    {
        if (BattleResourceManager.Instance.CanLevelUp())
        {
            BattleResourceManager.Instance.LevelUp();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        var mgr = BattleResourceManager.Instance;

        levelText.text = $"Lv.{mgr.walletLevel}";
        costText.text = mgr.CanLevelUp() ? $"{mgr.GetLevelUpCost()}" : "MAX";
        levelUpButton.interactable = mgr.CanLevelUp() && mgr.currentResource >= mgr.GetLevelUpCost();
    }
}
