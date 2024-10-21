using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HotBarController : MonoBehaviour
{
    public List<GameObject> PickUps = new List<GameObject>(); // 存放撿到的物件
    public List<Button> hotbarButtons; // HotBar中的按鈕，最多6個
    private int currentIndex = 0; // 追踪目前已填滿的格子數

    // 撿到物品後的處理
    public void AddItem(GameObject pickup)
    {
        foreach (Button btn in hotbarButtons)
        {
            TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText.text == "Empty Slot")  // 找到第一個空槽
            {
                tmpText.text = pickup.name;   // 顯示撿起物品的名稱
                break;
            }
        }
    }

    // 更新HotBar UI
    private void UpdateHotBar(GameObject pickup)
    {
        // 更新當前索引對應的 TMP Button 圖示和文本
        Button currentButton = hotbarButtons[currentIndex];
        Image icon = currentButton.transform.GetChild(0).GetComponent<Image>();
        
        icon.enabled = true;

        currentIndex++;
    }

}
