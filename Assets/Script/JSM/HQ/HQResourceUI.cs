using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HQResourceUI : MonoBehaviour
{
    public static HQResourceUI Instance;
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI skullTxt;
    public TextMeshProUGUI bluePrintTxt;
    public GameObject targetPanel;
    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
    private void OnEnable()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        goldTxt.text = NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.gold);
        skullTxt.text = NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.tribute);
        bluePrintTxt.text = NumberFormatter.FormatNumber(PlayerDataManager.Instance.player.bluePrint);
    }
    public void ShowLackPanel(float duration = 3f)
    {
        if (targetPanel == null)
        {
            Debug.LogWarning("패널이 지정되지 않았습니다.");
            return;
        }
        StartCoroutine(ShowAndHide(duration));
    }

    private IEnumerator ShowAndHide(float delay)
    {
        targetPanel.SetActive(true);
        yield return new WaitForSeconds(delay);
        targetPanel.SetActive(false);
    }
}
