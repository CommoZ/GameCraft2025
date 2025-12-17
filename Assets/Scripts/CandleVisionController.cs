using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleVisionController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Rigidbody2D playerRb;
	[SerializeField] private Light2D candleLight;

	[Header("Speed Settings")]
	[SerializeField] private float maxSpeed = 10f;

	[Header("Vision Settings")]
	[SerializeField] private float maxRadius = 6f;
	[SerializeField] private float minRadius = 2.5f;
	[SerializeField] private float maxIntensity = 1.2f;
	[SerializeField] private float minIntensity = 0.5f;

	[Header("Fade Speeds")]
	[SerializeField] private float shrinkSpeed = 10f; // fast loss of vision
	[SerializeField] private float growSpeed = 3f;    // slow regain of vision

	void Update()
	{
		float speed = playerRb.linearVelocity.magnitude;
		float t = Mathf.Clamp01(speed / maxSpeed);

		// Target values based on speed
		float targetRadius = Mathf.Lerp(maxRadius, minRadius, t);
		float targetIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);

		// Choose speed depending on direction
		float radiusSpeed = targetRadius < candleLight.pointLightOuterRadius
			? shrinkSpeed
			: growSpeed;

		float intensitySpeed = targetIntensity < candleLight.intensity
			? shrinkSpeed
			: growSpeed;

		// Smooth transition
		candleLight.pointLightOuterRadius = Mathf.MoveTowards(
			candleLight.pointLightOuterRadius,
			targetRadius,
			radiusSpeed * Time.deltaTime
		);

		candleLight.intensity = Mathf.MoveTowards(
			candleLight.intensity,
			targetIntensity,
			intensitySpeed * Time.deltaTime
		);
	}
}
