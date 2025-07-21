using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnButton : MonoBehaviour
{
     public int unitID;
    [HideInInspector] public bool isEnemy;
    [HideInInspector] public bool isHero;

    public Image cooldownOverlay;
    public Button button;
    public TextMeshProUGUI costText;
    public GameObject iconParent;

    private bool initialized = false;
    public UnitStats stats;


    public void Update()
    {
        if (!initialized) return;
        if (unitID == 0) return;

        float remaining = CoolTimeManager.Instance.GetRemainingCooldown(unitID);
        float total = GetCooldown();
        if(unitID == 0)
        {
            cooldownOverlay.fillAmount = 1;
        }
        cooldownOverlay.fillAmount = total > 0 ? remaining / total : 0f;
        button.interactable = remaining <= 0f;
    }

    public void InitializeUI()
    {
        if (unitID == 0)
        {
            return;
        }
        stats = BuffManager.ApplyBuff(UnitDataManager.Instance.GetStats(unitID));
        stats = UnitSpawner.Instance.SetGimmick(stats);
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

            string path = $"Units/{stats.ModelName}";
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

        Debug.Log($"{unitID} 세팅!");

        initialized = true;
    }

    private float GetCooldown()
    {
        return stats != null ? stats.SpawnInterval : 0f;
    }

}
