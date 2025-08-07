using Firebase;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerDataLoader : MonoBehaviour
{
    private async void Start()
    {
        if (FirebaseInitializer.IsInitialized)
        {
            await PlayerDataManager.Instance.Load();
        }
        else
        {
            FirebaseInitializer.OnFirebaseInitialized += async () =>
            {
                await PlayerDataManager.Instance.Load();
            };
        }
    }
}
