using UnityEngine;

public class FPSLogger : MonoBehaviour
{
	private float deltaTime = 0f;

	private void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;

		Debug.Log($"FPS: {Mathf.RoundToInt(fps)}");
	}
}