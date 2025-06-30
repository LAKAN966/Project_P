using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Button stageBtn;

    public int stageID;

    public void Init(StageData data)
    {
        stageID = data.ID;
        
        stageBtn.onClick.RemoveAllListeners();
        stageBtn.onClick.AddListener(OnClickNode);
    }

    public void OnClickNode()
    {
        StageManager.instance.SelectStage(stageID);
    }

}
