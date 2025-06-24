using UnityEngine;
using System.Collections.Generic;

public class GraveObject : MonoBehaviour
{
    private static readonly List<GraveObject> activeGraves = new();

    private Vector3 spawnPosition;
    private bool isEnemy;
    private GraveSpawnSkill skillRef;

    public void Init(Vector3 pos, bool isEnemy, GraveSpawnSkill skill)
    {
        this.spawnPosition = pos;
        this.isEnemy = isEnemy;
        this.skillRef = skill;
        activeGraves.Add(this);
    }

    public void ActivateZombie()
    {
        skillRef?.SpawnZombie(spawnPosition, isEnemy);
        activeGraves.Remove(this);
        Destroy(gameObject);
    }
    public static List<GraveObject> GetAllGraves()
    {
        return new List<GraveObject>(activeGraves);
    }
}
