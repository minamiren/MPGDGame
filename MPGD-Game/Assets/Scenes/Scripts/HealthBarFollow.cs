using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Vector3 offset;            // Offset to position the health bar above the player

    void Update()
    {
        // Update the position of the health bar to follow the player
        transform.position = playerTransform.position + offset;
    }
}

