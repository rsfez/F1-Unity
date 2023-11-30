using UnityEngine;

public class DrawTrack : MonoBehaviour
{
    public LineRenderer lineRenderer;

    new public Camera camera;

    void Start()
    {
        Vector3[] points = LoadTrack();

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        camera.GetComponent<CameraController>().FitCameraToLineTrack(lineRenderer);
    }

    private Vector3[] LoadTrack()
    {
        string[][] csv = CSVUtils.Parse("Assets/Data/2023/Japan/R/track.csv");
        Vector3[] points = new Vector3[csv.Length];

        for (int y = 0; y < csv.Length; y++)
        {
            Vector3 point = CSVLineToTrackPoint(csv[y]);
            points[y] = point;
        }

        return points;
    }

    private Vector3 CSVLineToTrackPoint(string[] line)
    {
        return new Vector3(float.Parse(line[0]), float.Parse(line[1]), 0f);
    }
}
