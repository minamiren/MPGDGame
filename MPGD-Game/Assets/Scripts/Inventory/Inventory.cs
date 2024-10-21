using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour
{
    public GameObject[] PickUps;
    public List<Button> hotbarButtons;
    public ItemController itemController;
    public PlayerStates playerHungry;
    public bool[] hotbarSlotOccupied;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI hotBarFulledText;
    public int currentHotbarCount = 0;
    public int PlayerFillBelly = 10;

    void Start()
    {
        hotbarSlotOccupied = new bool[hotbarButtons.Count];
        ResetHotbarSlots();
        PickUps = new GameObject[6];
    }

    void Update()
    {
        if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
        {
            UseHotbarItem(0);
        }
        else if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
        {
            UseHotbarItem(1);
        }
        else if (Keyboard.current[Key.Digit3].wasPressedThisFrame)
        {
            UseHotbarItem(2);
        }
        else if (Keyboard.current[Key.Digit4].wasPressedThisFrame)
        {
            UseHotbarItem(3);
        }
        else if (Keyboard.current[Key.Digit5].wasPressedThisFrame)
        {
            UseHotbarItem(4);
        }
        else if (Keyboard.current[Key.Digit6].wasPressedThisFrame)
        {
            UseHotbarItem(5);
        }
    }


    public void AddItem(GameObject pickup)
    {        
        int availableSlot = FindFirstAvailableSlot();

        // 確保存在空的 hotbar 槽位
        if (availableSlot < hotbarButtons.Count && currentHotbarCount < 6)
        {
            // 添加物品至 PickUps 列表
            ItemController itemController = pickup.GetComponent<ItemController>();
            for(int i=0; i < PickUps.Length; i++)
            {
                if(PickUps[i] == null)
                {
                    PickUps[i] = pickup;
                    break;
                }
            }
            // PickUps.Add(pickup);
            UpdateHotBar(pickup, availableSlot); // add the object to hotbar and update to show
            currentHotbarCount++; // the number of object hotbar holding++
        }
        else
        {
            hotBarFulledText.text = "HotBar is full!";
            StartCoroutine(ClearHotBarFullText(1.5f));
        }
    }

    private IEnumerator ClearHotBarFullText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hotBarFulledText.text = ""; // 清空提示文字
    }

    private void UpdateHotBar(GameObject pickup, int slotIndex)
    {
        // 獲取當前空槽的按鈕
        Button currentButton = hotbarButtons[slotIndex];
        
        TextMeshProUGUI itemText = currentButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>(); 
        itemText.text = pickup.name;

        Image icon = currentButton.transform.GetChild(0).GetComponent<Image>();
        if (icon != null && pickup.GetComponent<SpriteRenderer>() != null)
        {
            icon.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
            icon.enabled = true;
        }

        hotbarSlotOccupied[slotIndex] = true; // 標記槽位為已佔用
        currentButton.onClick.RemoveAllListeners();
        currentButton.onClick.AddListener(() => MoveToInventory(currentButton)); ;
    }

    private void MoveToInventory( Button hotbarButton)
    {
        int index = hotbarButtons.IndexOf(hotbarButton);
        if (PickUps[index] != null)
        {
            GameObject pickup = PickUps[index]; // 獲取該物品的引用
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
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
                Debug.Log("Removing item from hotbar slot: " + slotIndex + ", Item: " + pickup.name);
                ClearHotBarSlot(hotbarButtons[slotIndex]);
                PickUps[slotIndex] = null;
                currentHotbarCount--;
            }
        }
    }

    private void ClearHotBarSlot(Button hotbarButton)
    {
        TextMeshProUGUI itemText = hotbarButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
        itemText.text = " ";

        /* 清空图标
        Image icon = hotbarButton.transform.GetChild(0).GetComponent<Image>();
        icon.sprite = null;  // 移除图标
        icon.enabled = false; // 隐藏图标
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
        for (int i = 0; i < hotbarSlotOccupied.Length; i++)
        {
            if (!hotbarSlotOccupied[i])
            {
                return i; // 返回第一個空槽位的索引
            }
        }
        return hotbarSlotOccupied.Length; // 如果沒有可用的槽位，返回槽位總數
    }

    private void ResetHotbarSlots()
    {
        for (int i = 0; i < hotbarSlotOccupied.Length; i++)
        {
            hotbarSlotOccupied[i] = false; // 設置為未佔用
        }
    }

}