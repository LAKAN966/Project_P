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
        SpawnSlot();
    }

    public void SpawnSlot()
    {
        Dictionary<int, Item> items = ItemListLoader.Instance.GetAllList();

        Debug.Log(items.Count + "총아이템의 개수");

        foreach(var item in items)
        {
            GameObject go = Instantiate(Merchandise, Content);
            ItemSlot slot = go.AddComponent<ItemSlot>();
            slot.init(item.Value);
        }
    }
}
