using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SpawnButtonData
{
    public Button button;       // UI 버튼
    public int unitID;          // 소환할 유닛 ID
    public bool isEnemy;        // 적/아군 여부
}

public class UnitSpawner : MonoBehaviour
{
    public UnitPool allyPool;
    public UnitPool enemyPool;

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
        var stats = DataManager.Instance.GetStats(data.unitID);
        if (stats == null)
        {
            Debug.LogWarning($"알 수 없는 유닛 ID: {data.unitID}");
            return;
        }

        var spawnPos = data.isEnemy ? enemySpawnPosition : allySpawnPosition;
        var pool = data.isEnemy ? enemyPool : allyPool;

        var unit = pool.GetUnit(stats, spawnPos);
        if (unit == null)
        {
            Debug.LogWarning($"{(data.isEnemy ? "적군" : "아군")} 유닛 풀 부족!");
        }
    }
}
