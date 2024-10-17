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
    public GameObject itemMenu;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }


    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        var distance = Vector3.Distance(playerPos, trunkPos.position);
        if(distance < 5)
        {
            if(trunkText.enabled != true)
            {
                trunkText.enabled = true;
            }
            if(Keyboard.current[Key.E].wasPressedThisFrame)
            {
                if(!inventory.activeSelf)
                {
                    inventory.SetActive(true);
                    itemMenu.SetActive(true);
                    InventoryManager.Instance.ListItems();
                } else
                {
                    inventory.SetActive(false);
                    itemMenu.SetActive(false);
                }
            } 
        } else
        {
            if (trunkText.enabled == true)
            {
                trunkText.enabled = false;
            }
        }
    }

    public void PopulateItemMenu(Item item)
    {
        Debug.Log("Call populate item menu  "+item);
    }


}
