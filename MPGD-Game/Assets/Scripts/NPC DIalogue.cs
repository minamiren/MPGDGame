using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using System;

public class NPCDialogue : MonoBehaviour
{
    // Various controls to detect if player is in range and already speaking, etc
    private bool playerInRange;
    private bool startDialogue;
    private PlayerInput playerInput;
    private InputAction interact;
    private bool keyReleased;

    // The actual dialogue and response UI
    public GameObject template; 
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    // Keeps track of where we are in the conversation
    private string mostRecentResponse;
    private int dialogueIndex;
    private bool waitingForItem;
    private bool closeDialogue;

    // which item we are waiting on
    private int itemIndex;
    private struct DialogueLine
    {
        public bool isQuestion;
        public string message;
        public string res1;
        public string res2;
        public string res3;
        public string res4;
        public int skip;
        public bool closeDialogue;
        public bool awaitingItem;
        public DialogueLine(bool isQuestion, string message, string res1, string res2, string res3, string res4, int skip, bool closeDialogue, bool awaitingItem)
        {
            this.isQuestion = isQuestion;
            this.message = message;
            this.res1 = res1;
            this.res2 = res2;
            this.res3 = res3;
            this.res4 = res4;
            this.skip = skip;
            this.closeDialogue = closeDialogue;
            this.awaitingItem = awaitingItem;
        }
    }
    private List<DialogueLine> dialogue = new List<DialogueLine>();

    private struct AwaitedItems
    {
        public string[] waitingList;
        public string[] returnedList;
        public string defaultDialogue;
        public AwaitedItems(string[] waitingList, string[] returnedList, string defaultDialogue)
        {
            this.waitingList = waitingList;
            this.returnedList = returnedList;
            this.defaultDialogue = defaultDialogue;
        }
    }
    private List<AwaitedItems> itemExchange = new List<AwaitedItems>();

    // Items to give the player
    public GameObject stick;

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
        dialogueText.enabled = false;
        mostRecentResponse = "";
        dialogueIndex = 0;
        itemIndex = 0;
        waitingForItem = false;
        closeDialogue = false;

        // Add all dialogue
        dialogue.Add(new DialogueLine(false, "I have been waiting for you to wake.", "", "", "", "", 0, true, false));
        dialogue.Add(new DialogueLine(false, "It seems you have hit your head.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "I am afraid that we are in a bit of a bind, if you don't remember.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(true, "Do you remember what happened to you?", "Yes", "No", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "That's a relief to hear. I will wait here until you can find us something  useful.", "", "", "", "", 2, true, false));
        dialogue.Add(new DialogueLine(false, "We are part of an exploratory party, but there was a cave-in and we were separated. It's likely our team thinks that we were lost to the falling rocks.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "We are on our own until we find something that we can use to help ourselves.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "Why don't you try exploring some? We need food to survive.", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "Now that you have a stick, why don't you go fetch me more food?", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "Now that you have two sticks, go win.", "", "", "", "", 0, true, false));
        //dialogue.Add(new DialogueLine(false, "", "", "", "", "", 0, false, false));

        itemExchange.Add(new AwaitedItems(new string[] {"Food"}, new string[] {"Stick"}, "If you bring me food, I might be able to do something with it."));
        itemExchange.Add(new AwaitedItems(new string[] {"Food"}, new string[] {"Stick"}, "If you bring me food, can transmute it into a secret second stick."));
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyReleased && interact.ReadValue<float>() == 0)
        {
            keyReleased = true;
        }

        // determines if we enter conversation
        if (playerInRange == true && interact.ReadValue<float>() == 1 && startDialogue == false && keyReleased && !dialogueBox.activeSelf)
        {
            PlayerMovement.dialogue = true;
            if (!template.activeSelf)
            {
                template.SetActive(true);
                // Dialogue not currently being shown
                if (dialogue.Count <= dialogueIndex)
                {
                    string defaultLine = "Use the items I've given you near the lake next to the mountains in order to escape.";
                    if (waitingForItem)
                    {
                        bool hasItems = HandleItemCheck(itemIndex);
                        Debug.Log(hasItems);
                        Debug.Log(itemIndex);
                        if (!hasItems)
                        {
                            defaultLine = itemExchange[itemIndex].defaultDialogue;
                        } else
                        {
                            defaultLine = "Thank you. I've given you something in return.";
                        }
                    } 
                    // End of scripted conversation
                    DefaultDialogueLine(defaultLine);
                } else
                {
                    if (waitingForItem)
                    {
                        bool hasItems = HandleItemCheck(itemIndex);
                        Debug.Log("not at end of dialogue");
                        Debug.Log(hasItems);
                        Debug.Log(itemIndex);
                        if (!hasItems)
                        {
                            DefaultDialogueLine(itemExchange[itemIndex].defaultDialogue);
                        }
                        else
                        {
                            NextDialogueLine(dialogueIndex);
                        }
                    }
                    else
                    {
                        NextDialogueLine(dialogueIndex);
                    }
                }
            } else
            {
                // Dialogue box already open
                if(closeDialogue || dialogue.Count <= dialogueIndex)
                {
                    // We are at the end of a conversation. Close dialogue
                    template.SetActive(false);
                    PlayerMovement.dialogue = false;
                    closeDialogue = false;
                } else
                {
                    if (waitingForItem)
                    {
                        bool hasItems = HandleItemCheck(itemIndex);
                        if (!hasItems)
                        {
                            template.SetActive(false);
                            PlayerMovement.dialogue = false;
                            closeDialogue = false;
                        }
                        else
                        {
                            NextDialogueLine(dialogueIndex);
                        }
                    }
                    else
                    {
                        NextDialogueLine(dialogueIndex);
                    }
                }
            }
            
            keyReleased = false;
        }
    }

    // Show 'press E to talk' if in range of player
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
            dialogueText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            dialogueText.enabled = false;
        }
    }

    // Set all potential responses for the player to the question
    void NewPlayerResponse(List<string> responseOptions)
    {
        for (int i = 0; i < responseOptions.Count; i++)
        {
            if (!responseOptions[i].Equals(""))
            {
                GameObject button = dialogueBox.transform.GetChild(i).gameObject;
                button.SetActive(true);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = responseOptions[i];
            }
        }
        dialogueBox.SetActive(true);
    }

    // Get the response that the player selected
    public void GetPlayerResponse(string response)
    {
        mostRecentResponse = response;
        PlayerMovement.dialogue = false;
        dialogueIndex += System.Convert.ToInt32(mostRecentResponse);
        if (dialogue.Count > dialogueIndex)
        {
            NextDialogueLine(dialogueIndex);
        } else
        {
            template.SetActive(false);
        }
    }

    // Show the current dialogue line
    void NextDialogueLine(int index)
    {
        DialogueLine dialogueLine = dialogue[index];
        template.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogueLine.message;
        if(!waitingForItem)
        {
            dialogueIndex+=(dialogueLine.skip+1);
        }

        closeDialogue = dialogueLine.closeDialogue;

        if (dialogueLine.isQuestion)
        {
            List<string> responseList = new List<string>();
            if(!waitingForItem) 
            {
                responseList.Add(dialogueLine.res1);
                responseList.Add(dialogueLine.res2);
                responseList.Add(dialogueLine.res3);
                responseList.Add(dialogueLine.res4);
            }
            NewPlayerResponse(responseList);
        }

        waitingForItem = dialogueLine.awaitingItem;
    }

    // Shows whatever line is passed as a quote
    void DefaultDialogueLine(string line)
    {
        template.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = line;
    }

    // Checks to see if items are had. Returns true if switch made successfully, and false if there are items still missing
    bool HandleItemCheck(int idx)
    {
        AwaitedItems items = itemExchange[idx];
        string[] hotbar = Inventory.Instance.GetHotBarList();
        Debug.Log(hotbar);
        bool broughtAllItems = true;
        foreach(string food in hotbar)
        {
            Debug.Log(food);
        }
        bool hasItem = false;
        foreach (string item in items.waitingList)
        {
            // if hotbar does not contain tiem, broughtAllItems is false
            // if hotbar at any point contains item, becomes true
            // have to do comparison this way to handle clones
            foreach (string barItem in hotbar)
            {
                if(barItem.Contains(item))
                {
                    hasItem = true;
                }
            }
            broughtAllItems = hasItem;
        }
        if(broughtAllItems)
        {
            foreach (string item in items.waitingList)
            {
                //int index = System.Array.IndexOf(hotbar, item);
                for(int i = 0; i < hotbar.Length; i++)
                {
                    if (hotbar[i].Contains(item))
                    {
                        Inventory.Instance.GiveHotbarItem(i);
                        break;
                    }
                }
            }
            foreach (string ret in items.returnedList) 
            {
                Inventory.Instance.AddItem(stick);
            }
            if(itemIndex == 1)
            {
                WinGame.Instance.SetWinCondition(true);
            }
            waitingForItem = false;
            itemIndex += 1;
        }
        return broughtAllItems;
    }
}
