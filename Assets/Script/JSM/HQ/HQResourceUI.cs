using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HQResourceUI : MonoBehaviour
{
    public static HQResourceUI Instance;
    public TextMeshProUGUI GoldTxt;
    public TextMeshProUGUI SkullTxt;
    public GameObject targetPanel;
    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        GoldTxt.text = PlayerDataManager.Instance.player.gold.ToString();
        SkullTxt.text = PlayerDataManager.Instance.player.tribute.ToString();
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
