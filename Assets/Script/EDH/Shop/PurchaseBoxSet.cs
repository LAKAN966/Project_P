using TMPro;
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

    private void Start()
    {
        ItemDescriptionText.text = _Item.Description.ToString(); // 아이템 설명 동기화.
        PurchaseItemIcon.onClick.AddListener(() =>DescriptionBox.SetActive(true));
        CancelButton.onClick.AddListener(TabClose);
    }

    public void TabClose()
    {
        purchaseUIBox.SetActive(false);
        DescriptionBox.SetActive(false);
    }
}
