using UnityEngine;
using UnityEngine.AI;

public class MudEnemyAttack : MonoBehaviour
{
    public ParticleSystem mudSpray; // Assign the particle system in the Inspector
    public float timeBetweenAttacks = 2f;
    public float sprayDuration = 0.5f;
    private bool alreadyAttacked;

    private void Awake()
    {
        mudSpray = GetComponentInChildren<ParticleSystem>(); // Ensure the particle system is assigned
    }

    public void AttackPlayer()
    {
        // Stop moving while attacking
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        if (!alreadyAttacked)
        {
            SprayMud();
            alreadyAttacked = true;
            Invoke(nameof(StopMudSpray), sprayDuration);
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Cooldown before the next attack
        }
    }

    private void SprayMud()
    {
        mudSpray.Play(); // Trigger the particle system
    }

    private void StopMudSpray()
    {
        mudSpray.Stop();
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
