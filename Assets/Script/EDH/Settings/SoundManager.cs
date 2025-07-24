using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager :MonoBehaviour
{
    public List<AudioSource> bgmSource;

    public List<AudioClip> clip; // 두 번째 음악

    void Start()
    {
        //// 첫 번째 음악 설정
        //bgmSource1.clip = clip1;
        //bgmSource1.loop = true;
        //bgmSource1.Play();

        //// 두 번째 음악 설정
        //bgmSource2.clip = clip2;
        //bgmSource2.loop = true;
        //bgmSource2.Play();

    }
}
