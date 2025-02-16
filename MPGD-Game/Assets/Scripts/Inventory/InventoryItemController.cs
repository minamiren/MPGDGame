using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public Button removeButton;
    public GameObject trunk;

    public Button backtoHotbarButton;

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void MoveBackToHotBar()
    {
        InventoryManager.Instance.BackToHotbar(item);
        Destroy(gameObject);
    }
}
