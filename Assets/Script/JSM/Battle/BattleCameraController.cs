using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BattleCameraController : CameraController
{
    public float minX;
    public float maxX;
    public float dragSpeed = 0.01f;

    public float zoomSpeed = 2f;
    public float pivotOffset = 0.8f; // 피벗 보정 (0 = 바닥, 1 = 위)

    private float minSize;     // 줌 인 한계
    private float maxSize;     // 줌 아웃 한계

    private Vector3 lastMousePos;
    private Vector3 initialCamPosition;

    private Vector3 savedCamPosition;
    private float savedCamSize;
    private bool hasSavedCamera = false;


    void Start()
    {
        initialCamPosition = cam.transform.position;
        minSize = cam.orthographicSize*0.7f;

        float stageWidth = maxX - minX;

        maxSize = Mathf.Min(5.7f, (stageWidth * 0.5f) / cam.aspect);

        setCamLocation();
    }

    void Update()
    {
        if ((this as CameraController).IsFocusing) return;
        if (TutorialManager.Instance.isPlaying) return;
        HandleZoom();
        HandleDrag();
    }
    public void setCamLocation()
    {
        if (WaveManager.Instance.stageType == 2)
        {
            //cam.transform.position += new Vector3(WaveManager.Instance.currentStage.BaseDistance / 2, 0, 0);
            Destroy(this);
            return;
        }
    }

    public void HandleZoom()
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

    public void HandleDrag()
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
    public void FocusLeftMax()
    {
        SaveCameraState();
        cam.orthographicSize = minSize;
        ClampCameraPosition();

        float halfView = cam.orthographicSize * cam.aspect;
        float leftLimit = minX + halfView;

        cam.transform.position = new Vector3(leftLimit, initialCamPosition.y, initialCamPosition.z);
    }

    public void FocusRightMax()
    {
        cam.orthographicSize = minSize;
        ClampCameraPosition();

        float halfView = cam.orthographicSize * cam.aspect;
        float rightLimit = maxX - halfView;

        cam.transform.position = new Vector3(rightLimit, initialCamPosition.y, initialCamPosition.z);
    }
    public void SaveCameraState()
    {
        savedCamPosition = cam.transform.position;
        savedCamSize = cam.orthographicSize;
        hasSavedCamera = true;
    }
    public void RestoreCameraState()
    {
        if (!hasSavedCamera)
        {
            Debug.LogWarning("카메라 상태가 저장되지 않았습니다.");
            return;
        }

        cam.transform.position = savedCamPosition;
        cam.orthographicSize = savedCamSize;

        ClampCameraPosition(); // 혹시 범위를 넘어갈 경우 보정
    }


}
