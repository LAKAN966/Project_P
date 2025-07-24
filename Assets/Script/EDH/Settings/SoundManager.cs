using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("메인, 스테이지")]
    public AudioSource bgmSource1;  //  메인로비, 스테이지 선택
    [Header("SFX")]
    public AudioSource bgmSource2;  //  SFX

    [Header("메인, 스테이지")]
    public AudioClip clip1; // 메인 로비 BGM
    [Header("SFX")]
    public AudioClip clip2; // SFX
    [Header("모집")]
    public AudioClip clip3; // 모집
    [Header("유닛 관리")]
    public AudioClip clip4; // 유닛관리
    [Header("전초기지")]
    public AudioClip clip5; // 전초기지
    [Header("상점")]
    public AudioClip clip6; // 상점

    void Start()
    {
        MainSound();
    }
    public void PlaySound(AudioClip clip)
    {
        bgmSource1.Stop();
        bgmSource1.clip = clip;
        bgmSource1.loop = true;
        bgmSource1.Play();
    }
    public void StopSound()
    {
        bgmSource1.Stop();
        MainSound();
    }
    public void MainSound()
    {
        PlaySound(clip1);
    }
    public void SoundEffect(AudioClip clip)
    {
        bgmSource2.Stop();
        bgmSource2.clip = clip;
        bgmSource2.loop = true;
        bgmSource2.Play();
    }
    public void SfXSound()
    {
        SoundEffect(clip2);
    }
    public void GottchaSound()
    {
        PlaySound(clip3);
    }
    public void DeckTabSound()
    {
        PlaySound(clip4);
    }
    public void HQSound()
    {
        PlaySound(clip5);
    }
    public void ShopSound()
    {
        PlaySound(clip6);
    }
}
