//Onclick 으로 받아준다.

//구매처리


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Button = UnityEngine.UI.Button;
using Unity.VisualScripting;
using TMPro;
using JetBrains.Annotations;


public class ItemSlot : MonoBehaviour
{
    //private ItemListLoader shoppingManager;
    //public PlayerManager playerManager;
    //public Button Merchandise;
    //public ItemList itemList;
    //public int slotindex;
    //public bool Purchase(int index)
    //{
    //    if (PlayerDataManager.Instance.player.gold > index.cost)
    //    {
    //        PlayerDataManager.Instance.UseGold(Cost);
    //        PlayerDataManager.Instance.AddTicket(amount);
    //    }
    //    return true;
    //}

    [SerializeField] private TMP_Text MerchandiseCost; // 아이템 가격
    [SerializeField] private Button BuyButton;        // 구매버튼
    public TMP_InputField InputAmount;
    private ItemList _ItemList;

    public void init(ItemList itemList)
    {
        MerchandiseCost.text = MerchandiseCost.text + "";
        _ItemList = itemList;

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(Purchase);

        
    }



    public void Purchase()
    {
        int amount = _ItemList.Cost;
        if (_ItemList.Cost < PlayerDataManager.Instance.player.gold)
        {
            if(_ItemList.ID == 101)
            {
                PlayerDataManager.Instance.UseGold(amount);
                int Buy = InputAmount.text.ToString().Length;
                PlayerDataManager.Instance.AddTicket(Buy);

            }

        }
       
         
     
    }

}
