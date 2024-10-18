using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTEnemy : MonoBehaviour
{
    public int PlayerGetDamage = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collides me!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerStates playerhealth = other.GetComponent<PlayerStates>();

            if (playerhealth != null)
            {
                playerhealth.TakeDamage(PlayerGetDamage);
                Debug.Log("Player took damage, LOL");
            }
        }
    }
}
