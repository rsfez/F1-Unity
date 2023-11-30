using UnityEngine;

public class TelemetryEvent
{
    public readonly Vector3 position;
    public readonly long time;

    public TelemetryEvent(Vector3 position, long time)
    {
        this.position = position;
        this.time = time;
    }

    public static TelemetryEvent GetFromCSVLine(string[] line)
    {
        return new TelemetryEvent(
            new Vector3(int.Parse(line[7]), int.Parse(line[8]), 0),
            int.Parse(line[0])
        );
    }
}