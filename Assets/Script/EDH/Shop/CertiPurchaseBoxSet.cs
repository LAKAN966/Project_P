using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CertiPurchaseBoxSet : MonoBehaviour
{
    public TMP_Text UnitDescriptionText; // 아이템 설명 텍스트

    public GameObject PurchaseCertiUnitBox;   // 구매UI 상자
    public GameObject CertiDescriptionBox;  // 아이템 설명 창

    public Image UnitIcon;

    public Button PurchaseItemIcon; // 상품 아이콘
    public Button CancelButton;     // 구매 취소 버튼

    public PickInfo _PickInfo;
    void Start()
    {
        PurchaseItemIcon.onClick.AddListener(DescriptionSet);
        SFXManager.Instance.PlaySFX(0);
        CancelButton.onClick.AddListener(TabClose);
    }

    // Update is called once per frame
    public void TabClose()
    {
        PurchaseCertiUnitBox.SetActive(false);
        CertiDescriptionBox.SetActive(false);
        SFXManager.Instance.PlaySFX(0);
    }

    public void DescriptionSet()
    {
        Debug.Log("유닛 설명" + _PickInfo.Description);
        CertiDescriptionBox.SetActive(true);
        CertiDescriptionBox.GetComponentInChildren<TMP_Text>().text = _PickInfo.Description; // null일경우 넣어주면 안됨.
    }

    public void SetUnitIcon(Sprite sprite)
    {
        UnitIcon.sprite = sprite;
    }
}
