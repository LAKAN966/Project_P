using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void CloseTab() // 인트로에서도 사용.
    {
        SceneManager.LoadScene("MainScene");    
    }

    public void ToStage()
    {
        SceneManager.LoadScene("StageScene");
    }

    public void ToDeckBuild()
    {
        SceneManager.LoadScene("DeckBuildScene");
    }

    public void ToPickUp()
    {
        SceneManager.LoadScene("GotchaScene");
    }

    public void ToHQ()
    {
        SceneManager.LoadScene("HQScene");
    }

    public void ToBattle()
    {
        SceneManager.LoadScene("BattleScene");

    }
}
