using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GospelState
{
    Available,   // 선택 가능
    Locked,      // 선택 불가
    Selected     // 이미 선택됨
}
public class GospelSlotUI : MonoBehaviour
{
    public GameObject SelectedImg;
    public CanvasGroup canvasGroup;

    public TMP_Text nameText;
    public TMP_Text costText;

    private GospelData gospelData;
    private GospelState state;

    public GameObject gospelConfirmUI;
    public GospelSpawner gospelSpawner;
    public GospelContainerUI containerUI;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void SetData(GospelData data, GospelState newState)
    {
        gospelData = data;
        nameText.text = data.name;
        costText.text = data.cost.ToString();
        state = newState;
        UpdateUIByState();
    }
    private void OnClick()
    {
        if (gospelConfirmUI != null && gospelData != null)
        {
            gospelConfirmUI.SetActive(true);
            gospelConfirmUI.GetComponent<GospelConfirmUI>().Show(gospelData);
            gospelConfirmUI.GetComponent<GospelConfirmUI>().confirmBtn.onClick.AddListener(ConfirmBuild);
            gospelConfirmUI.GetComponent<GospelConfirmUI>().cancleBtn.onClick.AddListener(CancleBuild);
        }
    }
    private void UpdateUIByState()
    {
        switch (state)
        {
            case GospelState.Locked:
                SelectedImg.SetActive(false);
                canvasGroup.alpha = 0.3f;
                break;
            case GospelState.Available:
                SelectedImg.SetActive(false);
                canvasGroup.alpha = 1f;
                break;
            case GospelState.Selected:
                SelectedImg.SetActive(true);
                canvasGroup.alpha = 1f;
                break;
        }
    }
    public void ConfirmBuild()
    {
        if (state == GospelState.Locked) return;

        //if (PlayerDataManager.Instance.player.tribute < gospelData.cost)
        //{
        //    HQResourceUI.Instance.ShowLackPanel();
        //    CancleBuild();
        //    return;
        //}
        //자원 부족시 보여주는 판넬, 추후 주석 삭제 필요
        //플레이어자원 제거하는 코드 필요

        GospelManager.Instance.SelectGospel(gospelData.buildID, gospelData.id);
        Debug.Log("?");

        gospelSpawner.OnSlotSelected(this);

        if (gospelConfirmUI != null && gospelData != null)
        {
            gospelConfirmUI.SetActive(true);
            gospelConfirmUI.GetComponent<GospelConfirmUI>().Show(gospelData);
        }
        CancleBuild();
    }
    public void CancleBuild()
    {
        gospelConfirmUI.GetComponent<GospelConfirmUI>().confirmBtn.onClick.RemoveListener(ConfirmBuild);
        gospelConfirmUI.GetComponent<GospelConfirmUI>().cancleBtn.onClick.RemoveListener(CancleBuild);
        gospelConfirmUI.SetActive(false);
    }
}
