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
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y);

        float aspectRatio = GetComponent<Camera>().aspect;
        objectSize /= aspectRatio;

        float cameraView =
            Mathf.Tan((GetComponent<Camera>().fieldOfView / 2) * Mathf.Deg2Rad);
        float cameraDistance = objectSize / cameraView;
        cameraDistance += sizeMultiplier * objectSize;

        GetComponent<Camera>().transform.position =
            bounds.center - cameraDistance * Vector3.forward;
        GetComponent<Camera>().transform.LookAt(bounds.center);
    }

}

