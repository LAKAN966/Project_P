using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class QuestDataManager
{
    private static QuestDataManager instance;
    public static QuestDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestDataManager();
            }
            return instance;
        }
    }

    public List<QuestData> allQuests = new();
    public Dictionary<int, QuestData> questDic = new();

    public async void Init()
    {
        try
        {
            await LoadQuestData();
        }
        catch (Exception ex)
        {
            Debug.LogError($"퀘스트 데이터 불러오기 오류 {ex.Message}");
        }
    }

    private async Task LoadQuestData()
    {
        string path = "questData/quests";
        var dataRef = FirebaseDatabase.DefaultInstance.GetReference(path);
        var task = await dataRef.GetValueAsync();

        if (task == null || !task.Exists)
        {
            Debug.Log("퀘스트 데이터 로드 실패");
            return;
        }

        allQuests.Clear();
        questDic.Clear();

        foreach (var child in task.Children)
        {
            string json = child.GetRawJsonValue();
            QuestData quest = JsonConvert.DeserializeObject<QuestData>(json);

            if (quest != null)
            {
                allQuests.Add(quest);
                questDic[quest.ID] = quest;
            }
        }
        allQuests.Sort((a, b) => a.Order.CompareTo(b.Order));
        Debug.Log($"{allQuests.Count}개 로드 완료");
    }

    public QuestData GetQuestID(int id)
    {
        questDic.TryGetValue(id, out QuestData quest);
        return quest;
    }

    public List<QuestData> GetDailyQuests()
    {
        return allQuests.FindAll(quest => quest.Type == QuestType.Daily);
    }
    public List<QuestData> GetWeeklyQuests()
    {
        return allQuests.FindAll(quest => quest.Type == QuestType.Weekly);
    }

}
