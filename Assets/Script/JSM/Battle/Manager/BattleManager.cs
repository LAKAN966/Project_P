using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public void StartBattle(int selectedStageID, List<UnitStats> normalDeck, UnitStats leaderDeck)
    {
        WaveManager.Instance.stageID = selectedStageID;
    }
}
