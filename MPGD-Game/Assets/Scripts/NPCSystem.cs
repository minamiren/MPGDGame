using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    private bool playerInRange;
    private bool startDialogue;
    private PlayerInput playerInput;
    private InputAction interact;
    public GameObject template;
    public GameObject canvas;
    private int interactionCount = 0;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];
    }
    // Start is called before the first frame update
    void Start()
    {
        startDialogue = false;
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && interact.ReadValue<float>() == 1 && startDialogue == false && !PlayerMovement.dialogue)
        {
            //canvas.transform.GetChild(3).gameObject.SetActive(true);
            PlayerMovement.dialogue = true;
            startDialogue = true;
            SetDialoguePath();
            canvas.transform.GetChild(3).gameObject.SetActive(true);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void NewDialogue(string text)
    {
        GameObject templateClone = Instantiate(template, template.transform);
        templateClone.transform.SetParent(canvas.transform, false);
        templateClone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }

    void NewPlayerResponse(List<string> responseOptions)
    {
        for (int i = 0; i < responseOptions.Count; i++)
        {
            Debug.Log(responseOptions[i]); 
        }
    }

    void SetDialoguePath()
    {
        // for first interaction, npc just says some things
        // later we will check for triggered events
        // this should maybe be a switch or case or whatever c# uses
        if (interactionCount == 0)
        {
            NewDialogue("Hello!");
            NewDialogue("I will not be this friendly in the real game probably");
            NewDialogue("It sure would be cool if you could click through all this dialogue");
            startDialogue = false;
        }
        if (interactionCount == 1)
        {
            NewDialogue("Now, could you answer a question for me?");
            List<string> playerResponses = new List<string>();
            playerResponses.Add("Yes");
            playerResponses.Add("No");
            NewPlayerResponse(playerResponses);
        }
        interactionCount++;
    }
}
