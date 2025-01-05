using UnityEngine;

public class SandboxEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;      // Reference to the enemy prefab
    public Transform[] spawnPoints;    // Array of spawn points
    public int maxEnemies = 1;          // Maximum number of enemies allowed at a time

    private int currentEnemyCount = 0;  // Current number of active enemies

    private void Start()
    {
        // Spawn initial enemies up to the max limit
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    // Method to spawn an enemy
    public void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoints.Length > 0 && currentEnemyCount < maxEnemies)
        {
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject newEnemy = Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
            currentEnemyCount++;
        }
    }

    // Method to call when an enemy is destroyed
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
        SpawnEnemy(); // Spawn a new enemy to replace the destroyed one
    }
}


