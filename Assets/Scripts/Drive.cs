using System;
using Unity.VisualScripting;
using UnityEngine;

public class Drive : MonoBehaviour
{
    private Driver driver;
    private Timer timer;
    private TelemetryEvent currentTelemetryEvent, nextTelemetryEvent;

    void Start()
    {
        driver = GetComponent<DriverController>().driver;
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        currentTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
        driver.session.SafeIncrementTelemetryEventIndex();
        nextTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
        transform.position = currentTelemetryEvent.position;
    }

    void Update()
    {
        if (driver == null || timer == null || !timer.IsRunning()) return;
        long currentTime = timer.GetTime();
        long timeSinceLastUpdate = currentTime - currentTelemetryEvent.time;
        long totalMovementTime = nextTelemetryEvent.time - currentTelemetryEvent.time;
        if (currentTime < nextTelemetryEvent.time)
        {
            float interpolationFactor = Math.Clamp(timeSinceLastUpdate / (float)totalMovementTime, 0, 1);
            transform.position =
                Vector3.LerpUnclamped(currentTelemetryEvent.position, nextTelemetryEvent.position, interpolationFactor);
        }
        else
        {
            while (currentTime >= nextTelemetryEvent.time)
            {
                currentTelemetryEvent = nextTelemetryEvent.CopyWithTime(currentTime);
                driver.session.SafeIncrementTelemetryEventIndex();
                nextTelemetryEvent = driver.session.telemetryEvents[driver.session.currentTelemetryEventIndex];
            }
        }
    }
}
