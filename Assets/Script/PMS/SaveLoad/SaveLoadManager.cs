using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public interface SaveTime
{
    long lastSaveTime { get; set; }
}

public static class SaveLoadManager
{
    public static string UserID => SystemInfo.deviceUniqueIdentifier;
    private static string GetLocalPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, $"{key}.json");
    }
    public static async Task<bool> Save<T>(string key, T value) where T : SaveTime
    {
        value.lastSaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        string json = JsonConvert.SerializeObject(value);

        try
        {
            File.WriteAllText(GetLocalPath(key), json);
        }
        catch (Exception e)
        {
            Debug.LogError($"로컬 저장 실패 {key}: {e.Message}");
            return false;
        }

        try
        {
            await FirebaseDatabase.DefaultInstance.GetReference($"users/{UserID}/{key}").SetValueAsync(json);
            Debug.Log($"저장 성공 {key}");
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError($"저장 실패 {key}. {exception.Message}");
            return false;
        }
    }
    
    public static async Task<T> Load<T>(string key, T defaultValue = default) where T : SaveTime
    {

        T localData = default;
        T remoteData = default;

        string localPath = GetLocalPath(key);
        if (File.Exists(localPath))
        {
            try
            {
                string localJson = File.ReadAllText(localPath);
                localData = JsonConvert.DeserializeObject<T>(localJson);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"로컬 데이터 읽기 실패 {key}: {e.Message}");
            }
        }

        try
        {
            var data = await FirebaseDatabase.DefaultInstance.GetReference($"users/{UserID}/{key}").GetValueAsync();

            if(data.Exists && data.Value != null)
            {
                string json = data.Value.ToString();
                remoteData = JsonConvert.DeserializeObject<T>(json);
            }
            
        }
        catch(Exception exception)
        {
            Debug.LogError($"불러오기 실패 {key}. {exception.Message}");
            return defaultValue;
        }

        if (localData == null && remoteData == null)
            return defaultValue;

        if (localData != null && remoteData == null)
            return localData;

        if (localData == null && remoteData != null)
        {
            Save(key, remoteData);
            return remoteData;
        }

        if (localData.lastSaveTime >= remoteData.lastSaveTime)
        {
            Save(key, localData);
            return localData;
        }
        else
        {
            Save(key, remoteData);
            return remoteData;
        }
    }
}
