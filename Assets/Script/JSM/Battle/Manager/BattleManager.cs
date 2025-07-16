using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    private static BattleManager instance;
    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BattleManager();
            }
            return instance;
        }
    }

    private BattleManager() { }

    public void StartBattle(int selectedStageID, List<UnitStats> normalDeck, UnitStats leaderDeck)
    {
        Debug.Log(selectedStageID + "스타트 배틀");
        WaveManager.Instance.stageID = selectedStageID;
        UnitSpawner.Instance.Init(normalDeck, leaderDeck);
        UnitSpawner.Instance.SetButton();
    }
}

