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

}
