using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private const int maxEntryCounts = 3;
    private PlayerTowerData data => PlayerDataManager.Instance.player.towerData;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Init()
    {

    }
}
