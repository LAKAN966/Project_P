using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Button EnterGameButton;
    private bool isLoading = false;

    void Start()
    {
        EnterGameButton.onClick.AddListener(OnClickEnter);
    }

    private void OnClickEnter()
    {
        if (isLoading) return;
        StartCoroutine(WaitForPlayerDataAndEnter());
    }

    private IEnumerator WaitForPlayerDataAndEnter()
    {
        isLoading = true;

        while (!PlayerDataManager.Instance.IsLoaded)
        {
            yield return null;
        }

        EnterGame();
    }

    public void EnterGame()
    {
        if (!PlayerDataManager.Instance.player.tutorialDone[0])
        {
            TutorialManager.Instance.StartTuto(0);
            return;
        }
        SceneManager.LoadScene("MainScene");
        SFXManager.Instance.PlaySFX(0);
    }
}
