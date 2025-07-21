using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public float timeLimit=60;
    public float m_Time;
    public bool m_Running=false;

    public void Start()
    {
        m_Running = true;
    }
    public void FixedUpdate()
    {
        if (!m_Running) return;
        m_Time += Time.deltaTime;
        float displayTime = Mathf.Floor(m_Time * 100f) / 100f;
        timer.text = $"{displayTime:00.00}";
        if (displayTime >= timeLimit)
        {
            BattleManager.Instance.OnBaseDestroyed(false);
        }
    }
}
