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
                    inventory.SetActive(true);
                    InventoryManager.Instance.ListItems();
                }
                else
                {
                    inventory.SetActive(false);
                    InventoryManager.Instance.CleanContent();
                    //inventory.SetActive(false);
                    //InventoryManager.Instance.CleanContent();
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


}
