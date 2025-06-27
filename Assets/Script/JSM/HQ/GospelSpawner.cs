using UnityEngine;
using System.Collections.Generic;

public class GospelSpawner : MonoBehaviour
{
    public GameObject GospelContainerPrefab;
    public GameObject GospelSlotPrefab;
    public Transform parent;
    public int buildID;
    public GameObject gospelConfirmUI;

    private readonly List<GameObject> spawnedContainers = new();

    private void OnEnable()
    {
        SpawnGospels();
    }

    private void OnDisable()
    {
        ClearGospels();
    }

    private void ClearGospels()
    {
        foreach (var go in spawnedContainers)
        {
            if (go != null)
                Destroy(go);
        }
        spawnedContainers.Clear();
    }

    private void SpawnGospels()
    {
        if (parent == null) parent = this.transform;

        var layeredGospels = GospelManager.Instance.GetGospelsByBuildID(buildID);
        int currentSelectableOrder = GospelManager.Instance.GetCurrentSelectableOrder(buildID);

        for (int order = layeredGospels.Count - 1; order >= 0; order--) // 역순
        {
            var layerData = layeredGospels[order];

            GameObject container = Instantiate(GospelContainerPrefab, parent);
            container.name = $"GospelContainer_Order{order + 1}";
            spawnedContainers.Add(container);

            // 🔸 컨테이너 상태 설정 (어둡게 처리)
            var containerUI = container.GetComponent<GospelContainerUI>();
            if (containerUI != null)
                containerUI.SetInteractable(order == currentSelectableOrder);

            foreach (var gospel in layerData)
            {
                GameObject slot = Instantiate(GospelSlotPrefab, container.transform);
                slot.name = $"GospelSlot_{gospel.id}";

                var slotUI = slot.GetComponent<GospelSlotUI>();
                if (slotUI != null)
                {
                    slotUI.gospelConfirmUI = gospelConfirmUI;

                    GospelState state;
                    if (GospelManager.Instance.IsSelected(buildID, gospel.id))
                        state = GospelState.Selected;
                    else if (order < currentSelectableOrder)
                        state = GospelState.Locked;
                    else
                        state = GospelState.Available;

                    slotUI.SetData(gospel, state);
                }
            }
        }
    }
}
