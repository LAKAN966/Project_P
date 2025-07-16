using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerData
{
    public Dictionary<string, int> lastClearFloor = new(); // 마지막 클리어 층
    public Dictionary<string, int> entryCounts = new(); // 입장 횟수
    public long resetTime = 0;

    
}
