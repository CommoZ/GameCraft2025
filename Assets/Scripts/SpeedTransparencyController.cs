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

	[Header("Fade Speeds")]
	[SerializeField] private float fadeOutSpeed = 12f; // fast when moving
	[SerializeField] private float fadeInSpeed = 3f;   // slow when stopping

	private int targetLayer;
	private SpriteRenderer[] affectedRenderers;

	private float currentAlpha = 1f;

	void Awake()
	{
		targetLayer = LayerMask.NameToLayer(targetLayerName);

		affectedRenderers = FindObjectsOfType<SpriteRenderer>();
		affectedRenderers = System.Array.FindAll(
			affectedRenderers,
			r => r.gameObject.layer == targetLayer
		);
	}

	void Update()
	{
		float speed = playerRb.linearVelocity.magnitude;

		// Compute target alpha from speed
		float t = Mathf.Clamp01(speed / maxSpeed);
		float targetAlpha = Mathf.Lerp(maxAlpha, minAlpha, t);

		// Choose fade speed based on direction
		float fadeSpeed = targetAlpha > currentAlpha
			? fadeInSpeed     // becoming visible
			: fadeOutSpeed;   // becoming transparent

		currentAlpha = Mathf.MoveTowards(
			currentAlpha,
			targetAlpha,
			fadeSpeed * Time.deltaTime
		);

		foreach (SpriteRenderer sr in affectedRenderers)
		{
			Color c = sr.color;
			c.a = currentAlpha;
			sr.color = c;
		}
	}
}
