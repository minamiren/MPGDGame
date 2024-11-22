using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Renderer enemyRenderer;
    private int attackCount = 0;
    private EnemySpawner spawner;

    //DropDown
    private DropDown dropDown;

    private Color[] healthColors = new Color[]
    {
        Color.green,
        new Color(1f, 0.64f, 0f),
        Color.red,
        new Color(0.65f, 0.16f, 0.16f),
        Color.black
    };

    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        UpdateColor();
        dropDown = GetComponent<DropDown>();
    }

    public void TakeDamage()
    {
        if (attackCount < healthColors.Length - 1)
        {
            attackCount++;
            UpdateColor();
        }
        else
        {
            DestroyEnemy();
        }
    }

    private void UpdateColor()
    {
        enemyRenderer.material.color = healthColors[attackCount];
    }

    public void SetSpawner(EnemySpawner spawnerReference)
    {
        spawner = spawnerReference;
    }

    private void DestroyEnemy()
    {
        if (spawner != null)
            spawner.OnEnemyDestroyed();
        if (dropDown != null)
        {
            dropDown.DropItem(transform.position);
        }
        Destroy(gameObject);
    }
}
