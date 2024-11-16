using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    private bool playerInRange;
    private bool startDialogue;
    private PlayerInput playerInput;
    private InputAction interact;
    public GameObject template;
    private int interactionCount = 0;
    public TMP_Text dialogueText;
    private bool keyReleased;
    public GameObject dialogueBox;
    private string mostRecentResponse;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];
    }
    // Start is called before the first frame update
    void Start()
    {
        keyReleased = true;
        startDialogue = false;
        playerInRange = false;
        mostRecentResponse = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyReleased && interact.ReadValue<float>() == 0)
        {
            keyReleased = true;
        }
        if (playerInRange)
        {
            dialogueText.enabled = true;
        }
        else
        {
            dialogueText.enabled = false;
        }
        if (playerInRange == true && interact.ReadValue<float>() == 1 && startDialogue == false && keyReleased && !dialogueBox.activeSelf)
        {
            PlayerMovement.dialogue = true;
            NextDialogueLine();
            //SetDialoguePath();
            //canvas.transform.GetChild(3).gameObject.SetActive(true);
            startDialogue = true;
            keyReleased = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void NewPlayerResponse(List<string> responseOptions)
    {
        dialogueBox.SetActive(true);
        for (int i = 0; i < responseOptions.Count; i++)
        {
            GameObject button = dialogueBox.transform.GetChild(i).gameObject;
            button.SetActive(true);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = responseOptions[i];
        }
    }

    public void GetPlayerResponse(GameObject response)
    {
        mostRecentResponse = response.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }

    void NextDialogueLine()
    {
        Debug.Log("setting active");
        template.SetActive(true);
        Debug.Log("finished setting active");
        template.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "I have been waiting for you to wake.";
        Debug.Log("set template text");
    }

        //void SetDialoguePath()
        //{
        //    // for first interaction, npc just says some things
        //    // later we will check for triggered events
        //    // this should maybe be a switch or case or whatever c# uses
        //    switch (interactionCount)
        //    {
        //        case 0:
        //            {
        //                NewDialogue("I have been waiting for you to wake.");
        //                NewDialogue("It seems you have hit your head.");
        //                NewDialogue("I am afraid that we are in a bit of a bind, if you don't remember.");
        //                startDialogue = false;
        //                break;
        //            }
        //        case 1:
        //            {
        //                NewDialogue("Do you remember what happened to you?");
        //                List<string> playerResponses = new List<string>();
        //                playerResponses.Add("Yes");
        //                playerResponses.Add("No");
        //                NewPlayerResponse(playerResponses);
        //                startDialogue = false;
        //                break;
        //            }
        //        case 2:
        //            {
        //                if (mostRecentResponse == "Yes")
        //                {
        //                    NewDialogue("That's a relief to hear. I will wait here until you can find us something useful.");
        //                }
        //                else
        //                {
        //                    NewDialogue("We are part of an exploratory party, but there was a cave-in and we were separated. It's likely our team thinks that we were lost to the falling rocks.");
        //                    NewDialogue("We are on our own until we find something that we can use to help ourselves.");
        //                }
        //                startDialogue = false;
        //                break;
        //            }
        //        default:
        //            NewDialogue("Thank you for speaking with me.");
        //            break;
        //    }
        //    interactionCount++;
        //}
    }
