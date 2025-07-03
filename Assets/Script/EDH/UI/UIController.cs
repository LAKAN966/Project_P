using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject Main;         //메인
    public GameObject Stage;        //스테이지
    public GameObject DeckBuild;    //덱빌드
    public GameObject Shop;         //상점
    public GameObject HQ;           //내실
    public GameObject Gotta;        //뽑기

    [SerializeField]
    private static UIController Instance = new();
    public static UIController getInstance()
    {
        return Instance;
    }

    private void Start()
    {
        Instance = this;
        Main.SetActive(true);
    }
}