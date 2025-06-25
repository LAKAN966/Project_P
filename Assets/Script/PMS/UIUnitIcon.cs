using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUnitIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image iconImage; //유닛 이미지 표시
    public TextMeshProUGUI costText; //코스트 소모량 표시
    public CanvasGroup canvasGroup; //캔버스 그룹, 투명도 위함
    public Transform originalParent; //시작 위치
    public UnitStats stats;

    private UnitStats myStats;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(UnitStats stats)
    {
        myStats = stats;
        costText.text = stats.Cost.ToString();
        //iconImage.sprite = // 모델명에 따라 스프라이트 로딩
    }

    public UnitStats GetStats()
    {
        return myStats;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // 캔버스 루트로 옮겨서 가장 위에 보이게
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.3f; // 드래그 중 반투명
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = Vector3.zero; // 드롭 안된 경우 원래 자리로 돌아감
        canvasGroup.alpha = 1f; // 드롭후 다시 원래대로
    }

    public void SetDisabled()
    {
        canvasGroup.alpha = 0.5f;
        this.enabled = false;
    }
}
