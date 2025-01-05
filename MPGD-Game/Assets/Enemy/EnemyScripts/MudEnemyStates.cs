using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MudEnemyStates : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy;

    // Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private MudEnemyAttack mudEnemyAttack;

    // Group Behavior
    public float groupRange = 10f; // Range to detect nearby enemies
    public float alignmentStrength = 0.5f; // Strength of alignment behavior
    public float cohesionStrength = 0.5f; // Strength of cohesion behavior

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        mudEnemyAttack = GetComponent<MudEnemyAttack>();
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Enemies can only react if the player has collected food
        if (PlayerFoodCollection.hasCollectedFood)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) mudEnemyAttack.AttackPlayer();
        }
        else
        {
            // Continue patrol if the player hasn't collected food
            Patroling();
        }

        ApplyGroupBehavior();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
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

    private void ApplyGroupBehavior()
    {
        // Get all nearby enemies
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, groupRange, whatIsEnemy);

        Vector3 cohesionVector = Vector3.zero; // Move towards the center of nearby enemies
        Vector3 alignmentVector = Vector3.zero; // Align movement with nearby enemies
        int groupCount = 0;

        foreach (Collider collider in nearbyEnemies)
        {
            if (collider.gameObject != gameObject) // Avoid self
            {
                cohesionVector += collider.transform.position;
                alignmentVector += collider.GetComponent<NavMeshAgent>().velocity;
                groupCount++;
            }
        }

        if (groupCount > 0)
        {
            // Calculate average position and velocity
            cohesionVector /= groupCount;
            alignmentVector /= groupCount;

            // Apply cohesion (move towards group center)
            Vector3 cohesionDirection = (cohesionVector - transform.position).normalized * cohesionStrength;

            // Apply alignment (match group movement)
            Vector3 alignmentDirection = alignmentVector.normalized * alignmentStrength;

            // Combine forces
            Vector3 groupMovement = cohesionDirection + alignmentDirection;

            // Adjust the agent's destination
            agent.SetDestination(transform.position + groupMovement);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize group range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, groupRange);
    }
}
