using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleFlicker2D : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField] private Light2D light2D;

    [SerializeField] private float baseIntensity = 1.2f;
    [SerializeField] private float intensityVariation = 0.25f;

    [SerializeField] private float baseRadius = 3f;
    [SerializeField] private float radiusVariation = 0.4f;

    [SerializeField] private float flickerSpeed = 6f;

    float noiseOffset;

    void Awake()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        noiseOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        // Smooth Perlin noise (natural flicker)
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, noiseOffset);

        light2D.intensity = baseIntensity + (noise - 0.5f) * intensityVariation;
        light2D.pointLightOuterRadius = baseRadius + (noise - 0.5f) * radiusVariation;
    }
}
