using Models.Builders;
using UnityEngine;

public class DrawTrackController : MonoBehaviour
{
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public new GameObject camera;

    private void Start()
    {
        var points = TrackBuilder.Instance.Build();

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        if (camera != null) camera.GetComponent<TrackCameraController>().FitCameraToTrack();
    }
}