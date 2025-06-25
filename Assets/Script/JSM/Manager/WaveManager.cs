using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public TextAsset stageCSV;
    public int stageID;  // 외부에서 설정
    private StageData currentStage;
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
        currentStage = StageDataLoader.LoadByID(stageCSV, id);
        if (currentStage == null) return;

        string assetPath = "Assets/Data/" + currentStage.StageName + ".csv";
        TextAsset waveText = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
        if (waveText == null)
        {
            Debug.LogError($"CSV not found at {assetPath}");
            return;
        }

        waves = WaveDataLoader.Load(waveText);
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

        // 대기 웨이브가 없으면 새로운 웨이브 탐색
        if (pendingWave == null)
        {
            if ((triggerActive?triggerWaves[waveCount] : nonTriggerWaves[waveCount]).Time <= timer && (triggerActive ? triggerWaves.Count : nonTriggerWaves.Count) > waveCount)
            {
                pendingWave = triggerActive ? triggerWaves[waveCount] : nonTriggerWaves[waveCount];
                waveCount++;
            }
        }
        else
        {
            if (enemyPool.HasAvailable())
            {
                isPaused = false;
                if (SpawnEnemy(pendingWave.EnemyID))
                {
                    Debug.Log(pendingWave.Time+":"+waveCount);
                    pendingWave = null;
                    isPaused = false;
                }
            }
            else
            {
                isPaused = true;
                return;
            }
        }

        if (timer >= currentStage.ResetTime)
        {
            timer = 0f;
            waveCount = 0;
            Debug.Log("시간초기화!");
        }

        if (isPaused && enemyPool.HasAvailable())
        {
            isPaused = false;
        }
    }



    private bool SpawnEnemy(int enemyID)
    {
        var stats = UnitDataManager.Instance.GetStats(enemyID);
        if (stats == null)
        {
            Debug.LogWarning($"EnemyID {enemyID} 유닛 데이터 없음");
            return false;
        }

        var pos = UnitSpawner.Instance.GetSpawnPosition(true); // 적군
        var pool = stats.IsHero ? UnitSpawner.Instance.enemyHeroPool : UnitSpawner.Instance.enemyPool;

        var unit = pool.GetUnit(stats, pos);
        if (unit == null)
        {
            Debug.Log($"EnemyID {enemyID} 스폰 실패: 풀 없음");
            return false;
        }

        return true;
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
    }
}
