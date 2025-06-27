using UnityEngine;

public class GospelContainerUI : MonoBehaviour
{
    public CanvasGroup canvasGroup; // 어두운 효과용

    public void SetInteractable(bool interactable)
    {
        canvasGroup.alpha = interactable ? 1f : 0.5f;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
}
