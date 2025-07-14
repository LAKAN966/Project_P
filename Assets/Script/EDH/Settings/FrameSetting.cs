using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameSetting : MonoBehaviour
{
    public Button LowFPSbtn; // 30프레임
    public Button HighFPSbtn;// 60프레임
    // Start is called before the first frame update
    void Start()
    {
        LowFPSbtn.onClick.AddListener(SetLowFPS);
        HighFPSbtn.onClick.AddListener(SetHighFPS);
    }


    public void SetLowFPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Debug.Log(Application.targetFrameRate);
    }

    public void SetHighFPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Debug.Log(Application.targetFrameRate);
    }
}
