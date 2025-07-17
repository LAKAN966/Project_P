using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameoverUI;
    public GameObject allyPool;
    public GameObject enemyPool;
    public GameObject allyHeroPool;
    public GameObject enemyHeroPool;
    public BattleSpeed battleSpeed;
    public GameObject touchBlock;
    public Timer timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        gameoverUI.GetComponent<Button>().onClick.AddListener(OnClicked);
    }


    public void OnBaseDestroyed(bool isEnemyBase)
    {
        gameoverUI.SetActive(true);
        touchBlock.SetActive(true);
        if (isEnemyBase)
        {//승리
            gameoverUI.GetComponent<GameOverPanel>().Win();
            enemyPool.SetActive(false);
            enemyHeroPool.SetActive(false);
        }
        else
        {//패배
            gameoverUI.GetComponent<GameOverPanel>().Lose();
            allyPool.SetActive(false);
            allyHeroPool.SetActive(false);
        }
        timer.m_Running = false;
        battleSpeed.gameSpeed = 1;
    }
    public void OnClicked()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainScene");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(WaveManager.Instance.stageType != 2 || PlayerDataManager.Instance.player.goldDungeonData.entryCounts > 0)
        {
            StageManager.instance.AddReward(WaveManager.Instance.stageID);
        }
        if (WaveManager.Instance.stageType == 2)
        {
            PlayerDataManager.Instance.player.goldDungeonData.entryCounts--;
        }

        StageManager.instance.ClearStage(WaveManager.Instance.stageID);
        UIController.Instance.OpenStage();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}