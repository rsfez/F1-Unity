using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minOrthographicSize = 1f;

    new private Camera camera;
    private float maxOrthographicSize = float.MaxValue;
    private Vector3 targetPosition;
    private LineRenderer track;
    private float zoomSpeed = 1000f;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize -= scroll * zoomSpeed;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minOrthographicSize, maxOrthographicSize);

        if (scroll > 0)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z);
            targetPosition = camera.ScreenToWorldPoint(mousePos);
        }
        else if (scroll < 0)
        {
            targetPosition = transform.position;
        }

        // Move the camera towards the target position
        if (camera.orthographicSize >= maxOrthographicSize)
            FitCameraToTrack();
        else
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    public void FitCameraToLineTrack(LineRenderer lineRenderer)
    {
        track = lineRenderer;
        if (track == null || track.positionCount <= 0)
            return;

        Bounds bounds = new Bounds(track.GetPosition(0), Vector3.zero);
        for (int i = 0; i < track.positionCount; i++)
        {
            bounds.Encapsulate(track.GetPosition(i));
        }

        Vector3 boundsSize = bounds.size;
        float aspectRatio = camera.aspect;

        // Determine the orthographic size
        float orthographicSize = 0;
        if (aspectRatio >= 1.0f) // Wide aspect ratio
        {
            // For wider screens, use half the bounds width, adjusted for aspect ratio
            orthographicSize = boundsSize.x / 2f / aspectRatio;
        }
        else // Tall aspect ratio
        {
            // For taller screens, use half the bounds height
            orthographicSize = boundsSize.y / 2f;
        }

        // Set the orthographic size
        camera.orthographicSize = orthographicSize;
        maxOrthographicSize = orthographicSize;

        // Set the camera position
        transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);

        // Optionally, adjust the near and far clipping planes
        camera.nearClipPlane = -boundsSize.magnitude; // Using negative value as orthographic camera can view 'behind' its position
        camera.farClipPlane = boundsSize.magnitude;
    }

    private void FitCameraToTrack()
    {
        FitCameraToLineTrack(track);
    }
}
