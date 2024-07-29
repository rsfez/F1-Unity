using Models;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Controllers.Interactors
{
    public class DriveInteractor
    {
        private const int SegmentSize = 50;
        private readonly Driver _driver;
        private readonly Spline _spline;
        private readonly Timer _timer;
        private readonly Transform _transform;
        private TelemetryEvent _segmentStart, _segmentEnd;

        public DriveInteractor(Transform transform, Driver driver, Timer timer, Spline spline)
        {
            _transform = transform;
            _driver = driver;
            _timer = timer;
            _spline = spline;
            InitSpline(_driver.GetLastVisitedTelemetryEvent());
            _transform.position = _segmentStart.Position;
        }

        public void Drive()
        {
            if (_driver == null || !_timer || _spline == null || !_timer.IsRunning()) return;
            var normalizedTime = GetNormalizedTime();
            var currentTime = _timer.GetTime();
            var currentLapBeforeUpdate = _driver.CurrentLap;

            // "Fast-forward" to lastly covered telemetry event 
            while (currentTime > _driver.GetLastVisitedTelemetryEvent().Time)
                _driver.SetLastVisitedTelemetryEvent(_driver.GetLastVisitedTelemetryEvent().Next);

            // Reset spline when it gets close to the end to continue with smooth interpolation
            if (normalizedTime > 0.9)
            {
                UpdateSplineSegment(currentTime);
            }
            else
            {
                var position = _spline.EvaluatePosition(normalizedTime);
                _transform.position = position;
            }

            // Notify timer of new lap possibility
            var lapAfterUpdate = _driver.CurrentLap;
            if (currentLapBeforeUpdate < lapAfterUpdate) _timer.TryUpdateCurrentLap(_driver.CurrentLap);
        }

        private void UpdateSplineSegment(long currentTime)
        {
            _spline.Clear();
            var newSegmentStart = new TelemetryEvent(_transform.position, currentTime,
                _driver.GetLastVisitedTelemetryEvent().DriverAhead)
            {
                Next = _driver.GetLastVisitedTelemetryEvent().Next
            };
            InitSpline(newSegmentStart);
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
}