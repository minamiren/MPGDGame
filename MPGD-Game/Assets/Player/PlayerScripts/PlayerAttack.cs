using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2.0f; // Adjustable distance from the player to the enemy
    public int damageAmount = 100;   // Damage the attack deals, if you're using a health system

    private void Update()
    {
        // Check if the player presses the spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackEnemies();
        }
    }

    private void AttackEnemies()
    {
        // Find all objects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Calculate the distance between the player and the enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // Check if the enemy is within the attack range
            if (distanceToEnemy <= attackRange)
            {
                // Optionally, you can deal damage to the enemy if it has a health system
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount); // Call the TakeDamage method if the enemy has health
                }
                else
                {
                    // If there's no health system, destroy the enemy
                    Destroy(enemy);
                }
            }
        }
    }
}