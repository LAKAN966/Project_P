using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    public GameObject Merchandise;
    public Transform Content;

    public ItemListLoader ItemListLoader;

    public static ShoppingManager Instance { get; private set; }
    void Start()
    {
        ItemListLoader.GetAllList();
        ItemSlot.Instantiate(Content);
        if (ItemListLoader.Instance == null)
        {
            Instantiate(ItemListLoader);
        }
    }
}


   
