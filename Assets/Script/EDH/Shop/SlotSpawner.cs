using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ItemSlot;
    [SerializeField] private Transform Content;
    
    void Start()
    {
        SpawnSlot();
    }

    public void SpawnSlot()
    {
       var items = ItemListLoader.Instance.GetAllList();

        Debug.Log(items.Count + "총아이템의 개수");

        foreach(var item in items)
        {
            GameObject go = Instantiate(ItemSlot, Content);
            ItemSlot slot = go.GetComponent<ItemSlot>();
            slot.init(item.Value);
        }
    }
}
