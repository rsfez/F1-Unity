using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Drive : MonoBehaviour
{
    private readonly int segmentSize = 50;
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
        InitSpline(driver.lastVisitedTelemetryEvent);
        transform.position = spline.Knots.First().Position;
    }

    void Update()
    {
        if (driver == null || timer == null || spline == null || !timer.IsRunning()) return;
        float normalizedTime = GetNormalizedTime();
        long currentTime = timer.GetTime();

        // "Fast-forward" to lastly covered telemetry evevent 
        while (currentTime > driver.lastVisitedTelemetryEvent.time)
        {
            driver.lastVisitedTelemetryEvent = driver.lastVisitedTelemetryEvent.next;
        }

        // Reset spline when it gets close to the end to continue with smooth interpolation
        if (normalizedTime > 0.90)
        {
            UpdateSplineSegment(currentTime);
        }
        else
        {
            float3 position = spline.EvaluatePosition(normalizedTime);
            transform.position = position;
        }
    }

    private void UpdateSplineSegment(long currentTime)
    {
        while (currentTime > segmentStart.time)
        {
            segmentStart = segmentStart.next;
            segmentEnd = segmentEnd.next;
            spline.RemoveAt(0);
            spline.Add(new BezierKnot(segmentEnd.position));
        }
        // Using the current position to start the new segment as not to teleport to the next one
        TelemetryEvent current = new TelemetryEvent(transform.position, currentTime, driver.lastVisitedTelemetryEvent.driverAhead);

        current.next = segmentStart;
        segmentStart.previous = current;
        segmentStart = current;
        spline.Insert(0, new BezierKnot(current.position));
        spline.RemoveAt(spline.Knots.Count() - 1);
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
        return (timer.GetTime() - (float)segmentStart.time) / (segmentEnd.time - (float)segmentStart.time);
    }
}
