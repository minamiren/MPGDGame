using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public List<GameObject> PickUps = new List<GameObject>(); // 存放撿到的物件
    public List<Button> hotbarButtons; // HotBar中的按鈕，最多6個
    public bool[] hotbarSlotOccupied; // if hotbar is avalible
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI hotBarFulledText;

    public int currentHotbarCount = 0; // 当前Hotbar中物品数量

    void Start()
    {
        hotbarSlotOccupied = new bool[hotbarButtons.Count];
        ResetHotbarSlots();
    }

    // 撿到物品後的處理
    public void AddItem(GameObject pickup)
    {        
        int availableSlot = FindFirstAvailableSlot();

        // 確保存在空的 hotbar 槽位
        if (availableSlot < hotbarButtons.Count && currentHotbarCount < 6)
        {
            // 添加物品至 PickUps 列表
            ItemController itemController = pickup.GetComponent<ItemController>();
            PickUps.Add(pickup);
            UpdateHotBar(pickup, availableSlot); // 将物品添加至 hotbar 并显示
            currentHotbarCount++; // 增加 Hotbar 中物品数量
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

    // 更新HotBar UI
    private void UpdateHotBar(GameObject pickup, int slotIndex)
    {
        // 獲取當前空槽的按鈕
        Button currentButton = hotbarButtons[slotIndex];
        TextMeshProUGUI itemText = currentButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
        itemText.text = pickup.name;

        Image icon = currentButton.transform.GetChild(0).GetComponent<Image>();
        if (icon != null && pickup.GetComponent<SpriteRenderer>() != null)
        {
            icon.sprite = pickup.GetComponent<SpriteRenderer>().sprite; // 假設物品有 sprite 圖示
            icon.enabled = true;
        }

        hotbarSlotOccupied[slotIndex] = true; // 標記槽位為已佔用
        currentButton.onClick.RemoveAllListeners();
        currentButton.onClick.AddListener(() => MoveToInventory(currentButton)); ;
    }

    private void MoveToInventory( Button hotbarButton)
    {
        int index = hotbarButtons.IndexOf(hotbarButton);
        if (index >= 0 && index < PickUps.Count)
        {
            GameObject pickup = PickUps[index]; // 獲取該物品的引用
            ItemController itemController = pickup.GetComponent<ItemController>();

            if (itemController != null)
            {
                Item item = itemController.item;
                InventoryManager.Instance.AddToInventory(item);
                ClearHotBarSlot(hotbarButton);
                hotbarSlotOccupied[index] = false;
                currentHotbarCount--; // 减少 Hotbar 中物品数量
                PickUps.RemoveAt(index);
            }
        }
    }


    private void ClearHotBarSlot(Button hotbarButton)
    {
        // 清空文字
        TextMeshProUGUI itemText = hotbarButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
        itemText.text = " "; // 将按键文字设为空白

        /* 清空图标
        Image icon = hotbarButton.transform.GetChild(0).GetComponent<Image>();
        icon.sprite = null;  // 移除图标
        icon.enabled = false; // 隐藏图标
        */
        // 移除所有点击事件
        hotbarButton.onClick.RemoveAllListeners();

        // 找到按钮索引并清除槽位状态
        int index = hotbarButtons.IndexOf(hotbarButton);
        if (index >= 0 && index < hotbarSlotOccupied.Length)
        {
            hotbarSlotOccupied[index] = false;  // 标记槽位为未占用
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