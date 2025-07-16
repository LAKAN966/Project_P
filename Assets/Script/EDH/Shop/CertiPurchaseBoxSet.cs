using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CertiPurchaseBoxSet : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    public TMP_Text ItemDescriptionText; // 아이템 설명 텍스트

    public GameObject purchaseUIBox;   // 구매UI 상자
    public GameObject DescriptionBox;  // 아이템 설명 창

    public Button PurchaseItemIcon; // 상품 아이콘
    public Button CancelButton;     // 구매 취소 버튼


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
