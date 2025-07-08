using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Button stageBtn;
    [SerializeField] private TextMeshProUGUI stageNameTxt;

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

}
