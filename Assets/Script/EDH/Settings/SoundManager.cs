using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager :MonoBehaviour
{
    [Header("메인, 스테이지")]
    public AudioSource bgmSource1;  //  메인로비, 스테이지 선택
    [Header("SFX")]
    public AudioSource bgmSource2;  //  SFX
    [Header("모집")]
    public AudioSource bgmSource3;  //  모집
    [Header("Field")]
    public AudioSource bgmSource4;  //  유닛관리
    public AudioSource bgmSource5;  //  전초기지
    public AudioSource bgmSource6;  //  상점
    public AudioSource bgmSource7;  //  일반 전투
    public AudioSource bgmSource8;  //  보스 전투
    public AudioSource bgmSource9;  //  골드 던전 전투
    public AudioSource bgmSource10; //  미궁의 탑 전투


    public AudioClip clip1; // 메인 로비 BGM
    public AudioClip clip2; // SFX
    public AudioClip clip3; // 모집
    public AudioClip clip4; // 유닛관리
    public AudioClip clip5; // 전초기지
    public AudioClip clip6; // 상점
    public AudioClip clip7; // 일반 스테이지 
    public AudioClip clip8; // 보스 스테이지
    public AudioClip clip9; // 골드던전 전투
    public AudioClip clip10;// 미궁의탑 전투

    void Start()
    {
        // 첫 번째 음악 설정
        bgmSource1.clip = clip1;
        bgmSource1.loop = true;
        bgmSource1.Play();

        // 두 번째 음악 설정
        bgmSource2.clip = clip2;
        bgmSource2.loop = true;
        bgmSource2.Play();

    }
}
