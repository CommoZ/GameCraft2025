using UnityEngine;
using UnityEngine.UI; // Required when using UI elements like RawImage

public class BackgroundScrollerUI : MonoBehaviour
{
    [Header("Settings")]
    // Controls how fast it scrolls. 
    // Positive numbers scroll Right-to-Left.
    // Negative numbers scroll Left-to-Right.
    [Tooltip("Use negative values for Left-to-Right scrolling")]
    public float scrollSpeed = -0.1f;

    private RawImage rawImage;
    private Rect currentUVRect;

    void Start()
    {
        // Get the RawImage component attached to this object
        rawImage = GetComponent<RawImage>();
        
        // Store the starting UV rectangle definition
        currentUVRect = rawImage.uvRect;
    }

    void Update()
    {
        // Calculate the new X position based on time and speed
        // We use += because the speed is negative for L->R movement
        currentUVRect.x += scrollSpeed * Time.deltaTime;

        // Apply the modified rectangle back to the RawImage
        rawImage.uvRect = currentUVRect;
    }
}