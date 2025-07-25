using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public Image image;

    private Coroutine blinkCoroutine;

    private void OnEnable()
    {
        Invoke(nameof(DisablePanel), 5f);

        blinkCoroutine = StartCoroutine(BlinkColor());
    }

    private void OnDisable()
    {
        // 꺼지면 코루틴 정지
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator BlinkColor()
    {
        bool isRed = false;

        while (true)
        {
            image.color = isRed ? Color.yellow : Color.magenta;
            isRed = !isRed;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
