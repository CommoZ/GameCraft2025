using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlatformLight2D : MonoBehaviour
{
	[Header("Light Settings")]
	[SerializeField] private Light2D platformLight;
	[SerializeField] private float litIntensity = 1.2f;
	[SerializeField] private float unlitIntensity = 0f;
	[SerializeField] private float fadeSpeed = 6f;

	private float targetIntensity;

	void Awake()
	{
		if (platformLight == null)
			platformLight = GetComponentInChildren<Light2D>();

		platformLight.intensity = unlitIntensity;
		targetIntensity = unlitIntensity;
	}

	void Update()
	{
		platformLight.intensity = Mathf.Lerp(
			platformLight.intensity,
			targetIntensity,
			fadeSpeed * Time.deltaTime
		);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerWhole"))
			targetIntensity = litIntensity;
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		//if (collision.gameObject.CompareTag("Player"))
		//	targetIntensity = unlitIntensity;
	}
}
