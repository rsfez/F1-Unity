using System.Linq;
using Controllers;
using Models;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Drive : MonoBehaviour
{
    private const int SegmentSize = 50;
    private Driver _driver;
    private TelemetryEvent _segmentStart, _segmentEnd;
    private Spline _spline;
    private Timer _timer;

    private void Start()
    {
        _driver = GetComponent<DriverController>().Driver;
        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        var splineContainer = GameObject.FindWithTag("GP").AddComponent<SplineContainer>();
        _spline = splineContainer.Spline;
        InitSpline(_driver.LastVisitedTelemetryEvent);
        transform.position = _spline.Knots.First().Position;
    }

    public void Update()
    {
        if (_driver == null || !_timer || _spline == null || !_timer.IsRunning()) return;
        var normalizedTime = GetNormalizedTime();
        var currentTime = _timer.GetTime();

        // "Fast-forward" to lastly covered telemetry event 
        while (currentTime > _driver.LastVisitedTelemetryEvent.Time)
            _driver.LastVisitedTelemetryEvent = _driver.LastVisitedTelemetryEvent.Next;

        // Reset spline when it gets close to the end to continue with smooth interpolation
        if (normalizedTime > 0.90)
        {
            UpdateSplineSegment(currentTime);
        }
        else
        {
            var position = _spline.EvaluatePosition(normalizedTime);
            transform.position = position;
        }
    }

    private void UpdateSplineSegment(long currentTime)
    {
        while (currentTime > _segmentStart.Time)
        {
            _segmentStart = _segmentStart.Next;
            _segmentEnd = _segmentEnd.Next;
            _spline.RemoveAt(0);
            _spline.Add(new BezierKnot(_segmentEnd.Position));
        }

        // Using the current position to start the new segment as not to teleport to the next one
        TelemetryEvent current = new(transform.position, currentTime, _driver.LastVisitedTelemetryEvent.DriverAhead)
        {
            Next = _segmentStart
        };
        _segmentStart = current;
        _spline.Insert(0, new BezierKnot(current.Position));
        _spline.RemoveAt(_spline.Knots.Count() - 1);
    }

    private void InitSpline(TelemetryEvent segmentStart)
    {
        _segmentStart = segmentStart;
        var current = segmentStart;
        for (var i = 0; i < SegmentSize; i++)
        {
            _spline.Add(new BezierKnot(new float3(current.Position.x, current.Position.y, 0)));
            current = current.Next;
        }

        _segmentEnd = current;
    }

    private float GetNormalizedTime()
    {
        return (_timer.GetTime() - (float)_segmentStart.Time) / (_segmentEnd.Time - (float)_segmentStart.Time);
    }
}