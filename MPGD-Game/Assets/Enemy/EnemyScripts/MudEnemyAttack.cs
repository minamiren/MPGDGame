using UnityEngine;

public class MudEnemyAttack : MonoBehaviour
{
    public ParticleSystem mudSpray; // Assign the particle system in the Inspector
    public float timeBetweenAttacks = 2f;
    private bool alreadyAttacked;

    private void Awake()
    {
        mudSpray = GetComponentInChildren<ParticleSystem>(); // Ensure the particle system is assigned
    }

    public void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            SprayMud();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Cooldown before the next attack
        }
    }

    private void SprayMud()
    {
        mudSpray.Play(); // Trigger the particle system
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
