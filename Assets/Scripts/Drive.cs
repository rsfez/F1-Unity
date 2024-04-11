using System;
using UnityEngine;

public class Drive : MonoBehaviour
{
    private Driver driver;
    private Timer timer;
    private TelemetryEvent currentTelemetryEvent;

    void Start()
    {
        driver = GetComponent<DriverController>().driver;
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        currentTelemetryEvent = driver.currentTelemetryEvent;
        transform.position = currentTelemetryEvent.position;
    }

    void Update()
    {
        if (driver == null || timer == null || !timer.IsRunning()) return;
        long currentTime = timer.GetTime();
        long timeSinceLastUpdate = currentTime - currentTelemetryEvent.time;
        long totalMovementTime = currentTelemetryEvent.next.time - currentTelemetryEvent.time;
        if (currentTime < currentTelemetryEvent.next.time)
        {
            float interpolationFactor = Math.Clamp(timeSinceLastUpdate / (float)totalMovementTime, 0, 1);
            transform.position =
                Vector3.LerpUnclamped(currentTelemetryEvent.position, currentTelemetryEvent.next.position, interpolationFactor);
        }
        else
        {
            while (currentTime >= currentTelemetryEvent.next.time)
            {
                currentTelemetryEvent = currentTelemetryEvent.next.CopyWithTime(currentTime);
            }
        }
    }
}
