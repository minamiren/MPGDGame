using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WinGame : MonoBehaviour
{
    public static WinGame Instance;
    private PlayerInput playerInput;
    private InputAction interact;

    public bool winCondition;
    public TMP_Text winText;
    public GameObject winScreen;

    private bool winListen;

    // bad way
    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GameObject player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];
    }

    void Start()
    {
        winCondition = false;
        winListen = false;
        winText.enabled = false;
    }

    void Update()
    {
        if(interact.ReadValue<float>() == 1 && winListen)
        {
            winScreen.SetActive(true);
        }
    }

    public void SetWinCondition(bool canWin)
    {
        Debug.Log("set condition");
        winCondition = canWin;
    }

    // Show 'press E to use win items' if in range of player
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && winCondition)
        {
            winListen = true;
            winText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            winListen = false;
            winText.enabled = false;
        }
    }

}
