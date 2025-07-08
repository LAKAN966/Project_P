using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject GameoverUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        GameoverUI.GetComponent<Button>().onClick.AddListener(OnClicked);
    }


    public void OnBaseDestroyed(bool isEnemyBase)
    {
        GameoverUI.SetActive(true);
        if (isEnemyBase)
        {//승리
            GameoverUI.GetComponent<GameOverPanel>().Win();
        }
        else
        {//패배
            GameoverUI.GetComponent<GameOverPanel>().Lose();
        }

        UnitSpawner spawner = FindObjectOfType<UnitSpawner>();
        if (spawner != null)
            spawner.enabled = false;
    }
    public void OnClicked()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainScene");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StageManager.instance.AddReward();
        StageManager.instance.ClearStage();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}