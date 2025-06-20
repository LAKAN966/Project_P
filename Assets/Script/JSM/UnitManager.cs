using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    public List<Unit> allyUnits = new();
    public List<Unit> enemyUnits = new();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterUnit(Unit unit)
    {
        if (unit.isEnemy) enemyUnits.Add(unit);
        else allyUnits.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        if (unit.isEnemy) enemyUnits.Remove(unit);
        else allyUnits.Remove(unit);
    }

    public List<Unit> GetOpposingUnits(bool isEnemy)
    {
        return isEnemy ? allyUnits : enemyUnits;
    }

    public void ReturnToPool(Unit unit)
    {
        UnregisterUnit(unit);
        // 풀로 다시 넣기
    }
}
