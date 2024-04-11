using UnityEngine;

public class TelemetryEvent
{
    public readonly Vector3 position;
    public readonly long time;
    public TelemetryEvent previous, next;

    public TelemetryEvent(Vector3 position, long time)
    {
        this.position = position;
        this.time = time;
    }

    private TelemetryEvent(Vector3 position, long time, TelemetryEvent previous, TelemetryEvent next)
    {
        this.position = position;
        this.time = time;
        this.previous = previous;
        this.next = next;
    }

    public TelemetryEvent CopyWithTime(long currentTime)
    {
        return new TelemetryEvent(position, currentTime, previous, next);
    }

    public static TelemetryEvent GetFromCSVLine(string[] line)
    {
        return new TelemetryEvent(
            new Vector3(int.Parse(line[7]), int.Parse(line[8]), 0),
            int.Parse(line[0])
        );
    }
}