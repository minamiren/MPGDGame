using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public EnemySpawner spawner;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public int damage;
    public GameObject spikePrefab;  // Assign a cube prefab in the Inspector
    public float spikeRotationSpeed = 100f;  // Rotation speed (can be set in Inspector)
    private GameObject activeSpike;  // Reference to the active spike
    private bool hasDealtDamage = false;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        // uncommenting this causes jerky behaviour when the attacker is close to the player
        agent.SetDestination(transform.position);

        // Reset damage
        //hasDealtDamage = false;

        // Face the player
        // transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Spawn the spike when the attack starts
            StartCoroutine(SpinAndAttack());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);  // Set the cooldown between attacks
        }
    }

    private IEnumerator SpinAndAttack()
    {
        // Spawn the spike as a cube around the enemy
        activeSpike = Instantiate(spikePrefab, transform.position + transform.right * 1f, Quaternion.identity);
        activeSpike.transform.localScale = new Vector3(0.1f, 0.1f, 2f);  // Adjust size to represent a spike
        activeSpike.transform.SetParent(transform);  // Make spike a child of the enemy

        float totalRotation = 0f;  // Track the total rotation to perform one full spin

        // Spin the spike around the enemy
        while (totalRotation < 360f)
        {
            float step = spikeRotationSpeed * Time.deltaTime;  // Rotation step per frame

            activeSpike.transform.RotateAround(transform.position, Vector3.up, step);  // Rotate around enemy

            totalRotation += step;  // Update total rotation

            // Check if the player is within the spike's path during rotation
            if (IsPlayerHitBySpike() && !hasDealtDamage)
            {
                DealDamageToPlayer();  // Deal damage if the player is hit by the spike
            }

            yield return null;  // Wait until next frame
        }

        // Destroy the spike after one full spin
        hasDealtDamage = false;
        Destroy(activeSpike);
    }

    private bool IsPlayerHitBySpike()
    {
        // Calculate the distance from the spike to the player
        float distanceToPlayer = Vector3.Distance(activeSpike.transform.position, player.position);

        // Check if the player is close enough to the spike to take damage
        return (distanceToPlayer <= 2f);
    }

    private void DealDamageToPlayer()
    {
        // Assuming your player has a method to take damage
        PlayerStates playerHealth = player.GetComponent<PlayerStates>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);  // Adjust damage value as necessary
            hasDealtDamage = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;  // Allow the enemy to attack again after cooldown
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        if (spawner != null)
            spawner.OnEnemyDestroyed();
        Destroy(gameObject);
    }

}
