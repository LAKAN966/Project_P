using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PickLogic : MonoBehaviour
{
    private PickUp _PickUp;

    public void GottchaLogic()
    {
        float v = 0;
        v = Random.value;
        PickInfo pickInfo = new PickInfo();
        Dictionary<int, PickInfo> _PickInfo = PickUpListLoader.Instance.GetAllPickList();
        if (PlayerDataManager.Instance.UseTicket(1) == true)
        {
            bool Hero = pickInfo.IsHero;
            List<bool> IsHero =new List<bool>();
            IsHero.Add(pickInfo.IsHero);
            if (v < 0.10f)
            {
                TakeInfo.Equals(pickInfo.IsHero, Hero = true);
                
                SlotSpawner slotSpawner = new SlotSpawner();
                //slotSpawner.SpawnSlot(IsHero.Count) = TakeInfo(slotSpawner);

                






            }
            else if (v > 0.10f)
            {
                pickInfo.IsHero = true;
            }
        }
        if (PlayerDataManager.Instance.UseTicket(10) == true)
        {
            if (v < 0.1f)
            {



            }
        }
    }
}
