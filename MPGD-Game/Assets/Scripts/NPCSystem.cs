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
        if (playerInRange == true && interact.ReadValue<float>() == 1 && startDialogue == false  && !PlayerMovement.dialogue)
        {
            //canvas.transform.GetChild(3).gameObject.SetActive(true);
            PlayerMovement.dialogue = true;
            startDialogue = true;
            NewDialogue("Hello!");
            NewDialogue("I will not be this friendly in the real game probably");
            NewDialogue("It sure would be cool if you could click through all this dialogue");
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
        //templateClone.transform.parent = canvas.transform;
        templateClone.transform.SetParent(canvas.transform, false );
        templateClone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }
}
