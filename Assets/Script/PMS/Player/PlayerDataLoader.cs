using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataLoader : MonoBehaviour
{
    private async void Start()
    {
        await PlayerDataManager.Instance.Load();
    }
}
