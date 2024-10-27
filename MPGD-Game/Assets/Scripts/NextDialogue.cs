using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NextDialogue : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction interact;
    private int index = 4;

    private float timeElapsed;

    private void Awake()
    {
        // Set up input system for use
        GameObject player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        Debug.Log(playerInput);
        interact = playerInput.actions["Interact"];
    }

    void Start()
    {
        timeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        if(interact.triggered && transform.childCount > 3)
        {
            if(PlayerMovement.dialogue)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                Debug.Log("adding child with name " + transform.GetChild(index).gameObject.name);
                index += 1;
                if(transform.childCount == index)
                {
                    index = 4;
                    PlayerMovement.dialogue = false;
                }
            } else
            {
                transform.GetChild(3).gameObject.SetActive(false);
            }
        } 
    }
}