using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX = -20f;
    public float maxX = 20f;
    public float dragSpeed = 0.01f;

    private Vector3 lastMousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float moveX = -delta.x * dragSpeed;

            Vector3 newPos = Camera.main.transform.position + new Vector3(moveX, 0, 0);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

            Camera.main.transform.position = newPos;
            lastMousePos = Input.mousePosition;
        }
    }
}
