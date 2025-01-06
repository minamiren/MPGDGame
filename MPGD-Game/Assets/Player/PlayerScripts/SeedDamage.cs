using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        // Check if the colliding particle has the tag "SeedParticles"
        if (other.CompareTag("SeedParticles"))
        {
            DealDamageToPlayer();
        }
    }

    private void DealDamageToPlayer()
    {
        // Access the PlayerStates component attached to this GameObject
        PlayerStates playerHealth = GetComponent<PlayerStates>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // Deal damage to the player
        }
    }
}

