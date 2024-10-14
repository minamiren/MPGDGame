using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public Button removeButton;

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
        Debug.Log(item.value);
        // TODO: do something with this item value
        // probably call Player.AddHealth(item.value) or Player.ReduceHunger(item.value) depending on item value
        InventoryManager.Instance.Remove(item);

        Destroy(gameObject);
    }

    private void OnSelect()
    {
        Debug.Log("selected item");
    }
}
