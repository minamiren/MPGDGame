using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NextDialogue : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction interact;
    private int index = 4;
    private int dialogueIndexStart = 4;

    private void Awake()
    {
        // Set up input system for use
        GameObject player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];
    }

    // Update is called once per frame
    void Update()
    {
        if (interact.triggered && transform.childCount > index)
        {
            if(PlayerMovement.dialogue)
            {
                index += 1;
                if (index < transform.childCount)
                {
                    transform.GetChild(index).gameObject.SetActive(true);
                }
                if(transform.childCount == index)
                {
                    index = dialogueIndexStart;
                    PlayerMovement.dialogue = false;
                    for (int i = transform.childCount - 1; i >= dialogueIndexStart; i--)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                }
            } 
        } 
    }
}
