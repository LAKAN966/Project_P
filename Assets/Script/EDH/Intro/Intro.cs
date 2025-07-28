using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Button EnterGameButton;
    void Start()
    {
        EnterGameButton.onClick.AddListener(EnterGame);
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("MainScene");
        SFXManager.Instance.PlaySFX(0);
    }
}
