using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseBoxSet : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_Text ItemDescriptionText; // 아이템 설명 텍스트

    public GameObject purchaseUIBox;   // 구매UI 상자
    public GameObject DescriptionBox;  // 아이템 설명 창

    public Button PurchaseItemIcon; // 상품 아이콘
    public Button CancelButton;     // 구매 취소 버튼


    public Item _Item;
    internal CertiSlot _Info;

    private void Start()
    {
        PurchaseItemIcon.onClick.AddListener(DescriptionSet);

        CancelButton.onClick.AddListener(TabClose);
    }

    public void TabClose()
    {
        purchaseUIBox.SetActive(false);
        DescriptionBox.SetActive(false);
    }

    public void DescriptionSet()
    {
        Debug.Log("아이템 설명" + _Item.Description);
        DescriptionBox.SetActive(true);
        DescriptionBox.GetComponentInChildren<TMP_Text>().text = _Item.Description; // null일경우 넣어주면 안됨.
    }
}
