using System.Collections.Generic;
using UnityEngine;

public static class BuffManager
{
    public static void InitBuffs()
    {
        PlayerDataManager.Instance.player.raceBuffTable.Clear();

        foreach (int raceID in RaceManager.GetAll().Keys)
        {
            UnitStats buff = new UnitStats
            {
                ID = raceID,
                RaceID = raceID,
                Name = RaceManager.GetNameByID(raceID),
                AttackRange = 1f,
                Damage = 1f,
                MaxHP = 1f,
                MoveSpeed = 1f,
                SpawnInterval = 1f,
                Cost = 1,
                Hitback = 1,
                PreDelay = 1f,
                PostDelay = 1f,
                AttackType = 1,
                Size = 1f,
                tagId = null,
            };

            PlayerDataManager.Instance.player.raceBuffTable[raceID] = buff;
        }

        PlayerDataManager.Instance.player.tagBuffTable.Clear();

        foreach (int tagID in TagManager.GetAll().Keys)
        {
            UnitStats buff = new UnitStats
            {
                ID = tagID,
                RaceID = tagID,
                Name = RaceManager.GetNameByID(tagID),
                AttackRange = 1f,
                Damage = 1f,
                MaxHP = 1f,
                MoveSpeed = 1f,
                SpawnInterval = 1f,
                Cost = 1,
                Hitback = 1,
                PreDelay = 1f,
                PostDelay = 1f,
                AttackType = 1,
                Size = 1f,
            };

            PlayerDataManager.Instance.player.tagBuffTable[tagID] = buff;
        }
    }

    public static UnitStats GetBuff(UnitStats baseStats)
    {
        GetRaceBuff()
    }

    public static UnitStats GetRaceBuff(int raceID)
    {
        return PlayerDataManager.Instance.player.raceBuffTable.TryGetValue(raceID, out var buff) ? buff : null;
    }

    public static Dictionary<int, UnitStats> GetTagBuff(List<int> tagID)
    {
        Dictionary<int, UnitStats> list = new Dictionary<int, UnitStats>();
        foreach(int i in tagID) 
        {
            list[i] = PlayerDataManager.Instance.player.tagBuffTable.TryGetValue(i, out var buff) ? buff : null;
        }

        return list;
    }

    public static UnitStats ApplyBuff(UnitStats baseStats)
    {
        var buff = GetBuff(baseStats);
        if (buff == null) return baseStats;

        return new UnitStats
        {
            ID = baseStats.ID,
            Name = baseStats.Name,
            Description = baseStats.Description,
            RaceID = baseStats.RaceID,
            IsHero = baseStats.IsHero,
            IsAOE = baseStats.IsAOE,
            AttackRange = baseStats.AttackRange * buff.AttackRange,
            Damage = baseStats.Damage * buff.Damage,
            MaxHP = baseStats.MaxHP * buff.MaxHP,
            MoveSpeed = baseStats.MoveSpeed * buff.MoveSpeed,
            SpawnInterval = baseStats.SpawnInterval * buff.SpawnInterval,
            Cost = Mathf.RoundToInt(baseStats.Cost * buff.Cost),
            Hitback = Mathf.RoundToInt(baseStats.Hitback * buff.Hitback),
            PreDelay = baseStats.PreDelay * buff.PreDelay,
            PostDelay = baseStats.PostDelay * buff.PostDelay,
            ModelName = baseStats.ModelName,
            AttackType = baseStats.AttackType,
            Size = baseStats.Size * buff.Size,
            SkillID = baseStats.SkillID
        };
    }

    public static void UpdateBuffStat(int raceID, List<int> statIndex, float value)
    {
        if (!PlayerDataManager.Instance.player.raceBuffTable.ContainsKey(raceID))
        {
            Debug.LogWarning($"Race ID {raceID}에 대한 버프가 존재하지 않습니다.");
            return;
        }
        var buff = PlayerDataManager.Instance.player.raceBuffTable[raceID];
        value *= 0.01f;
        foreach( var i in statIndex ) 
        {
            switch (i)
            {
                case 0: buff.Damage += value; break;
                case 1: buff.MaxHP += value; break;
                case 2: buff.MoveSpeed += value; break;
                case 3: buff.AttackRange += value; break;
                case 4: buff.SpawnInterval += value; break;
                case 5: buff.Cost += Mathf.RoundToInt(value); break;
                case 6: buff.Hitback += Mathf.RoundToInt(value); break;
                case 7: buff.PreDelay += value; break;
                case 8: buff.PostDelay += value; break;
                case 9: buff.Size += value; break;
                default:
                    Debug.LogWarning($"알 수 없는 statIndex: {statIndex}");
                    return;
            }
        }
        PlayerDataManager.Instance.player.raceBuffTable[raceID] = buff;
    }
}
