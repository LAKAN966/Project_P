using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GospelSpawner : MonoBehaviour
{
    public GameObject GospelContainerPrefab;
    public GameObject GospelSlotPrefab;
    public GameObject LackLevelPanel;
    public Transform parent;
    public int buildID;
    public GameObject gospelConfirmUI;
    public int level;

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

        for (int order = layeredGospels.Count - 1; order >= 0; order--)
        {
            var layerData = layeredGospels[order];

            GameObject container = Instantiate(GospelContainerPrefab, parent);
            container.name = $"GospelContainer_Order{order}";
            container.layer = order;
            spawnedContainers.Add(container);

            var containerUI = container.GetComponent<GospelContainerUI>();
            if (containerUI != null)
                containerUI.SetInteractable(order == currentSelectableOrder);

            foreach (var gospel in layerData)
            {
                GameObject slot = Instantiate(GospelSlotPrefab, container.transform);
                slot.name = $"GospelSlot_{gospel.id}";
                slot.GetComponent<GospelSlotUI>().gospelSpawner = this;
                slot.GetComponent<GospelSlotUI>().containerUI = container.GetComponent<GospelContainerUI>();
                container.GetComponent<GospelContainerUI>().AddSlot(slot.GetComponent<GospelSlotUI>());

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
            if(BuildManager.Instance.GetOrderLevel(buildID, level)<=order)
            {
                GameObject lack = Instantiate(LackLevelPanel, container.transform);
                lack.GetComponentInChildren<TextMeshProUGUI>().text = BuildManager.Instance.GetNextLevel(buildID, order).ToString()+"레벨에 해금됩니다.";
            }
        }
    }
    public void OnSlotSelected(GospelSlotUI selectedSlot)
    {
        ClearGospels();
        SpawnGospels();
    }
}
