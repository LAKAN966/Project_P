using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Merchandise;
    [SerializeField] private Transform Content;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnSlot()
    {
        Dictionary<int, ItemList> items = ItemListLoader.Instance.GetAllList();

        foreach(var item in items)
        {
            GameObject go = Instantiate(Merchandise, Content);
            ItemSlot slot = go.AddComponent<ItemSlot>();
            slot.init(item.Value);
        }
    }
}
