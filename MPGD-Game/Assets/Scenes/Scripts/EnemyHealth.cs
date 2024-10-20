using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;  // Reference to health bar slider

    public GameObject healthBarPrefab;  // Assign Health Bar prefab
    private GameObject healthBarInstance;

    private void Start()
    {
        currentHealth = maxHealth;

        // Instantiate health bar above the enemy
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        healthBar = healthBarInstance.GetComponent<Slider>();

        UpdateHealthBar();
    }

    private void Update()
    {
        // Follow the enemy's position
        healthBarInstance.transform.position = transform.position + Vector3.up * 2;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth / maxHealth;  // Update health bar
    }

    private void Die()
    {
        // Destroy health bar when enemy dies
        Destroy(healthBarInstance);
        Destroy(gameObject);
    }
}


