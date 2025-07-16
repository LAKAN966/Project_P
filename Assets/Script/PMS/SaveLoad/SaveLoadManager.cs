using System;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveLoadManager
{
    public static string UserID => SystemInfo.deviceUniqueIdentifier;
    public static async Task<bool> Save<T>(string key, T data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
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
    
    public static async Task<T> Load<T>(string key, T defaultValue = default) 
    {
        try
        {
            var data = await FirebaseDatabase.DefaultInstance.GetReference($"users/{UserID}/{key}").GetValueAsync();

            if(data.Exists && data.Value != null)
            {
                string json = data.Value.ToString();
                T load = JsonConvert.DeserializeObject<T>(json);
                Debug.Log($"불러오기 성공 {key}");
                return load;
            }
            else
            {
                Debug.Log($"불러올 데이터가 없습니다. {key}");
                return defaultValue;
            }
        }
        catch(Exception exception)
        {
            Debug.LogError($"불러오기 실패 {key}. {exception.Message}");
            return defaultValue;
        }
        
    }
}
