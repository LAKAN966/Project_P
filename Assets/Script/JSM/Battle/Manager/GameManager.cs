using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public GameObject winUI;
    public GameObject loseUI;

    public void OnBaseDestroyed(bool isEnemyBase)
    {
        if (isEnemyBase)
        {
            winUI?.SetActive(true);
        }
        else
        {
            loseUI?.SetActive(true);
        }

        UnitSpawner spawner = FindObjectOfType<UnitSpawner>();
        if (spawner != null)
            spawner.enabled = false;
    }
}