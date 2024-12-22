using UnityEngine;
using UnityEngine.AI;

public class SandboxEnemyAttack : MonoBehaviour
{
    public ParticleSystem seedExp; // Assign the particle system in the Inspector
    public float timeBetweenAttacks = 2f;
    public float sprayDuration = 0.5f;
    private bool alreadyAttacked;
    private Transform player;

    private void Awake()
    {
        seedExp = GetComponentInChildren<ParticleSystem>(); // Ensure the particle system is assigned
    }

    public void AttackPlayer()
    {
        // Stop moving while attacking
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        if (!alreadyAttacked)
        {
            SpraySeed();
            alreadyAttacked = true;
            Invoke(nameof(StopSeedSpray), sprayDuration);
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Cooldown before the next attack
        }
    }

    private void SpraySeed()
    {
        seedExp.Play(); // Trigger the particle system
    }

    private void StopSeedSpray()
    {
        seedExp.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStates playerHealth = player.GetComponent<PlayerStates>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Adjust damage value as necessary
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}

