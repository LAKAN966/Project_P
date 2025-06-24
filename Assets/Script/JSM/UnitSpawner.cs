using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SpawnButtonData
{
    public Button button;
    public int unitID;
    public bool isEnemy;
    public bool isHero; // 추가: 영웅 여부
}

public class UnitSpawner : MonoBehaviour
{
    public UnitPool allyPool;
    public UnitPool enemyPool;

    public UnitPool allyHeroPool;  // 추가: 아군 영웅 풀 (단일 유닛)
    public UnitPool enemyHeroPool; // 추가: 적군 영웅 풀 (단일 유닛)

    public Vector2 allySpawnPosition = new Vector2(-7f, 0f);
    public Vector2 enemySpawnPosition = new Vector2(7f, 0f);

    public List<SpawnButtonData> spawnButtons = new();

    void Start()
    {
        foreach (var data in spawnButtons)
        {
            data.button.onClick.AddListener(() => Spawn(data));
        }
    }

    void Spawn(SpawnButtonData data)
    {
        var stats = UnitDataManager.Instance.GetStats(data.unitID);
        if (stats == null)
        {
            Debug.LogWarning($"알 수 없는 유닛 ID: {data.unitID}");
            return;
        }

        var spawnPos = data.isEnemy ? enemySpawnPosition : allySpawnPosition;

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
            }
        }
    }
}
