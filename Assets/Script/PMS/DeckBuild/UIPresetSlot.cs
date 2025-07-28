using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPresetSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image unitImage;

    private UnitStats currentStats;
    private UIPresetDeck parentDeck;
    private bool isLeaderSlot;

    public void Setup(UnitStats stats, UIPresetDeck deck, bool isLeader)
    {
        currentStats = stats;
        parentDeck = deck;
        isLeaderSlot = isLeader;

        unitImage.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
        unitImage.color = Color.white;
    }

    public void SetParentDeck(UIPresetDeck deck, bool isLeader)
    {
        parentDeck = deck;
        isLeaderSlot = isLeader;
    }

    public void Clear()
    {
        currentStats = null;
        unitImage.sprite = null;
        unitImage.color = new Color(1, 1, 1, 0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (parentDeck == null)
        {
            Debug.LogError($"OnDrop 실패: parentDeck이 null입니다. slot={gameObject.name}");
            return;
        }

        var draggedIcon = eventData.pointerDrag?.GetComponent<UIUnitIcon>();
        if (draggedIcon != null)
        {
            draggedIcon.SetDropped();
            parentDeck.TryAddUnit(draggedIcon.GetStats(), isLeaderSlot);
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentStats != null && eventData.clickCount == 2)
        {
            parentDeck.RemoveUnit(currentStats);
        }
    }
}