using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTFood : MonoBehaviour
{
    public int PlayerFillBelly = 10;

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
            PlayerStates playerHungry = other.GetComponent<PlayerStates>();

            if (playerHungry != null)
            {
                playerHungry.FillBelly(PlayerFillBelly);
                Debug.Log("Player EAT, LOL");
            }
        }
    }
}
