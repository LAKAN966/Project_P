using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuildUIManager : MonoBehaviour
{
    [SerializeField] private GameObject unitIconPrefab;      // 아이콘 프리팹
    [SerializeField] private Transform myUnitContentParent;  // ScrollView의 Content

    public static DeckBuildUIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CreateMyUnitIcons()
    {
        // 이전에 있던 아이콘 정리
        foreach (Transform child in myUnitContentParent)
        {
            Destroy(child.gameObject);
        }

        // 보유 유닛 리스트 가져오기
        var myUnits = MyUnitList.Instance.GetAllUnit();

        foreach (var unit in myUnits)
        {
            GameObject iconGO = Instantiate(unitIconPrefab, myUnitContentParent);
            var unitIcon = iconGO.GetComponent<UnitIcon>();
            unitIcon.Setup(unit); // 유닛 정보 세팅
        }
    }
}
