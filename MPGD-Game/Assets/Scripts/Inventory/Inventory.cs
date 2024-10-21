using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour
{
    public List<GameObject> PickUps = new List<GameObject>();
    public List<Button> hotbarButtons;

    public ItemController itemController;
    public PlayerStates playerHungry;

    public bool[] hotbarSlotOccupied;
    public int currentHotbarCount = 0;

    public TextMeshProUGUI itemText;
    public TextMeshProUGUI hotBarFulledText;
    
    public int PlayerFillBelly = 10;

    void Start()
    {
        hotbarSlotOccupied = new bool[hotbarButtons.Count];
        ResetHotbarSlots();

    }

    void Update()
    {
        // setting keypress
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            UseHotbarItem(0);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            UseHotbarItem(1);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            UseHotbarItem(2);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            UseHotbarItem(3);
        }
        else if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            UseHotbarItem(4);
        }
        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            UseHotbarItem(5);
        }
    }


    public void AddItem(GameObject pickup)
    {        
        int availableSlot = FindFirstAvailableSlot();

        // be sure there are available slot
        if (availableSlot < hotbarButtons.Count && currentHotbarCount < 6)
        {
            // to add gameobject to PickUp List
            ItemController itemController = pickup.GetComponent<ItemController>();
            PickUps.Add(pickup);
            UpdateHotBar(pickup, availableSlot); // add the object to hotbar and update to show
            currentHotbarCount++; // the number of object hotbar holding++
        }
        else
        {
            //Prompt the player that they can no longer pick up items
            hotBarFulledText.text = "HotBar is filled!";
            StartCoroutine(ClearHotBarFullText(1.5f));
        }
    }

    private IEnumerator ClearHotBarFullText(float waitTime)
    {
        // Timer for cleaning promt
        yield return new WaitForSeconds(waitTime);
        hotBarFulledText.text = "";
    }

    private void UpdateHotBar(GameObject pickup, int slotIndex)
    {
        // Getting the first available button
        Button currentButton = hotbarButtons[slotIndex];
        
        TextMeshProUGUI itemText = currentButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
        ItemController itemController = pickup.GetComponent<ItemController>();
        itemText.text = itemController.item.itemName; // Use itemName here


        // showing image(not yet)
        Image icon = currentButton.transform.GetChild(0).GetComponent<Image>();
        if (icon != null && pickup.GetComponent<SpriteRenderer>() != null)
        {
            icon.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
            icon.enabled = true;
        }

        hotbarSlotOccupied[slotIndex] = true; // set the current button used
        currentButton.onClick.RemoveAllListeners(); 
        currentButton.onClick.AddListener(() => MoveToInventory(currentButton)); // get item to inventory
    }

    private void MoveToInventory( Button hotbarButton)
    {
        // Find the index of the hotbar button that was clicked
        int index = hotbarButtons.IndexOf(hotbarButton);
        if (index >= 0 && index < PickUps.Count)
        {
            GameObject pickup = PickUps[index];
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
                // If the gameobject is valid add the item to the inventory
                Item item = itemController.item;
                InventoryManager.Instance.AddToInventory(item);
                Destroy(item);
                ClearHotBarSlot(hotbarButton);
                hotbarSlotOccupied[index] = false;
                currentHotbarCount--;
                PickUps.RemoveAt(index); // remove object from the hotbar
            }
        }
    }

    private void UseHotbarItem(int slotIndex)
    {
        if (slotIndex < hotbarSlotOccupied.Length && hotbarSlotOccupied[slotIndex])
        {
            GameObject pickup = PickUps[slotIndex];
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
                Item item = itemController.item;
                GameObject player = GameObject.FindWithTag("Player");
                playerHungry = player.GetComponent<PlayerStates>();

                if (playerHungry != null)
                {
                    playerHungry.FillBelly(PlayerFillBelly);
                }
                ClearHotBarSlot(hotbarButtons[slotIndex]);
                PickUps.RemoveAt(slotIndex);
                currentHotbarCount--;
            }
        }
    }

    private void ClearHotBarSlot(Button hotbarButton)
    {
        // remove item from hotbar
        TextMeshProUGUI itemText = hotbarButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
        itemText.text = " ";

        /* remove item image(not yet)
        Image icon = hotbarButton.transform.GetChild(0).GetComponent<Image>();
        icon.sprite = null;
        icon.enabled = false;
        */
        
        hotbarButton.onClick.RemoveAllListeners();

        int index = hotbarButtons.IndexOf(hotbarButton);
        if (index >= 0 && index < hotbarSlotOccupied.Length)
        {
            hotbarSlotOccupied[index] = false;
        }
    }


    public int FindFirstAvailableSlot()
    {
        // to check for available slots
        for (int i = 0; i < hotbarSlotOccupied.Length; i++)
        {
            if (!hotbarSlotOccupied[i])
            {
                return i; 
            }
        }
        return hotbarSlotOccupied.Length;
    }

    private void ResetHotbarSlots()
    {
        // set hotbar status to false
        for (int i = 0; i < hotbarSlotOccupied.Length; i++)
        {
            hotbarSlotOccupied[i] = false;
        }
    }

}