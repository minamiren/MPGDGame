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
        if (inventory.currentIndex < 6)
        {
            if (other.gameObject.tag == "PickUp")
            {
                inventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
        else
        {
            hotBarFulledText.text = "HotBar is fulled!";
            StartCoroutine(ClearHotBarFullText(1.5f));
        }
    }
    private IEnumerator ClearHotBarFullText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hotBarFulledText.text = ""; // �M�Ŵ��ܤ�r
    }
}