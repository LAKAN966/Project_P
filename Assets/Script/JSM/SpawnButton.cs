using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnButton : MonoBehaviour
{
    [HideInInspector] public int unitID;
    [HideInInspector] public bool isEnemy;
    [HideInInspector] public bool isHero;

    public Image cooldownOverlay;
    public Button button;
    public TextMeshProUGUI costText;   // 인스펙터 연결
    public GameObject iconParent;      // 인스펙터 연결

    private bool initialized = false;

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        if (!initialized) return;

        float remaining = CoolTimeManager.Instance.GetRemainingCooldown(unitID);
        float total = GetCooldown();

        cooldownOverlay.fillAmount = total > 0 ? remaining / total : 0f;
        button.interactable = remaining <= 0f;
    }

    private void InitializeUI()
    {
        var stats = UnitDataManager.Instance.GetStats(unitID);
        if (stats == null)
        {
            Debug.LogWarning("Stats not found for unitID: " + unitID);
            return;
        }

        if (costText != null)
            costText.text = stats.Cost.ToString();

        if (iconParent != null)
        {
            foreach (Transform child in iconParent.transform)
                Destroy(child.gameObject);

            string path = $"SPUM/{stats.ModelName}";
            var modelPrefab = Resources.Load<GameObject>(path);
            if (modelPrefab != null)
            {
                var iconInstance = Instantiate(modelPrefab, iconParent.transform);
                iconInstance.transform.localPosition = new Vector3(0f, 0f, -10f);
                iconInstance.transform.localRotation = Quaternion.identity;
                iconInstance.transform.localScale = Vector3.one * 150f;

                DestroyImmediate(iconInstance.GetComponentInChildren<Animator>());
                DestroyImmediate(iconInstance.GetComponentInChildren<SPUM_Prefabs>());
            }
            else
            {
                Debug.LogWarning($"모델 프리팹을 찾을 수 없습니다: {path}");
            }
        }

        initialized = true;
    }

    private float GetCooldown()
    {
        var stats = UnitDataManager.Instance.GetStats(unitID);
        return stats != null ? stats.SpawnInterval : 0f;
    }
}
