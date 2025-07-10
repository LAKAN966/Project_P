using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UnitSpawner : MonoBehaviour
{
    public UnitPool allyPool;
    public UnitPool enemyPool;

    public UnitPool allyHeroPool;  // 추가: 아군 영웅 풀 (단일 유닛)
    public UnitPool enemyHeroPool; // 추가: 적군 영웅 풀 (단일 유닛)

    public Vector2 allySpawnPosition;
    public Vector2 enemySpawnPosition;

    public static UnitSpawner Instance;

    [System.Serializable]
    public class ButtonSetting
    {
        public SpawnButton spawnButton;
        public int unitID;
        public bool isEnemy;
        public bool isHero;
    }

    public List<ButtonSetting> buttonSettings = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        allySpawnPosition = GetSpawnPosition(false);
        enemySpawnPosition = GetSpawnPosition(true);

        SetButton();
    }
    public void Init(List<UnitStats> normalDeck, UnitStats leaderDeck)
    {
        for (int i = 0; i < normalDeck.Count; i++)
        {
            buttonSettings[i].unitID = normalDeck[i].ID;
        }
        buttonSettings[6].unitID = leaderDeck.ID;
    }
    private void TrySpawn(SpawnButton data)
    {
        var stats = UnitDataManager.Instance.GetStats(data.unitID);
        if (stats == null)
        {
            Debug.LogWarning($"알 수 없는 유닛 ID: {data.unitID}");
            return;
        }
        stats = BuffManager.ApplyBuff(stats);

        var spawnPos = data.isEnemy ? enemySpawnPosition : allySpawnPosition;

        if (BattleResourceManager.Instance.currentResource<stats.Cost)
        {
            Debug.Log($"자원이 부족합니다.");
            return;
        }

        if (!CoolTimeManager.Instance.CanSpawn(data.unitID))
        {
            Debug.Log($"유닛 쿨타임 중입니다.");
            return;
        }

        if (data.isHero)
        {
            var heroPool = data.isEnemy ? enemyHeroPool : allyHeroPool;
            var hero = heroPool.GetUnit(stats, spawnPos);
            if (hero == null)
            {
                Debug.LogWarning($"{(data.isEnemy ? "적" : "아군")} 영웅 유닛 풀 부족!");
                return;
            }
        }
        else
        {
            var pool = data.isEnemy ? enemyPool : allyPool;
            var unit = pool.GetUnit(stats, spawnPos);
            if (unit == null)
            {
                Debug.LogWarning($"{(data.isEnemy ? "적군" : "아군")} 유닛 풀 부족!");
                return;
            }
        }
        BattleResourceManager.Instance.Spend(stats.Cost);
        CoolTimeManager.Instance.SetCooldown(data.unitID, stats.SpawnInterval);
    }

    public Vector3 GetSpawnPosition(bool isEnemy)
    {
        float offset = 2f;
        return isEnemy
            ? new Vector3(WaveManager.Instance.currentStage.BaseDistance / 2f - offset, 0, 0)
            : new Vector3(-WaveManager.Instance.currentStage.BaseDistance / 2f + offset, 0, 0);
    }
    public bool SpawnEnemy(int unitID)
    {
        var stats = UnitDataManager.Instance.GetStats(unitID);
        if (stats == null)
        {
            Debug.LogWarning($"[UnitSpawner] 알 수 없는 유닛 ID: {unitID}");
            return false;
        }

        var unit = enemyPool.GetUnit(stats, enemySpawnPosition);
        if (unit == null)
        {
            Debug.LogWarning("[UnitSpawner] 적군 유닛 풀 부족!");
            return false;
        }
        return true;
    }
    public void SpawnEnemyHero(int unitID)
    {
        var stats = UnitDataManager.Instance.GetStats(unitID);
        if (stats == null)
        {
            Debug.LogWarning($"[UnitSpawner] 알 수 없는 유닛 ID: {unitID}");
            return;
        }

        var hero = enemyHeroPool.GetUnit(stats, enemySpawnPosition);
        if (hero == null)
        {
            Debug.LogWarning("[UnitSpawner] 적군 영웅 유닛 풀 부족!");
        }
    }
    public void SetButton()
    {
        foreach (var setting in buttonSettings)
        {
            var button = setting.spawnButton;
            button.unitID = setting.unitID;
            button.isEnemy = setting.isEnemy;
            button.isHero = setting.isHero;

            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(() => TrySpawn(button));
        }
    }
}
