using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickThrow : MonoBehaviour
{   public float gravity = Physics.gravity.y; // Use global gravity
    public Rigidbody rb;
    public float throwSpeed = 8f;
    private Vector3 targetDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetTartget(Vector3 targetPosition)
    {
        //targetDirection = (targetPosition - transform.position).normalized;
        Vector3 launchVelocity = CalculateLaunchVelocity(targetPosition);
        rb.velocity = launchVelocity;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 targetPosition)
    {
        Vector3 toTarget = targetPosition - transform.position;
        float yOffset = toTarget.y;
        toTarget.y = 0; // Zero out the y component for horizontal distance calculation

        float distance = toTarget.magnitude; // Horizontal distance
        float angle = 15f * Mathf.Deg2Rad;   // Choose an angle (45 degrees for max range)
        float speedSquared = (distance * Mathf.Abs(gravity)) / Mathf.Sin(2 * angle); // Kinematic equation

        if (speedSquared <= 0)
            return Vector3.zero; // Return zero if speed is non-viable

        float speed = Mathf.Sqrt(speedSquared);
        Vector3 velocity = toTarget.normalized * speed * Mathf.Cos(angle); // Horizontal velocity component
        velocity.y = speed * Mathf.Sin(angle); // Vertical velocity component

        return velocity;
    }
    void Update()
    {
       // transform.Translate(targetDirection * throwSpeed * Time.deltaTime, Space.World);

    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyAI component from the specific enemy involved in the collision
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(); // Damage only this enemy
            }
            Destroy(gameObject); // Destroy the projectile after hitting an enemy
        }

        // Check if the collision is with terrain or other objects
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Cave"))
        {
            Destroy(gameObject); // Destroy the projectile on impact with terrain or caves
        }
    }

}
