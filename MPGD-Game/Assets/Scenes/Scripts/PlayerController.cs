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
                // �p�G���i�μѦ�A�߰_���~�ñN��K�[�� inventory ��
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
        hotBarFulledText.text = ""; // �M�Ŵ��ܤ�r
    }
}