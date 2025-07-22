using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;


public class PurchaseSync : MonoBehaviour
{
    private UILoader uILoader;
    private UIManager uiManager;

    [SerializeField] public TMP_Text AtemptLeft;          // 남은 구매횟수 텍스트
    [SerializeField] public TMP_Text AtemptTotal;         // 구매 가능 횟수 텍스트

    public TMP_InputField InputAmount;   // 수량 입력칸

    public Button AddButton;        // 수량 추가 버튼
    public Button SubtractButton;   // 수량 빼기버튼

    public Button PurchaseButton;   // 아이템 구매 버튼

    private GameObject PurchaseUIBox; // 아이템 구매 

    public Item _Item;
    public ItemSlot iSlot;

    public TMP_Text NotEnoughBoxText;  // 재화 부족 경고 텍스트
    public GameObject NotEnoughBox;    // 재화 부족 경고
    public PurchaseBoxSet purchaseBoxSet;


    public void Start()
    {
        InputAmount.text = "1"; // 기본 세팅.

        //버튼 누적 호출 스책 초기화
        AddButton.onClick.RemoveAllListeners();
        SubtractButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.RemoveAllListeners();

        AddButton.onClick.AddListener(() =>
        {
            int amount = int.Parse(InputAmount.text);
            Debug.Log(amount);
            if (amount >= 1)
            {
                amount += 1;
                InputAmount.text = amount.ToString();
            }
            else
            { Debug.Log("s"); }
        }
        );
        SubtractButton.onClick.AddListener(() =>
        {
            int amount = int.Parse(InputAmount.text);
            Debug.Log(amount);
            if (amount > 1)
            {
                amount -= 1;
                InputAmount.text = amount.ToString();
            }
            else { Debug.Log("s"); }

            if (amount < 0)
            {
                Debug.Log("f");
            }
        }
        );
        PurchaseButton.onClick.AddListener(PurchaseItem); // 구매
    }
    public void PurchaseItem()
    {
        if (AtemptLeft == null && AtemptTotal == null)
        {
            AtemptLeft = FindObjectOfType<ItemSlot>().NowAttempt;
            AtemptTotal = FindObjectOfType<ItemSlot>().TotalAtempt;
        }
        int Attempt = _Item.DailyBuy;
        int Attemptleft = _Item.DailyBuy - 1;


        int Cost = _Item.Cost;
        int Amount = int.Parse(InputAmount.text);
        if (PlayerDataManager.Instance.UseGold(Cost * Amount))
        {
            if (_Item.ID == 101)
            {
                Debug.Log(Attempt + "일반 모집");
                if (Attemptleft > 0)
                {
                    PlayerDataManager.Instance.AddTicket(Amount);
                    InputAmount.text = "1";
                    Debug.Log(PlayerDataManager.Instance.player.ticket);
                    AtemptLeft.text = (Attempt - 1).ToString();
                }
                else
                {
                    AtemptNotEnoungh();
                }
            }
            if (_Item.ID == 102)
            {
                Debug.Log(Attempt + "건설도구");
                if (Attemptleft > 0)
                {
                    PlayerDataManager.Instance.AddTribute(Amount);
                    InputAmount.text = "1";
                    AtemptLeft.text = (Attempt - 1).ToString();
                }
                else
                {
                    AtemptNotEnoungh();
                }
            }
            if (_Item.ID == 103)
            {
                Debug.Log(Attempt + "설계도");
                if (Attemptleft > 0)
                {
                    PlayerDataManager.Instance.AddBluePrint(Amount);
                    InputAmount.text = "1";
                    AtemptLeft.text = (Attempt - 1).ToString();
                }
                else
                {
                    AtemptNotEnoungh();
                }
            }
            ShoppingManager.Instance.ShowNowGold();
            purchaseBoxSet.TabClose();
        }
        else
        {
            NotEnough();
        }
    }
    public void NotEnough()
    {
        UIController.Instance.NotEnoughBox.SetActive(true);
        NotEnoughBoxText.text = "골드가 부족합니다.";
        StartCoroutine(HideNotEnoughBox());

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(1f); // 3초 대기
            UIController.Instance.NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }
    public void AtemptNotEnoungh()
    {
        UIController.Instance.NotEnoughBox.SetActive(true);
        NotEnoughBoxText.text = "모든 구매 횟수를 모두 사용하셨습니다.";
        StartCoroutine(HideNotEnoughBox());

        IEnumerator HideNotEnoughBox()
        {
            yield return new WaitForSeconds(1f); // 3초 대기
            UIController.Instance.NotEnoughBox.SetActive(false);       // 경고창 비활성화
        }
    }

    public void Init(Item item, ItemSlot slot)
    {
        Debug.Log("c");
        _Item = item;
        iSlot = slot;
    }
}