using UnityEngine;

public class TrackCamera : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float sizeMultiplier = 0.1f;

    void Update()
    {
        if (lineRenderer.positionCount <= 0)
            return;

        Bounds bounds = new Bounds(lineRenderer.GetPosition(0), Vector3.zero);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            bounds.Encapsulate(lineRenderer.GetPosition(i));
        }

        FitCameraToBounds(bounds);
    }

    void FitCameraToBounds(Bounds bounds)
    {
        Camera camera = GetComponent<Camera>();
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

        // Set the camera position
        camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, camera.transform.position.z);

        // Optionally, adjust the near and far clipping planes
        camera.nearClipPlane = -boundsSize.magnitude; // Using negative value as orthographic camera can view 'behind' its position
        camera.farClipPlane = boundsSize.magnitude;
    }

}

