using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CertiPurchaseBoxSet : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_Text UnitDescriptionText; // 아이템 설명 텍스트

    public GameObject PurchaseCertiUnitBox;   // 구매UI 상자
    public GameObject CertiDescriptionBox;  // 아이템 설명 창

    public Button PurchaseItemIcon; // 상품 아이콘
    public Button CancelButton;     // 구매 취소 버튼


    public PickInfo _PickInfo;
    void Start()
    {
        PurchaseItemIcon.onClick.AddListener(DescriptionSet);

        CancelButton.onClick.AddListener(TabClose);
    }

    // Update is called once per frame
    public void TabClose()
    {
        PurchaseCertiUnitBox.SetActive(false);
        CertiDescriptionBox.SetActive(false);
    }

    public void DescriptionSet()
    {
        Debug.Log("아이템 설명" + _PickInfo.Description);
        CertiDescriptionBox.SetActive(true);
        CertiDescriptionBox.GetComponentInChildren<TMP_Text>().text = _PickInfo.Description; // null일경우 넣어주면 안됨.
    }
}
