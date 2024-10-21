using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;      // Reference to the enemy prefab
    public Transform spawnPoint;        // The spawn point of the enemy

    private GameObject currentEnemy;    // To track the currently spawned enemy

    private void Start()
    {
        // Spawn the first enemy at the spawn point
        SpawnEnemy();
    }

    // Method to spawn an enemy
    public void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoint != null)
        {
            currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            // Optional: Set a reference to this spawner in the enemy, if needed
            currentEnemy.GetComponent<EnemyAiTutorial>().spawner = this;
        }
    }

    // Method to call when the enemy is destroyed
    public void OnEnemyDestroyed()
    {
        // Spawn a new enemy at the spawn point
        SpawnEnemy();
    }
}
