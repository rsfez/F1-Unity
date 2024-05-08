using UnityEngine;

public class DrawTrack : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public new GameObject camera;

    private void Start()
    {
        var points = LoadTrack();

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        if (camera != null) camera.GetComponent<TrackCameraController>().FitCameraToTrack();
    }

    private Vector3[] LoadTrack()
    {
        var csv = CSVUtils.Parse("Data/2023/Japan/R/track");
        var points = new Vector3[csv.Length];

        for (var y = 0; y < csv.Length; y++)
        {
            var point = CSVLineToTrackPoint(csv[y]);
            points[y] = point;
        }

        return points;
    }

    private Vector3 CSVLineToTrackPoint(string[] line)
    {
        return new Vector3(int.Parse(line[0]), int.Parse(line[1]), 0);
    }
}