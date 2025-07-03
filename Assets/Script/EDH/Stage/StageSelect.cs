using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public Button MainChapterBtn;
    public Button GoldDungeonBtn;

    public GameObject MainStage;
    public GameObject GoldDunGeon;

    
    public void SetDungeonButton()
    {
        MainChapterBtn.onClick.AddListener(() => OnlyMain());
        GoldDungeonBtn.onClick.AddListener(() => OnlyGold());
    }

    public void OnlyMain()
    {
        GoldDunGeon.SetActive( false );
        MainStage.SetActive(true);
    }
    public void OnlyGold()
    {
        MainStage.SetActive( false );
        GoldDunGeon.SetActive(true);
    }
}
