using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ButtonBlink : MonoBehaviour
{

    public TextMeshProUGUI buttonText; public float blinkInterval = 0.5f; // 깜빡이는 간격

    private bool isBlinking = true;
    void Start()
    {
        StartCoroutine(BlinkText());
    }
    IEnumerator BlinkText()
    {
        while (true)
        {
            buttonText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            buttonText.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StopBlinking()
    {
        isBlinking = false;
        StopAllCoroutines();
        buttonText.color = new Color(1f, 1f, 1f, 1f); // 원래 색으로
    }
}
