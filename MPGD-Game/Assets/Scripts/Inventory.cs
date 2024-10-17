using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public List<GameObject> PickUps = new List<GameObject>(); // 存放撿到的物件
    public List<Button> hotbarButtons; // HotBar中的按鈕，最多6個
    public int currentIndex = 0; // 追踪目前已填滿的格子數

    public TextMeshProUGUI itemText;

    // 撿到物品後的處理
    public void AddItem(GameObject pickup)
    {
        if (currentIndex < hotbarButtons.Count)
        {
            // 將撿到的物品加入列表
            PickUps.Add(pickup);

            // 更新HotBar顯示
            UpdateHotBar(pickup);
        }
    }

    // 更新HotBar UI
    private void UpdateHotBar(GameObject pickup)
    {
        // 獲取當前空槽的按鈕
        Button currentButton = hotbarButtons[currentIndex];
        TextMeshProUGUI itemText = currentButton.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();

        // 更新按鈕的文本為物品名稱
        itemText.text = pickup.name;

        // 如果有圖示，更新圖示
        Image icon = currentButton.transform.GetChild(0).GetComponent<Image>();
        if (icon != null && pickup.GetComponent<SpriteRenderer>() != null)
        {
            icon.sprite = pickup.GetComponent<SpriteRenderer>().sprite; // 假設物品有 sprite 圖示
            icon.enabled = true;
        }
        currentIndex++;
    }
}