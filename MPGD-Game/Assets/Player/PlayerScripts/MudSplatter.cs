using UnityEngine;
using UnityEngine.UI;

public class MudSplatter : MonoBehaviour
{
    public GameObject splatterPrefab; // Assign a UI prefab of a brown circle
    public Transform canvas; // Assign the game UI canvas

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
    }
}
