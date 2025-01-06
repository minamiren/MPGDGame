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
        if(other.gameObject.tag == "PickUp" && other.gameObject.name.Contains("Little"))
        {
            foreach (string barItem in Inventory.Instance.GetHotBarList())
            {
                if (barItem.Contains("Axe"))
                {
                    int availableSlot = inventory.FindFirstAvailableSlot();
                    if (availableSlot < inventory.hotbarButtons.Count && inventory.currentHotbarCount < 6)
                    {
                        inventory.AddItem(other.gameObject);
                        other.gameObject.SetActive(false);
                    }
                    else
                    {
                        hotBarFulledText.text = "HotBar is full!";
                        StartCoroutine(ClearHotBarFullText(1.5f));
                    }
                } else
                {
                    Debug.Log(Inventory.Instance.GetHotBarList());
                }
            }
        } else if (other.gameObject.tag == "PickUp"|| other.gameObject.tag == "Food")
        {
            int availableSlot = inventory.FindFirstAvailableSlot();
            if (availableSlot < inventory.hotbarButtons.Count && inventory.currentHotbarCount < 6)
            {
                inventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }
            else
            {
                hotBarFulledText.text = "HotBar is full!";
                StartCoroutine(ClearHotBarFullText(1.5f));
            }
        }

    }
    private IEnumerator ClearHotBarFullText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hotBarFulledText.text = "";
    }
}