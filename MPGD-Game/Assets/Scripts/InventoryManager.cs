using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();

    public Transform itemContent;
    public GameObject inventoryItem;

    public Toggle enableRemove;

    public InventoryItemController[] inventoryItems;

    private void Awake()
    {
        Instance = this;
    }

    // Following logic may go in another location if this is not the best place for it
    public void Add(Item item)
    {
        // TODO: add check for item already exists. if so, increase count 
        items.Add(item);
    }

    public void Remove(Item item)
    {
        // TODO: reduce count of item. if 1, remove entirely
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in items)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("ExitButton").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if(enableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }
        }

        SetInventoryItems();
    }

    public void EnableItemsRemove()
    {
        if(enableRemove.isOn)
        {
            foreach(Transform item in itemContent)
            {
                item.Find("ExitButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach(Transform item in itemContent)
            {
                item.Find("ExitButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        inventoryItems = itemContent.GetComponentsInChildren<InventoryItemController>();

        for(int i = 0; i<items.Count; i++)
        {
            inventoryItems[i].AddItem(items[i]);
        }
    }
}