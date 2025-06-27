using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIDeckBuildManager : MonoBehaviour
{
    [Header ("보유 유닛 슬롯")]
    [SerializeField] private GameObject unitIconPrefab;      // 유닛 아이콘 프리팹
    [SerializeField] private Transform myUnitContentParent;  // ScrollView의 Content

    [Header("일반 유닛 덱 슬롯")]
    [SerializeField] private GameObject deckSlotPrefab; // 덱 슬롯 프리팹
    [SerializeField] private Transform deckSlotParent; // 덱 슬롯 프리팹의 부모 (생성위치)

    [Header("리더 덱 슬롯")]
    [SerializeField] private UIDeckSlot leaderSlot; // 리더 슬롯

    public List<UIDeckSlot> deckSlotList = new();
    private List<int> cachedUnitOrder = new();

    public static UIDeckBuildManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Init() //테스트용 이닛
    {
        InitDeckSlots();
        CacheUnitListOrder();
        SetDeckSlots();
        SetMyUnitIcons();
    }

    public void InitDeckSlots() // 시작할 때 덱 슬롯 만들기. 처음에만 쓰면됨.
    {
        foreach (Transform child in deckSlotParent)
        {
            Destroy(child.gameObject);
        }

        deckSlotList.Clear();

        for(int i = 0; i<6; i++)
        {
            GameObject slot = Instantiate(deckSlotPrefab, deckSlotParent);
            UIDeckSlot uiSlot = slot.GetComponent<UIDeckSlot>();
            deckSlotList.Add(uiSlot);
        }
    }

    public void SetDeckSlots() // 덱 슬롯 데이터 리셋용
    {
        var normalUnitIDs = DeckManager.Instance.GetAllNormalUnit();
        var leaderUnitID = DeckManager.Instance.GetLeaderUnit();

        for (int i = 0; i < deckSlotList.Count; i++)
        {
            UIDeckSlot slot = deckSlotList[i];

            if (i < normalUnitIDs.Count)
            {
                UnitStats stats = UnitDataManager.Instance.GetStats(normalUnitIDs[i]);
                //slot.iconImage.sprite = stats.; //이미지 아직 없음
                slot.unitData = stats;

            }
            else
            {
                //slot.iconImage.sprite = null;
                slot.unitData = null;
            }
        }
        if (leaderSlot != null && leaderUnitID.HasValue)
        {
            var leaderStats = UnitDataManager.Instance.GetStats(leaderUnitID.Value);
            //leaderSlot.iconImage.sprite = leaderStats.; //이미지 아직 없음
            leaderSlot.unitData = leaderStats;
        }
    }

    public void SetMyUnitIcons()
    {
        // 기존 아이콘 정리
        foreach (Transform child in myUnitContentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (int unitID in cachedUnitOrder)
        {
            var stats = UnitDataManager.Instance.GetStats(unitID);
            if (stats == null) continue;

            GameObject iconGO = Instantiate(unitIconPrefab, myUnitContentParent);
            var unitIcon = iconGO.GetComponent<UIUnitIcon>();
            unitIcon.Setup(stats);

            if (DeckManager.Instance.CheckInDeck(stats.ID))
            {
                unitIcon.SetDisabled();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CacheUnitListOrder()
    {
        var allUnits = MyUnitList.Instance.GetAllUnit();
        cachedUnitOrder = allUnits
            .OrderByDescending(unit => unit.IsHero)
            .ThenBy(unit => unit.ID)
            .Select(u => u.ID)
            .ToList();
    }

    
}
