using UnityEngine;

public class MapManager : MonoBehaviour
{
    public float mapLength = 50f;
    public Transform allyBasePrefab;
    public Transform enemyBasePrefab;
    public Transform mapRoot;
    public static MapManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        InitMap();
        SetupCameraBounds();
    }

    private void InitMap()
    {
        float halfLength = mapLength / 2f;
        Instantiate(allyBasePrefab, new Vector3(-halfLength, 0, 0), Quaternion.identity, mapRoot);
        Instantiate(enemyBasePrefab, new Vector3(halfLength, 0, 0), Quaternion.identity, mapRoot);
    }

    private void SetupCameraBounds()
    {
        float halfLength = mapLength / 2f;
        float padding = 2f;
        CameraController cam = Camera.main.GetComponent<CameraController>();
        cam.minX = -halfLength + padding;
        cam.maxX = halfLength - padding;
    }
}
