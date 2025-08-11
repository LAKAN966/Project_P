using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // 1초 안에 비활성화

    void OnEnable()
    {
        CancelInvoke(nameof(DisableMe));
        Invoke(nameof(DisableMe), delay);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(DisableMe));
    }

    private void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
