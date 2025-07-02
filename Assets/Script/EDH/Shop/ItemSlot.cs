//Onclick 으로 받아준다.

//구매처리


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Button = UnityEngine.UI.Button;
using Unity.VisualScripting;


public class ItemSlot : MonoBehaviour
{
    private ItemListLoader shoppingManager;
    public PlayerManager playerManager;

    public Button Merchandise;
    public ItemList itemList;


    public int slotindex;
   

    public void start()
    {
        Merchandise.onClick.AddListener(() => Purchase());
    }

    
    
    public bool Purchase(int index)
    {
        if (PlayerDataManager.Instance.player.gold > index.cost)
        {
            PlayerDataManager.Instance.UseGold(Cost);
            PlayerDataManager.Instance.AddTicket(amount);
        }
        return true;
    }

}
