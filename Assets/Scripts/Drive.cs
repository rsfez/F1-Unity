using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Drive : MonoBehaviour
{
    private int segmentSize = 20;
    private Spline spline;
    private Driver driver;
    private Timer timer;
    private TelemetryEvent segmentStart, segmentEnd;

    void Start()
    {
        driver = GetComponent<DriverController>().driver;
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        SplineContainer splineContainer = GameObject.FindWithTag("Track").AddComponent<SplineContainer>();
        spline = splineContainer.Spline;
        InitSpline(driver.initialTelemetryEvent);
        transform.position = spline.Knots.First().Position;
    }

    void Update()
    {
        if (driver == null || timer == null || spline == null || !timer.IsRunning()) return;
        float normalizedTime = GetNormalizedTime();
        long currentTime = timer.GetTime();

        if (normalizedTime > 0.99)
        {
            while (currentTime > segmentStart.time)
            {
                segmentStart = segmentStart.next;
                segmentEnd = segmentEnd.next;
                spline.RemoveAt(0);
                spline.Add(new BezierKnot(segmentEnd.position));
            }
            TelemetryEvent current = new TelemetryEvent(transform.position, currentTime);
            current.next = segmentStart;
            segmentStart.previous = current;
            segmentStart = current;
            spline.Insert(0, new BezierKnot(current.position));
            spline.RemoveAt(spline.Knots.Count() - 1);
        } else {
            float3 position = spline.EvaluatePosition(normalizedTime);
            transform.position = position;
        }
    }

    private void InitSpline(TelemetryEvent segmentStart)
    {
        this.segmentStart = segmentStart;
        TelemetryEvent current = segmentStart;
        for (int i = 0; i < segmentSize; i++)
        {
            spline.Add(new BezierKnot(new float3(current.position.x, current.position.y, 0)));
            current = current.next;
        }
        segmentEnd = current;
    }

    private float GetNormalizedTime()
    {
        return ((float) timer.GetTime() - (float) segmentStart.time) / ((float) segmentEnd.time - (float) segmentStart.time);
    }
}
