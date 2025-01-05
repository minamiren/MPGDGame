using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Trunk : MonoBehaviour
{
    private Vector3 playerPos;
    public Transform trunkPos;
    public TMP_Text trunkText;
    public GameObject inventory;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }


    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        var distance = Vector3.Distance(playerPos, trunkPos.position);
        if (distance < 5)
        {
            if (trunkText.enabled != true)
            {
                trunkText.enabled = true;
            }
            if (Keyboard.current[Key.E].wasPressedThisFrame)
            {
                if (!inventory.activeSelf)
                {
                    // inventory.SetActive(true);
                    // InventoryManager.Instance.ListItems();
                    OpenInventory();
                }
                else
                {
                    //inventory.SetActive(false);
                    //InventoryManager.Instance.CleanContent();
                    CloseInventory();
                }
            }
        }
        else
        {
            if (trunkText.enabled == true)
            {
                trunkText.enabled = false;
            }
        }
    }
    // Open inventory and update game state
    private void OpenInventory()
    {
        inventory.SetActive(true); // Show the inventory UI
        InventoryManager.Instance.ListItems(); // Populate the inventory UI with items

        // Lock the cursor and make it visible when inventory is opened
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Make cursor visible

        // Disable player movement and other gameplay interactions (optional)
        //PlayerMovement.Instance.DisableMovement(); // Assuming PlayerMovement has a DisableMovement method
    }

    // Close inventory and update game state
    private void CloseInventory()
    {
        inventory.SetActive(false); // Hide the inventory UI
        InventoryManager.Instance.CleanContent(); // Clean up the inventory UI

        // Lock the cursor and hide it when inventory is closed
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor back to the center
        Cursor.visible = false; // Hide cursor

        // Re-enable player movement and other gameplay interactions (optional)
        //PlayerMovement.Instance.EnableMovement(); // Assuming PlayerMovement has an EnableMovement method
    }

}
