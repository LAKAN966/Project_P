using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUnitIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image iconImage; //유닛 이미지 표시
    public TextMeshProUGUI costText; //코스트 소모량 표시
    public CanvasGroup canvasGroup; //캔버스 그룹, 투명도 위함
    public Transform originalParent; //시작 위치
    public UnitStats myStats;
    public Image slotBG;
    public GameObject leaderIcon;

    [Header ("유닛 등급 배경")]
    [SerializeField] private Sprite normalBG;
    [SerializeField] private Sprite leaderBG;

    private bool isDropped = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(UnitStats stats)
    {
        myStats = stats;
        costText.text = stats.Cost.ToString();
        iconImage.sprite = Resources.Load<Sprite>($"SPUMImg/{stats.ModelName}");
        
        slotBG.sprite = stats.IsHero ? leaderBG : normalBG;
        leaderIcon.SetActive(stats.IsHero);
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
        isDropped = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDropped)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (UIDeckBuildManager.instance != null && UIDeckBuildManager.instance.deckPanel.activeSelf)
        {
            UIDeckBuildManager.instance.SetMyUnitIcons();
            UIDeckBuildManager.instance.SetDeckSlots();
        }
        isDropped = false;
    }

    public void SetDropped()
    {
        isDropped = true;
    }


    public void SetDisabled()
    {
        canvasGroup.alpha = 0.5f;
        this.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIUnitInfo.instance.ShowInfo(myStats);
    }

    public void UpdateInteractable(bool disabled)
    {
        canvasGroup.alpha = disabled ? 0.5f : 1f;
        this.enabled = !disabled;
    }
}
