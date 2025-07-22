using UnityEngine;
using System.Collections.Generic;

public class GraveSpawnSkill : MonoBehaviour
{
    public GameObject gravePrefab;
    public UnitPool allyPool;
    public UnitPool enemyPool;
    public int zombieUnitID = 1001;

    public void TrySpawnGrave(Unit unit)
    {
        Vector3 pos = unit.transform.position;
        GameObject grave = Instantiate(gravePrefab, pos, Quaternion.identity);

        var graveComp = grave.GetComponent<GraveObject>();
        if (graveComp != null)
        {
            graveComp.Init(pos, unit.isEnemy, this);
        }
    }

    public void SpawnZombie(Vector3 pos, bool isEnemy)
    {
        var stat = UnitDataManager.Instance.GetStats(zombieUnitID);
        if (stat == null)
        {
            Debug.LogWarning("좀비 유닛 데이터를 찾을 수 없습니다.");
            return;
        }

        var pool = isEnemy ? enemyPool : allyPool;
        var unit = pool.GetUnit(stat, pos);
        if (unit == null)
        {
            Debug.LogWarning("좀비 유닛 풀 부족!");
            return;
        }
    }

    public void ActivateGraves(bool isEnemy)
    {
        var graves = GraveObject.GetAllGraves();

        foreach (var grave in graves)
        {
            if (grave != null && grave.isEnemy == isEnemy)
            {
                grave.ActivateZombie();
            }
        }
    }
}
