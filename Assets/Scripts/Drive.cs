using System;
using UnityEngine;

public class Drive : MonoBehaviour
{
    private Driver driver;

    private TelemetryEvent currentTelemetryEvent, nextTelemetryEvent;

    void Start()
    {
        driver = GetComponent<DriverController>().driver;
        currentTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
        driver.session.SafeIncrementTelemetryEventIndex();
        nextTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
        transform.position = currentTelemetryEvent.position;
    }

    void Update()
    {
        if (driver == null) return;
        long currentTime = (long)Math.Round(Time.time * 1000);
        long timeSinceLastUpdate = currentTime - currentTelemetryEvent.time;
        long totalMovementTime = nextTelemetryEvent.time - currentTelemetryEvent.time;
        if (currentTime < nextTelemetryEvent.time)
        {
            float interpolationFactor = Math.Clamp((float)timeSinceLastUpdate / (float)totalMovementTime, 0, 1);
            transform.position =
                Vector3.Lerp(currentTelemetryEvent.position, nextTelemetryEvent.position, interpolationFactor);
        }
        else
        {
            while (currentTime >= nextTelemetryEvent.time)
            {
                currentTelemetryEvent = nextTelemetryEvent;
                driver.session.SafeIncrementTelemetryEventIndex();
                nextTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
            }

        }
    }
}
