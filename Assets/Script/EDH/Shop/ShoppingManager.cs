using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    public GameObject Merchandise;
    public Transform Contenet;

    public ItemListLoader ItemListLoader;

    
    private void Start()
    {
        ItemListLoader.GetAllList();
        ItemSlot.Instantiate(Contenet);
    }
}


   
