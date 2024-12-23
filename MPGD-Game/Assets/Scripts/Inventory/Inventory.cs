using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public GameObject[] PickUps;
    public List<Button> hotbarButtons;

    public ItemController itemController;
    public PlayerStates playerHungry;
    public ObjectSpawn objectSpawn;

    public bool[] hotbarSlotOccupied;
    public int currentHotbarCount = 0;

    public TextMeshProUGUI itemText;
    public TextMeshProUGUI hotBarFulledText;
    
    public int PlayerFillBelly = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        hotbarSlotOccupied = new bool[hotbarButtons.Count];
        ResetHotbarSlots();
        PickUps = new GameObject[6];
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

    public string[] GetHotBarList()
    {
        string[] names = new string[PickUps.Length];
        for (int i = 0; i < PickUps.Length; i++)
        {
            if(PickUps[i] != null)
            {
                names[i] = PickUps[i].name;
            } else
            {
                names[i] = "";
            }
        }
        return names;
    }

    public void AddItem(GameObject pickup)
    {        
        int availableSlot = FindFirstAvailableSlot();

        // be sure there are available slot
        if (availableSlot < hotbarButtons.Count && currentHotbarCount < 6)
        {
            // to add gameobject to PickUp List
            ItemController itemController = pickup.GetComponent<ItemController>();
            bool itemAdded = false;

            for (int i=0; i < PickUps.Length; i++)
            {
                if(PickUps[i] == null)
                {
                    PickUps[i] = pickup;
                    itemAdded = true;
                    break;
                }
            }
            if (itemAdded && pickup.CompareTag("Food") && objectSpawn != null)
                {
                    objectSpawn.SpawnNewFood();
                }
            // PickUps.Add(pickup);
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
        if (PickUps[index] != null)
        {
            GameObject pickup = PickUps[index];
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
                // If the gameobject is valid add the item to the inventory
                Item item = itemController.item;
                //InventoryManager.Instance.CleanContent();
                InventoryManager.Instance.AddToInventory(item); 
                ClearHotBarSlot(hotbarButton);
                hotbarSlotOccupied[index] = false;
                currentHotbarCount--;
                PickUps[index] = null; // remove object from the hotbar
            }
        }
    }

    // The same as use, but for use when giving an item to the NPC. So there is no effect for the player
    // It just leaves the inventory
    public void GiveHotbarItem(int slotIndex)
    {
        if (slotIndex < hotbarSlotOccupied.Length && hotbarSlotOccupied[slotIndex])
        {
            GameObject pickup = PickUps[slotIndex];
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
                Item item = itemController.item;
                ClearHotBarSlot(hotbarButtons[slotIndex]);
                PickUps[slotIndex] = null;
                currentHotbarCount--;
            };
        }
    }

    private void UseHotbarItem(int slotIndex)
    {
        if (slotIndex < hotbarSlotOccupied.Length && hotbarSlotOccupied[slotIndex])
        {
            GameObject pickup = PickUps[slotIndex];
            if (pickup != null && pickup.CompareTag("Food"))
            {
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

                    // 清空該熱鍵欄槽位
                    ClearHotBarSlot(hotbarButtons[slotIndex]);
                    PickUps[slotIndex] = null;
                    currentHotbarCount--;
                }
            }
            else
            {
                Debug.Log($"Cannot use item in slot {slotIndex}. It is not tagged as 'Food'.");
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