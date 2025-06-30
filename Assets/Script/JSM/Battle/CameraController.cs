using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float dragSpeed = 0.01f;

    public float zoomSpeed = 2f;
    public float pivotOffset = 0.8f; // 피벗 보정 (0 = 바닥, 1 = 위)

    private float minSize;     // 줌 인 한계
    private float maxSize;     // 줌 아웃 한계

    private Vector3 lastMousePos;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        minSize = cam.orthographicSize;

        float stageWidth = maxX - minX;

        // 최대 줌 아웃 크기 = 카메라가 좌/우 기지의 앞부분만 보이게 하기 위한 값
        maxSize = Mathf.Max(minSize, (stageWidth * 0.5f) / cam.aspect);
    }

    void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        float oldSize = cam.orthographicSize;
        float newSize = Mathf.Clamp(oldSize - scroll * zoomSpeed, minSize, maxSize);
        if (Mathf.Approximately(oldSize, newSize)) return;

        float sizeDelta = newSize - oldSize;

        // 지면 기준 위로 줌 피벗 조정
        cam.transform.position += new Vector3(0, sizeDelta * (1f - pivotOffset), 0);
        cam.orthographicSize = newSize;

        ClampCameraPosition();
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float moveX = -delta.x * dragSpeed;

            cam.transform.position += new Vector3(moveX, 0, 0);
            lastMousePos = Input.mousePosition;

            ClampCameraPosition();
        }
    }

    void ClampCameraPosition()
    {
        float halfView = cam.orthographicSize * cam.aspect;

        // 좌/우 카메라 이동 제한: 뒷부분이 보이지 않게 조절
        float leftLimit = minX + halfView;
        float rightLimit = maxX - halfView;

        float clampedX = Mathf.Clamp(cam.transform.position.x, leftLimit, rightLimit);
        cam.transform.position = new Vector3(clampedX, cam.transform.position.y, cam.transform.position.z);
    }
}
