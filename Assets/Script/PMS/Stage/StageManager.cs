using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour 
{
    [SerializeField] private Transform nodeParent;

    private StageNode[] nodes;

    public static StageManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        nodes = nodeParent.GetComponentsInChildren<StageNode>();
        SetStageIDs();
        SetupNodes();
    }

    private void SetStageIDs()
    {
        var stageDatas = StageDataManager.Instance.GetAllStageData(); // 딕셔너리로 만들어진 모든 스테이지 데이터. 딕셔너리 키 값은 스테이지 아이디

        var sortedStages = new List<StageData>(stageDatas.Values);
        sortedStages.Sort((a, b) => a.ID.CompareTo(b.ID)); // 스테이지 리스트 정렬 (a와 b 비교해서 오름차순으로)

        for (int i = 0; i < nodes.Length && i < sortedStages.Count; i++)
        {
            nodes[i].stageID = sortedStages[i].ID; // 노드에 스테이지 매칭
        }
    }

    private void SetupNodes()
    {
        foreach (var node in nodes)
        {
            var data = StageDataManager.Instance.GetStageData(node.stageID);
            if (data == null) continue;

            node.Init(data);
        }
    }

}
