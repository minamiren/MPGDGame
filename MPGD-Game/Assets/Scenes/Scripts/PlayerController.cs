using System.Collections;
using System . Collections . Generic ;
using UnityEngine ;
using UnityEngine . InputSystem ;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    public PlayerStates playerHungry;
    public TextMeshProUGUI hotBarFulledText;

    void Start()
    {
        hotBarFulledText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            int availableSlot = inventory.FindFirstAvailableSlot();
            if (availableSlot < inventory.hotbarButtons.Count && inventory.currentHotbarCount < 6)
            {
                // 如果有可用槽位，撿起物品並將其添加到 inventory 中
                inventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }
            else
            {
                hotBarFulledText.text = "HotBar is fulled!";
                StartCoroutine(ClearHotBarFullText(1.5f));
            }
        }

    }
    private IEnumerator ClearHotBarFullText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hotBarFulledText.text = ""; // 清空提示文字
    }
}