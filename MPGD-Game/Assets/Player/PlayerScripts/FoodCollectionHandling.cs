using UnityEngine;

// Simple script to track when player first collects food - collecting food
// and disturbing the environment is the trigger for enemies to attack

public class PlayerFoodCollection : MonoBehaviour
{
    public static bool hasCollectedFood = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            hasCollectedFood = true;
        }
    }
}

