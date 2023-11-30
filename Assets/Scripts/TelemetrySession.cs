public class TelemetrySession
{
    public readonly TelemetryEvent[] telemetryEvents;
    public int currentTelemetryEventIndex = 0;

    public TelemetrySession(TelemetryEvent[] telemetryEvents)
    {
        this.telemetryEvents = telemetryEvents;
    }

    public void SafeIncrementTelemetryEventIndex()
    {
        if (++currentTelemetryEventIndex >= telemetryEvents.Length)
        {
            currentTelemetryEventIndex = 0;
        }
    }
}