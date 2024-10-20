using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 10;

    private Vector3 targetDirection;
    private Rigidbody rb;

    private void Start()
    {
        // Set the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Automatically destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    // Set the target direction when instantiated
    public void Initialize(Vector3 direction)
    {
        if (this != null)
        {
            targetDirection = direction.normalized; // Normalize to get the direction
        }
    }

    private void FixedUpdate()
    {
        if (this != null)
        {
            // Move the projectile in the target direction
            rb.MovePosition(transform.position + targetDirection * speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hits the player
        if (other.CompareTag("Player") && this != null)
        {
            // Optionally deal damage to the player (if there's a damage handling system)
            // other.GetComponent<PlayerHealth>().TakeDamage(damage);

            // Destroy the projectile on collision
            Destroy(gameObject);
        }
    }
}
