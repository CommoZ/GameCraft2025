using UnityEngine;

public class SpeedTransparencyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Fade Settings")]
    [SerializeField] private string targetLayerName = "Ground";
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float minAlpha = 0f;
    [SerializeField] private float maxAlpha = 1f;

    private int targetLayer;
    private SpriteRenderer[] affectedRenderers;

    void Awake()
    {
        targetLayer = LayerMask.NameToLayer(targetLayerName);

        affectedRenderers = FindObjectsOfType<SpriteRenderer>();

        // Filter only objects in the target layer
        affectedRenderers = System.Array.FindAll(
            affectedRenderers,
            r => r.gameObject.layer == targetLayer
        );
    }

    void Update()
    {
        float speed = playerRb.linearVelocity.magnitude;

        // Normalize speed 0–1
        float t = Mathf.Clamp01(speed / maxSpeed);

        // Invert if needed (fast = more transparent)
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, t);

        foreach (SpriteRenderer sr in affectedRenderers)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
