using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderSlot : MonoBehaviour
{
    [SerializeField] GameObject leaderPannel;
    public void OnClickLeader()
    {
        leaderPannel.SetActive(true);
        UILeaderUnitPannel.instance.Init();
    }
}
