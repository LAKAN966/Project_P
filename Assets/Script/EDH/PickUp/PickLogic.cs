using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PickLogic : MonoBehaviour
{
    private PickUp _PickUp;
    public SlotSpawner slotSpawner;
    public void GottchaLogic()
    {
      
        Dictionary<int, PickInfo> _PickInfo = PickUpListLoader.Instance.GetAllPickList();

        if (PlayerDataManager.Instance.UseTicket(1) == true)
        {
            float v = Random.value;
         
            int index = Random.Range(0, _PickInfo.Count);
            PickInfo randomPick = _PickInfo.ElementAt(index).Value;

            if (v < 0.1f)
                randomPick.IsHero = true;
            else
                randomPick.IsHero = false;

            Debug.Log($"뽑기 결과: {(randomPick.IsHero ? "영웅!" : "일반")}");

        }

        if (PlayerDataManager.Instance.UseTicket(10) == true)
        {
            for (int i = 0; i < 10; i++)
            {
                float v = Random.value;

        
                int index = Random.Range(0, _PickInfo.Count);
                PickInfo randomPick = _PickInfo.ElementAt(index).Value;

                if (v < 0.1f)
                    randomPick.IsHero = true;
                else
                    randomPick.IsHero = false;

                Debug.Log($"뽑기 결과: {(randomPick.IsHero ? "영웅!" : "일반")}");

            }
        }
    }
}

