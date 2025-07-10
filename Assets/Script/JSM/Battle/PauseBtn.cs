using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    public Button pauseBtn;
    public Button resumeBtn;
    public Button retreatBtn;

    private float gameSpeed=1;
    public void Awake()
    {
        pauseBtn.onClick.AddListener(Pause);
        resumeBtn.onClick.AddListener(Resume);
        retreatBtn.onClick.AddListener(Retreat);
    }
    private void Pause()
    {
        gameSpeed = Time.timeScale;
        Time.timeScale = 0;
    }
    private void Resume()
    {
        Time.timeScale = gameSpeed;
    }
    private void Retreat()
    {
        SceneManager.LoadScene("MainScene");
    }
}
