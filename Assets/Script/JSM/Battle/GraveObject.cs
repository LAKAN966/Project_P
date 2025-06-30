using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveObject : MonoBehaviour
{
    private static readonly List<GraveObject> activeGraves = new();

    private Vector3 spawnPosition;
    public bool isEnemy;
    private GraveSpawnSkill skillRef;

    public void Init(Vector3 pos, bool isEnemy, GraveSpawnSkill skill)
    {
        this.spawnPosition = pos;
        this.isEnemy = isEnemy;
        this.skillRef = skill;
        activeGraves.Add(this);
        StartCoroutine(AutoRemoveAfterSeconds(3f));
    }
    private IEnumerator AutoRemoveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (activeGraves.Contains(this))
        {
            activeGraves.Remove(this);
            Destroy(gameObject);
        }
    }
    public void ActivateZombie()
    {
        Debug.Log("소환!");
        skillRef.SpawnZombie(spawnPosition, isEnemy);
        activeGraves.Remove(this);
        Destroy(gameObject);
    }
    public static List<GraveObject> GetAllGraves()
    {
        return new List<GraveObject>(activeGraves);
    }
}
