using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float snapSpeed = 20f;

    private Vector3 targetPosition;
    private bool snapping = false;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (snapping)
        {
            //transform.position = Vector3.Lerp(
            //    transform.position,
            //    targetPosition,
            //    snapSpeed * Time.deltaTime
            //);


            //if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            //{
            //    transform.position = targetPosition;
            //    snapping = false;
            //}
            transform.position = targetPosition;
            snapping = false;
        }
    }

    public void SnapTo(Vector3 newPosition)
    {
        targetPosition = new Vector3(
            newPosition.x,
            newPosition.y,
            transform.position.z
        );

        snapping = true;
    }
}
