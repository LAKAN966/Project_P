using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public int stageID;  // 외부에서 설정
    public StageData currentStage;
    private List<WaveData> waves = new();
    private List<WaveData> triggerWaves;
    private List<WaveData> nonTriggerWaves;

    private float timer = 0f;
    private bool triggerActive = false;
    private bool waveStarted = false;
    private bool isPaused = false;
    private WaveData? pendingWave = null;
    private int waveCount;

    public UnitPool enemyPool;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        LoadStageAndWave(stageID);
        StartCoroutine(StartWaveAfterTeaTime());
    }

    private void LoadStageAndWave(int id)
    {
        TextAsset stageCSV = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/StageData.csv");

        currentStage = StageDataLoader.LoadByID(stageCSV, id);
        if (currentStage == null) return;

        Debug.Log($"스테이지 로딩: {currentStage.StageName}");
        TextAsset waveText = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/WaveData.csv");
        if (waveText == null)
        {
            Debug.LogError($"WaveData.csv 파일이 없습니다: {"Assets/Data/WaveData.csv"}");
            return;
        }

        var allWaves = WaveDataLoader.Load(waveText);
        waves = allWaves.Where(w => w.StageID == id).ToList();

        waveCount = 0;
        triggerWaves = waves.Where(w => w.OnTrigger).ToList();
        nonTriggerWaves = waves.Where(w => !w.OnTrigger).ToList();
    }


    IEnumerator StartWaveAfterTeaTime()
    {
        yield return new WaitForSeconds(currentStage.TeaTime);
        waveStarted = true;
    }

    void Update()
    {
        if (!waveStarted) return;

        if (!isPaused)
            timer += Time.deltaTime;

        if (pendingWave == null)
        {
            var currentList = triggerActive ? triggerWaves : nonTriggerWaves;
            if (waveCount < currentList.Count &&
                currentList[waveCount].Time <= timer)
            {
                pendingWave = currentList[waveCount];
            }
        }
        else
        {
            if (enemyPool.HasAvailable())
            {
                if (UnitSpawner.Instance.SpawnEnemy(pendingWave.EnemyID))
                {
                    Debug.Log(pendingWave.Time + " : " + waveCount);
                    pendingWave = null;
                    waveCount++;
                    isPaused = false;
                }
                else
                {
                    isPaused = true;
                    return;
                }
            }
            else
            {
                isPaused = true;
                return;
            }
        }

        if (pendingWave == null && timer >= currentStage.ResetTime)
        {
            timer = 0f;
            waveCount = 0;
            Debug.Log("시간 초기화!");
        }

        if (isPaused && enemyPool.HasAvailable())
        {
            isPaused = false;
        }
    }

    public void TriggerWave()
    {
        Debug.Log("트리거 ON");
        if (!triggerActive)
        {
            triggerActive = true;
            timer = 0f;
            waveCount = 0;
            isPaused = false;
        }
        UnitSpawner.Instance.SpawnEnemyHero(currentStage.EnemyHeroID);
    }
}
