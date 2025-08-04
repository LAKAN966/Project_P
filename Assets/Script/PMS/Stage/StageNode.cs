using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Button stageBtn;
    [SerializeField] private TextMeshProUGUI stageNameTxt;
    [SerializeField] private GameObject clearText;

    public int stageID;

    public void Init(StageData data)
    {
        stageID = data.ID;
        stageNameTxt.text = data.StageName.Replace("stage","");
        
        stageBtn.onClick.RemoveAllListeners();
        stageBtn.onClick.AddListener(OnClickNode);
    }

    public void OnClickNode()
    {
        StageManager.instance.SelectStage(stageID);
    }

    public void SetClear(bool isClear)
    {
        if(clearText != null)
        {
            clearText.gameObject.SetActive(isClear);
        }
    }

    public void SetInteractable(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;

        var image = GetComponent<Image>();
        if (image != null)
        {
            image.color = interactable ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        }
    }

}
