using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotEnough : MonoBehaviour
{
    public TextMeshPro NotEnoughBoxText;
    public GameObject  NotEnoughBox;

    private PlayerDataManager playerDataManager;
    private Player player;

   public Item item;
    public void MoneyCheck()
    {
        if(player.gold < item.Cost )
        {
            NotEnoughBoxText.text = "골드가 부족합니다.";
            NotEnoughBox.SetActive(true);
            Time.time
        }
    }
}
