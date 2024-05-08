using UnityEngine;

namespace Models
{
    public class TelemetryEvent
    {
        public readonly short DriverAhead;
        public readonly Vector3 Position;
        public readonly long Time;
        public TelemetryEvent Previous, Next;

        public TelemetryEvent(Vector3 position, long time, short driverAhead)
        {
            Position = position;
            Time = time;
            DriverAhead = driverAhead;
        }
    }
}