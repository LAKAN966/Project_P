using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void AddActionPoint(int amount)
    {
        player.actionPoint += amount;
    }

    public bool AddUnit(int id)
    {
        if (player.myUnitIDs.Contains(id))
        {
            Debug.Log("이미 동일한 유닛이 존재합니다.");
            return false;
        }

        player.myUnitIDs.Add(id);
        Save();
        return true;
    }

    public bool HasUnit(int id)
    {
        return player.myUnitIDs.Contains(id);
    }

    public List<UnitStats> GetAllNormalUnit()
    {
        return player.myUnitIDs
            .Select(id => UnitDataManager.Instance.GetStats(id))
            .Where(stat => stat != null && !stat.IsHero)
            .ToList();
    }

    public List<UnitStats> GetAllLeaderUnit()
    {
        return player.myUnitIDs
            .Select(id => UnitDataManager.Instance.GetStats(id))
            .Where(stat => stat != null && stat.IsHero)
            .ToList();
    }

    public List<UnitStats> GetAllUnit()
    {
        return player.myUnitIDs
            .Select(id => UnitDataManager.Instance.GetStats(id))
            .Where(stat => stat != null)
            .ToList();
    }
 
    public void ClearStage(int stageID)
    {
        if (!player.clearedStageIDs.Contains(stageID))
        {
            player.clearedStageIDs.Add(stageID);
        }
    }

    public void RefreshActionPoint()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long last = player.lastActionPointTime;

        if(last == 0)
        {
            player.lastActionPointTime = now;
            return;
        }

        long elapsedSeconds = now - last;
        int recovered = (int)(elapsedSeconds / 60); // 1분에 행동력 1씩 회복

        if (recovered > 0)
        {
            player.actionPoint = Mathf.Min(player.actionPoint + recovered, 100); // 최대치 100과 비교해서 낮은쪽 사용.
            player.lastActionPointTime += recovered * 60;
        }
    }

    public bool UseActionPoint(int amount)
    {
        RefreshActionPoint();

        if(player.actionPoint >= amount)
        {
            player.actionPoint -= amount;
            return true;
        }

        else
        {
            Debug.Log("행동력이 부족합니다.");
            return false;
        }
    }

}
