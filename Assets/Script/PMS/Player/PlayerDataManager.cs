using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class PlayerDataManager
{
    private static PlayerDataManager instance;

    public static PlayerDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerDataManager();
            }
            return instance;
        }
    }

    private Player Player = new Player();
    public Player player => Player;

    public async void Save() // 플레이어 데이터 저장
    {
        Debug.Log("저장 직전 tutorialDone Count: " + player.tutorialDone.Count);
        bool save = await SaveLoadManager.Save("playerJson", player);
        if (save)
        {
            Debug.Log("저장 성공");
        }
    }
    public async Task Load() // 플레이어 데이터 불러오기
    {
        Player load = await SaveLoadManager.Load<Player>("playerJson", null);
        if(load != null)
        {
            Player = load;
            Debug.Log("불러오기 성공");
        }
        else
        {
            Player = new Player();
            Player.AddUnit(1001);
            Player.AddUnit(1002);
            Save();
        }
    }

    public void AddGold(int amount)
    {
        player.gold += amount;
        PlayerCurrencyEvent.OnGoldChange?.Invoke(player.gold);
    }

    public void AddTicket(int amount)
    {
        player.ticket += amount;
        PlayerCurrencyEvent.OnTicketChange?.Invoke(player.ticket);
    }

    public void AddBluePrint(int amount)
    {
        player.bluePrint += amount;
        PlayerCurrencyEvent.OnBluePrintChange?.Invoke(player.bluePrint);
    }

    public void AddActionPoint(int amount)
    {
        player.actionPoint += amount;
        PlayerCurrencyEvent.OnActionPointChange?.Invoke(player.actionPoint);
    }

    public void AddTribute(int amount)
    {
        player.tribute += amount;
        PlayerCurrencyEvent.OnTributeChange?.Invoke(player.tribute);
    }

    public void AddCerti(int amount)
    {
        player.certi += amount;
        PlayerCurrencyEvent.OnCertiChange?.Invoke(player.certi);
    }
    public void AddSpecT(int amount)
    {
        player.specTicket += amount;
        PlayerCurrencyEvent.OnSpecTicketChange?.Invoke(player.specTicket);
    }

    public bool AddUnit(int id)
    {
        if (player.myUnitIDs.Contains(id))
        {
            Debug.Log("이미 동일한 유닛이 존재합니다."+id);
            return false;
        }

        player.myUnitIDs.Add(id);
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

        var stage = StageDataManager.Instance.GetStageData(stageID);
        if (stage.Type == 1)
        {
            int raceID = stage.RaceID;
            int floor = stage.Chapter;

            if(!player.towerData.lastClearFloor.ContainsKey(raceID) || player.towerData.lastClearFloor[raceID] < floor)
            {
                player.towerData.lastClearFloor[raceID] = floor;
            }
        }
    }

    public int RefreshActionPoint()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long last = player.lastActionPointTime;

        if (last == 0)
        {
            //player.lastActionPointTime = now;
            return 60;
        }

        long elapsedSeconds = now - last;
        int recovered = (int)(elapsedSeconds / 60); // 1분에 행동력 1씩 회복

        if (recovered > 0)
        {
            player.actionPoint = Mathf.Min(player.actionPoint + recovered, player.maxActionPoint); // 최대치 100과 비교해서 낮은쪽 사용.
            player.lastActionPointTime += recovered * 60;

            PlayerCurrencyEvent.OnActionPointChange?.Invoke(player.actionPoint);
        }
        return NextRecoverTime();
    }
    public int NextRecoverTime()
    {
        if(player.actionPoint >= player.maxActionPoint)
        {
            return 0;
        }

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long last = player.lastActionPointTime;
        long elapsed = now - last;

        return (int)(60 - (elapsed % 60));
    }

    public bool UseActionPoint(int amount)
    {
        RefreshActionPoint();

        if (player.actionPoint >= amount)
        {
            player.actionPoint -= amount;

            if (player.actionPoint < player.maxActionPoint && player.lastActionPointTime == 0)
            {
                player.lastActionPointTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            PlayerCurrencyEvent.OnActionPointChange?.Invoke(player.actionPoint);
            QuestEvent.UseActionPoint?.Invoke(amount); // 퀘스트 이벤트
            return true;
        }

        else
        {
            Debug.Log("행동력이 부족합니다.");
            return false;
        }
    }

    public bool UseGold(int amount)
    {
        if (player.gold >= amount)
        {
            player.gold -= amount;
            PlayerCurrencyEvent.OnGoldChange?.Invoke(player.gold);
            return true;
        }

        else
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }
    }

    public bool UseTicket(int amount)
    {
        if (player.ticket >= amount)
        {
            player.ticket -= amount;
            PlayerCurrencyEvent.OnTicketChange?.Invoke(player.ticket);
            return true;
        }
        else
        {
            Debug.Log("티켓이 부족합니다.");
            return false;
        }
    }
    public bool UseSpecTicket(int amount)
    {
        if (player.specTicket >= amount)
        {
            player.specTicket -= amount;
            PlayerCurrencyEvent.OnSpecTicketChange?.Invoke(player.specTicket);
            return true;
        }
        else
        {
            Debug.Log("특수모집티켓이 부족합니다.");
            return false;
        }
    }
    public bool UseTribute(int amount)
    {
        if (player.tribute >= amount)
        {
            player.tribute -= amount;
            PlayerCurrencyEvent.OnTributeChange?.Invoke(player.tribute);
            return true;
        }
        else
        {
            Debug.Log("공물이 부족합니다.");
            return false;
        }
    }

    public bool UseBluePrint(int amount)
    {
        if(player.bluePrint >= amount)
        {
            player.bluePrint -= amount;
            PlayerCurrencyEvent.OnBluePrintChange?.Invoke(player.bluePrint);
            return true;
        }

        else
        {
            Debug.Log("설계도가 부족합니다.");
            return false;
        }
    }

    public bool UseCerti(int amount)
    {
        if (player.certi >= amount)
        {
            player.certi -= amount;
            PlayerCurrencyEvent.OnCertiChange?.Invoke(player.certi);
            return true;
        }

        else
        {
            Debug.Log("증명서가 부족합니다.");
            return false;
        }
    }


    public bool HasClearedStage(int stageID)
    {
        return player.clearedStageIDs.Contains(stageID);
    }

    public PlayerQuestData GetQuestProgress(int questID)
    {
        return player.playerQuest.Find(quest => quest.QuestID == questID);
    }
    public bool IsQuestCompleted(int questID)
    {
        var progress = GetQuestProgress(questID);
        return progress != null && progress.IsCompleted;
    }
    public bool HasReceivedQuestReward(int questID)
    {
        var progress = GetQuestProgress(questID);
        return progress != null && progress.IsReward;
    }

    public void AddQuestProgress(ConditionType conditionType, int value) // 플레이어 퀘스트 진행도 추적
    {
        Debug.Log($"[퀘스트 진행도 추가] 호출됨: {conditionType} +{value}");
        foreach (var progress in player.playerQuest)
        {
            QuestData quest = QuestDataManager.Instance.GetQuestID(progress.QuestID);
            if(quest == null || progress.IsCompleted)
            {
                continue;
            }

            if(quest.ConditionType == conditionType)
            {
                progress.CurrentValue += value;
                Debug.Log($"{quest.Title} 진행도 {progress.CurrentValue} / {quest.ConditionValue}");

                if(progress.CurrentValue >= quest.ConditionValue)
                {
                    progress.CurrentValue = quest.ConditionValue;
                    progress.IsCompleted = true;
                    Debug.Log($"{quest.Title} 완료");
                }
            }
        }
    }
    
    public bool TryGetQuestReward(int questID)
    {
        var progress = GetQuestProgress(questID);
        var quest = QuestDataManager.Instance.GetQuestID(questID);

        if(progress == null || !progress.IsCompleted || progress.IsReward)
        {
            return false;
        }

        switch (quest.RewardType)
        {
            case RewardType.Gold:
                AddGold(quest.RewardValue);
                break;
            case RewardType.Ticket:
                AddTicket(quest.RewardValue); 
                break;
            case RewardType.BluePrint:
                AddBluePrint(quest.RewardValue);
                break;
        }

        progress.IsReward = true;
        Debug.Log($"{quest.RewardType} {quest.RewardValue} 획득");
        return true;
    }
}
