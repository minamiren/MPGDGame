using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

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
    private List<int> conversationIndex = new List<int>();
    private bool waitingForItem;
    private struct DialogueLine
    {
        public bool isQuestion;
        public string message;
        public string res1;
        public string res2;
        public string res3;
        public string res4;
        public int skip;
        public DialogueLine(bool isQuestion, string message, string res1, string res2, string res3, string res4, int skip)
        {
            this.isQuestion = isQuestion;
            this.message = message;
            this.res1 = res1;
            this.res2 = res2;
            this.res3 = res3;
            this.res4 = res4;
            this.skip = skip;
        }
    }
    private List<DialogueLine> dialogue = new List<DialogueLine>();

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
        waitingForItem = false;

        // This is so that we can talk to the NPC more than once. This control should be useful when it comes to needing to check for things like
        // Specific benchmarks to continue conversation, otherwise I would include it in the struct
        // this is actually very dangerous i should add this and a potential item to the struct
        //conversationIndex.Add(7);
        //conversationIndex.Add(8);
        //conversationIndex.Add(9);
        //conversationIndex.Add(11);

        // Add all dialogue
        dialogue.Add(new DialogueLine(false, "Could you bring me a food?", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "I have been waiting for you to wake.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "It seems you have hit your head.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "I am afraid that we are in a bit of a bind, if you don't remember.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(true, "Do you remember what happened to you?", "Yes", "No", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "That's a relief to hear. I will wait here until you can find us something useful.", "", "", "", "", 2));
        //dialogue.Add(new DialogueLine(false, "We are part of an exploratory party, but there was a cave-in and we were separated. It's likely our team thinks that we were lost to the falling rocks.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "We are on our own until we find something that we can use to help ourselves.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "Bring me food.", "", "", "", "", 0));
        //dialogue.Add(new DialogueLine(true, "Would you give me some food?", "Yes", "No", "", "", 0));
        //dialogue.Add(new DialogueLine(false, "Thank you. Now we may continue.", "", "", "", "", 1));
        //dialogue.Add(new DialogueLine(false, "Please bring me some food.", "", "", "", "", -3));
        //dialogue.Add(new DialogueLine(false, "", "", "", "", ""));
    }

    // Update is called once per frame
    void Update()
    {
        //foreach(var item in Inventory.Instance.GetHotBarList())
        //{
        //    Debug.Log(item.ToString());
        //}
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
                if (dialogueIndex == 8)
                {
                    //waitingForItem = true;
                    
                }
                // Dialogue not currently being shown
                if (dialogue.Count <= dialogueIndex)
                {
                   
                    // End of scripted conversation
                    //PlayerMovement.dialogue = false;
                    if(waitingForItem)
                    {
                        NextDialogueLine(dialogueIndex-1);
                    } else
                    {
                        DefaultDialogueLine();
                    }
                    // here we want to give a default line
                } else
                {
                    // Show dialogue box and start conversation
                    //template.SetActive(true);
                    NextDialogueLine(dialogueIndex);
                }
            } else
            {
                // Dialogue box already open
                if(conversationIndex.Contains(dialogueIndex) || dialogue.Count <= dialogueIndex)
                {
                    // We are at the end of a conversation. Close dialogue
                    conversationIndex.Remove(dialogueIndex);
                    template.SetActive(false);
                    PlayerMovement.dialogue = false;
                } else
                {
                    // Continue to next line in conversation
                    NextDialogueLine(dialogueIndex);
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
        if(waitingForItem && mostRecentResponse.Equals("0"))
        {
            string[] hotbar = Inventory.Instance.GetHotBarList();
            if (hotbar.Contains("Food"))
            {
                int index = System.Array.IndexOf(hotbar, "Food");
                Inventory.Instance.GiveHotbarItem(index);
                Inventory.Instance.AddItem(stick);
            }
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

        if (dialogueLine.isQuestion)
        {
            List<string> responseList = new List<string>();
            if(waitingForItem)
            {
                // if item is in hotbar then add yes
                // add no either way
            } else
            {
                responseList.Add(dialogueLine.res1);
                responseList.Add(dialogueLine.res2);
                responseList.Add(dialogueLine.res3);
                responseList.Add(dialogueLine.res4);
            }
            NewPlayerResponse(responseList);
        }
    }

    void DefaultDialogueLine()
    {
        template.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Why don't you try exploring some? We need food to survive.";
    }
}
