using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    public Button EnterGameButton;
    private bool isLoading = false;

    [SerializeField] private GameObject errorPopup;
    [SerializeField] private TextMeshProUGUI errorText;

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

        float timeout = 10f;
        float timer = 0f;

        while (!PlayerDataManager.Instance.IsLoaded)
        {
            if (timer > timeout)
            {
                ShowError("네트워크가 불안정하여 데이터 로딩에 실패했습니다.");
                isLoading = false;
                yield break;
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (PlayerDataManager.Instance.LoadFailed)
        {
            ShowError("저장된 데이터를 불러오지 못했습니다.\n기본 데이터로 시작합니다.");
            yield return new WaitForSeconds(2f);
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
    private void ShowError(string msg)
    {
        errorText.text = msg;
        errorPopup.SetActive(true);
    }
}
