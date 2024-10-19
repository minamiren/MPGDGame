using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTFood : MonoBehaviour
{
    public int PlayerFillBelly = 10;
    public PlayerStates playerHungry;
    public GameObject food;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collides me FOOD!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHungry = other.GetComponent<PlayerStates>();

            if (playerHungry != null)
            {
                playerHungry.FillBelly(PlayerFillBelly);
                food.SetActive(false);
                Debug.Log("Player EAT, LOL");
            }
        }
    }
}
