using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public GameObject spikePrefab;
    public float spikeRotationSpeed = 100f;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    private GameObject activeSpike;
    private bool hasDealtDamage = false;
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    public void AttackPlayer()
    {
        // Stop moving while attacking
        GetComponent<NavMeshAgent>().SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            StartCoroutine(SpinAndAttack());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Set cooldown between attacks
        }
    }

    private IEnumerator SpinAndAttack()
    {
        // Spawn the spike as a cube around the enemy
        activeSpike = Instantiate(spikePrefab, transform.position + transform.right * 1f, Quaternion.identity);
        activeSpike.transform.localScale = new Vector3(0.1f, 0.1f, 2f); // Adjust size to represent a spike
        activeSpike.transform.SetParent(transform); // Make spike a child of the enemy

        float totalRotation = 0f; // Track the total rotation to perform one full spin

        while (totalRotation < 360f)
        {
            float step = spikeRotationSpeed * Time.deltaTime; // Rotation step per frame
            activeSpike.transform.RotateAround(transform.position, Vector3.up, step); // Rotate around enemy
            totalRotation += step;

            if (IsPlayerHitBySpike() && !hasDealtDamage)
            {
                DealDamageToPlayer(); // Deal damage if the player is hit by the spike
            }

            yield return null; // Wait until the next frame
        }

        hasDealtDamage = false;
        Destroy(activeSpike); // Destroy the spike after one full spin
    }

    private bool IsPlayerHitBySpike()
    {
        float distanceToPlayer = Vector3.Distance(activeSpike.transform.position, player.position);
        return (distanceToPlayer <= 2f); // Check if the player is close enough to the spike
    }

    private void DealDamageToPlayer()
    {
        PlayerStates playerHealth = player.GetComponent<PlayerStates>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // Adjust damage value as necessary
            hasDealtDamage = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false; // Allow the enemy to attack again after cooldown
    }
}

