using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public Button removeButton;
    private bool selected;
    public GameObject trunk;

    void Start()
    {
        selected = false;

    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);

        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        // Debug.Log(item.value);
        // TODO: do something with this item value
        // probably call Player.AddHealth(item.value) or Player.ReduceHunger(item.value) depending on item value
        /*InventoryManager.Instance.Remove(item);
        
        Destroy(gameObject);*/
        trunk.GetComponent<Trunk>().PopulateItemMenu(item);

    }
}
