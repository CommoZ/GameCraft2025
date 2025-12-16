using UnityEngine;

public class CameraZone : MonoBehaviour
{
    private CameraController cameraController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        cameraController.SnapTo(transform.position);
    }
}
