using UnityEngine;
using Utils;

namespace Models
{
    public class TelemetryEvent
    {
        public readonly short driverAhead;
        public readonly Vector3 position;
        public readonly long time;
        public TelemetryEvent previous, next;

        public TelemetryEvent(Vector3 position, long time, short driverAhead)
        {
            this.position = position;
            this.time = time;
            this.driverAhead = driverAhead;
        }

        private TelemetryEvent(Vector3 position, long time, short driverAhead, TelemetryEvent previous, TelemetryEvent next)
        {
            this.position = position;
            this.time = time;
            this.driverAhead = driverAhead;
            this.previous = previous;
            this.next = next;
        }

        public TelemetryEvent CopyWithTime(long currentTime)
        {
            return new TelemetryEvent(position, currentTime, driverAhead, previous, next);
        }

        public static TelemetryEvent LoadTelemetryEventsFromCSV(string driverAbbreviation)
        {
            var csv = CsvUtils.Parse("Data/2023/Japan/R/telemetry/" + driverAbbreviation);
            TelemetryEvent previousTelemetryEvent = null;
            TelemetryEvent firstTelemetryEvent = null;
            foreach (var line in csv)
            {
                var currentTelemetryEvent = GetFromCSVLine(line);
                currentTelemetryEvent.previous = previousTelemetryEvent;
                if (previousTelemetryEvent != null) previousTelemetryEvent.next = currentTelemetryEvent;
                previousTelemetryEvent = currentTelemetryEvent;
                firstTelemetryEvent ??= currentTelemetryEvent;
            }

            return firstTelemetryEvent;
        }

        private static TelemetryEvent GetFromCSVLine(string[] line)
        {
            short driverAhead;
            driverAhead = short.TryParse(line[9], out driverAhead) ? driverAhead : (short)0;
            return new TelemetryEvent(
                new Vector3(float.Parse(line[7]), float.Parse(line[8]), 0),
                int.Parse(line[0]),
                driverAhead
            );
        }
    }
}