using Firebase;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerDataLoader : MonoBehaviour
{
    private async void Start()
    {
        await WaitForFirebaseInitialization();

        await PlayerDataManager.Instance.Load();
    }

    private async Task WaitForFirebaseInitialization()
    {
        DependencyStatus status = DependencyStatus.UnavailableOther;

        while (FirebaseApp.DefaultInstance == null || status != DependencyStatus.Available)
        {
            var task = FirebaseApp.CheckAndFixDependenciesAsync();
            status = await task;

            if (status == DependencyStatus.Available)
            {
                Debug.Log("Firebase is initialized and ready.");
                return;
            }
            else
            {
                Debug.LogWarning($"Firebase dependencies not ready yet: {status}. Retrying...");
                await Task.Delay(500);
            }
        }
    }
}
