using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class BattleResourceManager : MonoBehaviour
{
    public static BattleResourceManager Instance { get; private set; }

    public TMP_Text moneyText;

    public float currentResource = 0f;   // float로 변경
    public int maxResource = 100;
    public int resourcePerSecond = 5;

    public int walletLevel = 1;
    public int maxWalletLevel = 8;
    public int levelUpBaseCost = 50;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        // 실시간 자원 증가
        currentResource += resourcePerSecond * Time.deltaTime;
        currentResource = Mathf.Min(currentResource, maxResource);

        // 실시간 UI 갱신
        if (moneyText != null)
        {
            moneyText.text = Mathf.FloorToInt(currentResource).ToString()+ " / " + maxResource.ToString();
        }
    }

    public void Add(int amount)
    {
        currentResource = Mathf.Min(currentResource + amount, maxResource);
    }

    public bool Spend(int amount)
    {
        if (currentResource < amount) return false;
        currentResource -= amount;
        return true;
    }

    public int GetLevelUpCost()
    {
        return levelUpBaseCost * walletLevel;
    }

    public bool CanLevelUp() => walletLevel < maxWalletLevel;

    public void LevelUp()
    {
        if (!CanLevelUp()) return;

        int cost = GetLevelUpCost();
        if (!Spend(cost)) return;

        walletLevel++;
        maxResource += 50;
        resourcePerSecond += 2;
    }
}
