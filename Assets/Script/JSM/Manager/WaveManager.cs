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

    private float timer = 0f;
    private bool triggerActive = false;
    private bool waveStarted = false;

    private HashSet<(float time, int enemyID)> triggeredWaves = new();

    private bool isPaused = false;
    private List<WaveData> pendingWaves = new();

    public UnitPool enemyPool;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
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
    }

    IEnumerator StartWaveAfterTeaTime()
    {
        yield return new WaitForSeconds(currentStage.TeaTime);
        waveStarted = true;
    }

    void Update()
    {
        Debug.Log(isPaused + " : " + timer);
        if (!waveStarted) return;

        if (!isPaused) timer += Time.deltaTime;

        foreach (var wave in waves)
        {
            if (wave.Time > timer) continue;

            if (triggerActive == wave.OnTrigger) continue;

            if (enemyPool.HasAvailable())
            {
                SpawnEnemy(wave.EnemyID);
            }
            else
            {
                timer -= 0.1f;
                isPaused = true;
                return;
            }
        }

        if (timer >= currentStage.ResetTime)
        {
            timer = 0f;
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
            isPaused = false;
        }
    }
}
