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
        public string defaultDialogue;
        public AwaitedItems(string[] waitingList, string defaultDialogue)
        {
            this.waitingList = waitingList;
            this.defaultDialogue = defaultDialogue;
        }
    }
    private List<AwaitedItems> itemExchange = new List<AwaitedItems>();

    // Items to give the player
    public GameObject stick;
    public GameObject axe;
    public GameObject rope;

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
        dialogue.Add(new DialogueLine(false, "It looks like you've found food. I won't take it from you; eat it instead if you are hungry.", "", "", "", "", 0, true, false));
        dialogue.Add(new DialogueLine(false, "However, there are very important things for you to look for.", "", "", "", "", 0, true, false));
        dialogue.Add(new DialogueLine(false, "In order to signal the party looking for us, we need to climb the mountain.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "Maybe with some tools, we can create some rope to do so.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "Some of the monsters around seem to be made of materials. Can you find me a stone and a stick?", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "With the stones and the stick, I will make you a axe.", "", "", "", "", 0, false, false));
        dialogue.Add(new DialogueLine(false, "It looks like some trees around here might be small enough to cut down. If you can manage it perhaps I can fashion you a rope to climb the mountain.", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "It appears as if the wood from one little tree isn't enough. Perhaps one more will be sufficient.", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "Thank you for all of the work you are doing. Your efforts are paying off. I don't think this is quite enough, so could you find me one last Little Tree?", "", "", "", "", 0, true, true));
        dialogue.Add(new DialogueLine(false, "You've done it!", "", "", "", "", 0, false, false));
        //dialogue.Add(new DialogueLine(false, "", "", "", "", "", 0, false, false));

        itemExchange.Add(new AwaitedItems(new string[] {"Food"}, "Explore a little bit and bring me some food if you can."));
        itemExchange.Add(new AwaitedItems(new string[] {"Stick", "Stone"}, "Fight some enemies to find me a stone and a stick. I can make you a tool with it."));
        itemExchange.Add(new AwaitedItems(new string[] {"Little Tree"}, "Go explore the trees for small ones and use your axe to cut them down. The properties that make them small also appear to make them sparkle slightly."));
        itemExchange.Add(new AwaitedItems(new string[] { "Little Tree" }, "Continue searching the trees for small trees to cut down."));
        itemExchange.Add(new AwaitedItems(new string[] { "Little Tree" }, "Only one more small tree and I will have enough to create a long enough rope for you."));
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
                    string defaultLine = "Use the rope I've given you near the lake next to the mountain in order to climb it and escape.";
                    if (waitingForItem)
                    {
                        bool hasItems = HandleItemCheck(itemIndex);
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
        bool broughtAllItems = true;
        bool hasItem = false;
        foreach (string item in items.waitingList)
        {
            // if hotbar does not contain item, broughtAllItems is false
            // if hotbar at any point contains item, becomes true
            // have to do comparison this way to handle clones
            foreach (string barItem in hotbar)
            {
                if(barItem.Contains(item))
                {
                    hasItem = true;
                }
            }
            // if false, we do not have an item and don't want to update to true
            if(broughtAllItems)
            {
                broughtAllItems = hasItem;
            }
            hasItem = false;
        }
        if(broughtAllItems)
        {
            // If we have all the items required, remove them to give them to the NPC
            foreach (string item in items.waitingList)
            {
                for(int i = 0; i < hotbar.Length; i++)
                {
                    if (hotbar[i].Contains(item))
                    {
                        Inventory.Instance.GiveHotbarItem(i);
                        break;
                    }
                }
            }
            // Make sure that we return the proper item depending on which stage of dialogue we are at
            switch(itemIndex)
            {
                case 0:
                    Inventory.Instance.AddItem(stick);
                    break;
                case 1:
                    Inventory.Instance.AddItem(axe);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    Inventory.Instance.AddItem(rope);
                    break;
                default:
                    Debug.Log("We have given all items back");
                    break;
            }

            // We received the last item and it is time to go win the game
            if(itemIndex == itemExchange.Count-1)
            {
                WinGame.Instance.SetWinCondition(true);
            }
            waitingForItem = false;
            itemIndex += 1;
        }
        return broughtAllItems;
    }
    public void RestartDialogue()
    {
        // Reset dialogue-related states
        startDialogue = false;
        mostRecentResponse = "";
        dialogueIndex = 0;
        itemIndex = 0;
        waitingForItem = false;
        closeDialogue = false;
        keyReleased = true;

        // Deactivate the dialogue UI
        template.SetActive(false);
        PlayerMovement.dialogue = false;

        // Optionally re-enable the dialogue text prompt if player is still in range
        if (playerInRange)
        {
            dialogueText.enabled = true;
        }

        Debug.Log("Dialogue has been restarted.");
    }
}
