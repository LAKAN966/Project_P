using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoader
    : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.LeftShift))
        { UIManager.Instance.Open<MainUI>(); }

        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q))
        { UIManager.Instance.Open<MainUI>(); }
        //Shift + Q = MainUI

        else if (Input.GetKeyDown(KeyCode.W))
        { UIManager.Instance.Open<StageUI>(); }
        //W = StageUI

        else if (Input.GetKeyDown(KeyCode.E))
        { UIManager.Instance.Open<GottaUI>(); }
        //E = GottchaUI

        else if (Input.GetKeyDown(KeyCode.R))
        { UIManager.Instance.Open<ShopUI>(); }
        //R = ShopUI

        else if (Input.GetKeyDown(KeyCode.T))
        { UIManager.Instance.Open<HQUI>(); }
        //T = HQUI

        else if (Input.GetKeyDown(KeyCode.Y))
        { UIManager.Instance.Open<DeckBuildUI>(); }
        //Y = DeckBuildUI

        //전체 삭제 Delete
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            UIManager.Instance.Close<StageUI>();
            UIManager.Instance.Close<GottaUI>();
            UIManager.Instance.Close<ShopUI>();
            UIManager.Instance.Close<HQUI>();
            UIManager.Instance.Close<DeckBuildUI>();
        }


    }

}
