using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
    private static PlayerDataManager instance;

    public static PlayerDataManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerDataManager();
            }
            return instance;
        }
    }

    public Player player = new();

    public void Save() // 플레이어 데이터 저장
    {

    }

    public void Load() // 플레이어 데이터 불러오기
    {

    }
    public void AddGold(int amount)
    {
        player.gold += amount;
    }

    public void AddTicket(int amount)
    {
        player.ticket += amount;
    }

}
