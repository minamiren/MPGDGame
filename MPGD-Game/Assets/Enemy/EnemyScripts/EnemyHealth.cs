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
        new Color(0.13f, 0.55f, 0.13f),  // Forest Green
        new Color(0.66f, 0.71f, 0.61f),  // Sage Green
        new Color(0.83f, 0.79f, 0.34f),  // Yellowing
        new Color(0.48f, 0.29f, 0.24f),  // Brownish
        new Color(0.82f, 0.82f, 0.82f)   // Pale Grey
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
        SoundManager.PlaySound(SoundType.ENEMIEDIE);
        Destroy(gameObject);
    }
}