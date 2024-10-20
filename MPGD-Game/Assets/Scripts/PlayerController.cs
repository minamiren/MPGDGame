using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 moveValue;
    public float speed;
    private float scalarSpeed;

    public Inventory inventory;
    public TextMeshProUGUI hotBarFulledText;

    void Start()
    {
        hotBarFulledText.text = "";
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            int availableSlot = inventory.FindFirstAvailableSlot();
            Debug.Log("Available Slot: " + availableSlot);
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