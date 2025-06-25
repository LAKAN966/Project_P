using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDeckSlot : MonoBehaviour, IDropHandler
{
    public bool isLeaderSlot = false;
    public Image iconImage;
    public UnitStats unitData;

    public void OnDrop(PointerEventData eventData)
    {
        var draggedIcon = eventData.pointerDrag.GetComponentInChildren<UIUnitIcon>();
        if (draggedIcon == null) return;

        var unitStats = draggedIcon.GetStats();
        if (unitStats == null) return;

        if (isLeaderSlot && !unitStats.IsHero)
        {
            Debug.LogWarning("일반 유닛은 리더 슬롯에 들어갈 수 없습니다.");
            return;
        }

        if (!isLeaderSlot && unitStats.IsHero)
        {
            Debug.LogWarning("리더 유닛은 일반 슬롯에 들어갈 수 없습니다.");
            return;
        }


        //덱 매니저에 등록 시도
        bool success = DeckManager.Instance.TryAddUnitToDeck(unitStats.ID);
        if (!success)
        {
            Debug.Log("이미 덱에 있음 또는 실패");
            return;
        }

        //iconImage.sprite = unitStats. //유닛 이미지 가지고 오기.
        unitData = unitStats;

        // 드래그 원본 비활성화 (중복 방지)
        //draggedIcon.SetDisabled();
        Destroy(draggedIcon);
        UIDeckBuildManager.instance.SetMyUnitIcons();
        UIDeckBuildManager.instance.SetDeckSlots();


    }
}
