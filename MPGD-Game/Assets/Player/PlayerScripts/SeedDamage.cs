using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDamage : MonoBehaviour
{
    private Transform player;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("SeedParticles"))
        {
            DealDamageToPlayer();
        }
    }

    private void DealDamageToPlayer()
    {
        PlayerStates playerHealth = player.GetComponent<PlayerStates>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // Adjust damage value as necessary
        }
    }

}
