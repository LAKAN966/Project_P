using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Button stageBtn;
    [SerializeField] private Button battleBtn;

    public int stageID;

    public void Init(StageData data)
    {
        stageID = data.ID;
        
        stageBtn.onClick.RemoveAllListeners();
        stageBtn.onClick.AddListener(OnClickNode);
        battleBtn.onClick.RemoveAllListeners();
        battleBtn.onClick.AddListener(OnClickEnterBattle);

    }

    public void OnClickNode()
    {
        var data = StageDataManager.Instance.GetStageData(stageID);
    }

    public void OnClickEnterBattle() // 배틀에 들어갈대 플레이어의 노말, 유니크 덱, 스테이지 아이디 전달
    {
        var normal = DeckManager.Instance.GetAllDataInDeck();
        var leader = DeckManager.Instance.GetLeaderDataInDeck();

    }
}
