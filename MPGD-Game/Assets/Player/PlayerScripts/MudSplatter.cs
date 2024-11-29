using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MudSplatter : MonoBehaviour
{
    public GameObject splatterPrefab; // Assign a UI prefab of a brown circle
    public Transform canvas; // Assign the game UI canvas
    public float duration = 3f;
    public float minSize = 100f;
    public float maxSize = 300f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("MudParticles")) // Ensure particles are tagged correctly
        {
            CreateMudSplatter();
        }
    }

    private void CreateMudSplatter()
    {
        // Instantiate a splatter on the canvas
        GameObject splatter = Instantiate(splatterPrefab, canvas);

        // Randomly position the splatter
        RectTransform rt = splatter.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(
            Random.Range(-canvas.GetComponent<RectTransform>().rect.width / 2, canvas.GetComponent<RectTransform>().rect.width / 2),
            Random.Range(-canvas.GetComponent<RectTransform>().rect.height / 2, canvas.GetComponent<RectTransform>().rect.height / 2)
        );

        // Randomize size
        float randomSize = Random.Range(minSize, maxSize);
        rt.sizeDelta = new Vector2(randomSize, randomSize);

        // Randomize rotation
        // rt.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // Start the fade-out coroutine
        StartCoroutine(FadeOutAndDestroy(splatter, duration)); // Fade out over 3 seconds
    }

    private IEnumerator FadeOutAndDestroy(GameObject splatter, float duration)
    {
        Image splatterImage = splatter.GetComponent<Image>();

        if (splatterImage == null)
        {
            Debug.LogError("Splatter prefab does not have an Image component!");
            yield break;
        }

        Color originalColor = splatterImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / duration); // Gradually reduce alpha
            splatterImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure alpha is set to 0 and destroy the splatter
        splatterImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(splatter);
    }

}
