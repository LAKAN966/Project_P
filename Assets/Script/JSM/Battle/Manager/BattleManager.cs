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
        Debug.Log(normalDeck.Count);
        WaveManager.Instance.stageID = selectedStageID;
        UnitSpawner.Instance.Init(normalDeck, leaderDeck);
    }
}

