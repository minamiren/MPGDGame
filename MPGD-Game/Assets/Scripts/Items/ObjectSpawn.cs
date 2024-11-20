using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject foodPrefab;
    public int initialFoodCount = 8;
    private Vector3 minSpawnRange = new Vector3(70, 1, 36);
    private Vector3 maxSpawnRange = new Vector3(165, 1, 156);

    private List<GameObject> spawnedFoods = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialFoodCount; i++)
        {
            SpawnNewFood();
        }
    }

    public void SpawnNewFood()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(minSpawnRange.x, maxSpawnRange.x),
            Random.Range(minSpawnRange.y, maxSpawnRange.y),
            Random.Range(minSpawnRange.z, maxSpawnRange.z)
        );
        GameObject newFood = Instantiate(foodPrefab, randomPosition, Quaternion.identity);
        newFood.SetActive(true);
    }
}
