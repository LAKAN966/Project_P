using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDeckSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public bool isLeaderSlot = false;
    public Image DeckImage;
    public UnitStats unitData;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

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
            Debug.Log("이미 덱에 있음");
            return;
        }

        DeckImage.sprite = Resources.Load<Sprite>($"SPUMImg/{unitStats.ModelName}"); //유닛 이미지 가지고 오기.
        
                        
        unitData = unitStats;
        UIUnitInfo.instance.ShowInfo(unitData);
        UIDeckBuildManager.instance.SetMyUnitIcons();
        UIDeckBuildManager.instance.SetDeckSlots();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unitData == null) return;

        //그냥 클릭은 정보 불러오기
        UIUnitInfo.instance.ShowInfo(unitData);

        

        // 더블클릭 되도록
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            DeckManager.Instance.RemoveFromDeck(unitData.ID);

            unitData = null;
            DeckImage.sprite = null;
            
            UIDeckBuildManager.instance.SetMyUnitIcons();
            UIDeckBuildManager.instance.SetDeckSlots();

        }
        lastClickTime = Time.time;

    }


}
