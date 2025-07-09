using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new QuestManager();
            }
            return instance;
        }
    }

    public void Init()
    {
        InitializePlayerQuest();
        OnEnable();
    }

    void InitializePlayerQuest() // 플레이어 퀘스트 초기화
    {
        var allQuests = QuestDataManager.Instance.allQuests;
        var playerQuests = PlayerDataManager.Instance.player.playerQuest;
        
        if(playerQuests.Count > 0)
        {
            return;
        }

        foreach (var quest in allQuests)
        {
            bool exists = playerQuests.Exists(q => q.QuestID == quest.ID);
            if (!exists)
            {
                playerQuests.Add(new PlayerQuestData
                {
                    QuestID = quest.ID,
                    CurrentValue = 0,
                    IsCompleted = false,
                    IsReward = false
                });
            }
        }
    }

    void OnEnable()
    {
        QuestEvent.UseActionPoint += UseActionPoint;
        QuestEvent.OnLooting += Looting;
        QuestEvent.OnLogin += Login;
        QuestEvent.OnRecruit += Recurit;
        QuestEvent.OnTowerClear += TowerClear;
        QuestEvent.OnMainChapterClear += ChapterClear;
    }

    void UseActionPoint(int value)
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.UseActionPoint, value);
    }

    void Looting()
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.Looting, 1);
    }

    void Login()
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.Login, 1);
    }

    void Recurit(int value)
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.Recruit, value);
    }

    void TowerClear()
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.Tower, 1);
    }

    void ChapterClear()
    {
        PlayerDataManager.Instance.AddQuestProgress(ConditionType.MainChapter, 1);
    }
}
