using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GospelState
{
    Locked,      // 선택 불가
    Available,   // 선택 가능
    Selected     // 이미 선택됨
}
public class GospelSlotUI : MonoBehaviour
{
    public Image SelectedImg;
    public CanvasGroup canvasGroup;

    public TMP_Text nameText;
    public TMP_Text costText;

    private GospelData gospelData;
    private GospelState state;

    public GameObject gospelConfirmUI;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void SetData(GospelData data, GospelState state)
    {
        gospelData = data;
        nameText.text = data.name;
        costText.text = data.cost.ToString();

        UpdateUIByState();
    }
    private void OnClick()
    {
        if (gospelConfirmUI != null && gospelData != null)
            gospelConfirmUI.SetActive(true);
            gospelConfirmUI.GetComponent<GospelConfirmUI>().Show(gospelData);
    }
    private void UpdateUIByState()
    {
        switch (state)
        {
            case GospelState.Locked:
                SelectedImg.enabled = false;
                canvasGroup.alpha = 0.3f;
                break;
            case GospelState.Available:
                SelectedImg.enabled = false;
                canvasGroup.alpha = 1f;
                break;
            case GospelState.Selected:
                SelectedImg.enabled = true;
                canvasGroup.alpha = 1f;
                break;
        }
    }
}
