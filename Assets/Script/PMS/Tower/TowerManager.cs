using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    private const int maxEntryCounts = 3;

    private void Awake()
    {
        if (instance == null) instance = this;
        
        else Destroy(gameObject);
    }

    public void Init()
    {

    }
}
