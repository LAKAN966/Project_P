using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource audioSource;
    public AudioClip clip;

    
    void Start()
    {
        audioSource.Play();
        audioSource.clip = this.clip;

        audioSource = GetComponent<AudioSource>();
    }
}
