using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
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

    public bool isInit = false;
    public void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;
        GetServerTimeAndCheckReset();
        InitializePlayerQuest();
        RegistEvent();
    }

    void GetServerTimeAndCheckReset()
    {
        var dbtime = FirebaseDatabase.DefaultInstance.GetReference("serverTime");
        dbtime.SetValueAsync(ServerValue.Timestamp).ContinueWith(setTask =>
        {
            dbtime.GetValueAsync().ContinueWith(getTask =>
            {
                if (getTask.IsCompletedSuccessfully)
                {
                    long time = (long)getTask.Result.Value;
                    DateTime serverTime = DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime;
                    CheckQuestReset(serverTime);
                }
                else
                {
                    Debug.Log("서버 시간 불러오기 실패.");
                }
            });
        });
    }

    void CheckQuestReset(DateTime serverTime)
    {
        var player = PlayerDataManager.Instance.player;

        TimeZoneInfo kst = TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul");

        DateTime serverTimeKst = TimeZoneInfo.ConvertTimeFromUtc(serverTime, kst).Date;
        DateTime lastDailyUtc = DateTimeOffset.FromUnixTimeMilliseconds(player.lastDailyQuestTime).UtcDateTime;
        DateTime lastDailyKst = TimeZoneInfo.ConvertTimeFromUtc(lastDailyUtc, kst).Date;

        if (serverTimeKst > lastDailyKst)
        {
            ResetDailyQuests();
            player.lastDailyQuestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        DateTime weekly = DateTimeOffset.FromUnixTimeMilliseconds(player.lastWeeklyQuestTime).UtcDateTime;
        if(serverTime.DayOfWeek == DayOfWeek.Monday && GetWeekNumber(serverTime) != GetWeekNumber(weekly))
        {
            ResetWeeklyQuests();
            player.lastWeeklyQuestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

    }

    int GetWeekNumber(DateTime serverTime)
    {
        var ci = System.Globalization.CultureInfo.InvariantCulture;
        return ci.Calendar.GetWeekOfYear(serverTime, System.Globalization.CalendarWeekRule.FirstFourDayWeek,DayOfWeek.Monday);
    }

    void ResetDailyQuests()
    {
        var dailyQuestID = QuestDataManager.Instance.GetDailyQuests().Select(quest => quest.ID).ToHashSet();
        var playerQuests = PlayerDataManager.Instance.player.playerQuest;

        foreach (var quest in playerQuests)
        {
            if (dailyQuestID.Contains(quest.QuestID))
            {
                quest.CurrentValue = 0;
                quest.IsCompleted = false;
                quest.IsReward = false;
            }
        }
    }

    void ResetWeeklyQuests()
    {
        var weeklyQuestID = QuestDataManager.Instance.GetDailyQuests().Select(quest => quest.ID).ToHashSet();
        var playerQuests = PlayerDataManager.Instance.player.playerQuest;

        foreach (var quest in playerQuests)
        {
            if (weeklyQuestID.Contains(quest.QuestID))
            {
                quest.CurrentValue = 0;
                quest.IsCompleted = false;
                quest.IsReward = false;
            }
        }
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

    void RegistEvent()
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
