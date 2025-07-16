using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioMixer MainMixer;
    public Slider BGMslider;
    public Slider SFXslider;

    public void SoundController()
    {
        float BGM = BGMslider.value;
        float SFX = SFXslider.value;

        if (BGM == -40f)// 배경음악
        {
            MainMixer.SetFloat("BGM", -80);
        }
        else
        {
            MainMixer.SetFloat("BGM", BGM);
        }


        if (SFX == -40f)//효과음
        {
            MainMixer.SetFloat("SFX", -80);
        }
        else
        {
            MainMixer.SetFloat("SFX", BGM);
        }
    }
}
