using UnityEngine;

public class MapManager : MonoBehaviour
{
    public float mapLength;
    public Transform allyBasePrefab;
    public Transform enemyBasePrefab;
    public Transform mapRoot;
    public static MapManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void MapStart()
    {
        mapLength = WaveManager.Instance.currentStage.BaseDistance;
        InitMap();
        SetupCameraBounds();
    }

    private void InitMap()
    {
        float halfLength = mapLength / 2f;
        Instantiate(allyBasePrefab, new Vector3(-halfLength, -1.0f, 0), Quaternion.identity, mapRoot);
        Instantiate(enemyBasePrefab, new Vector3(halfLength, -1.0f, 0), Quaternion.identity, mapRoot);
    }

    private void SetupCameraBounds()
    {
        float halfLength = mapLength / 2f;
        float padding = -2f;
        CameraController cam = Camera.main.GetComponent<CameraController>();
        cam.minX = -halfLength + padding;
        cam.maxX = halfLength - padding;
    }
}
