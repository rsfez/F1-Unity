using UnityEngine;

public class DriverCameraController : MonoBehaviour
{
    private Driver driver;
    private TrackCameraController trackCameraController;

    private void Awake()
    {
        trackCameraController = GetComponent<TrackCameraController>();
    }

    private void Update()
    {
        if (driver == null) return;
        transform.position = new Vector3(driver.gameObject.transform.position.x, driver.gameObject.transform.position.y, transform.position.z);
    }

    public void SetDriver(Driver driver)
    {
        this.driver = driver;
        enabled = driver != null;
        trackCameraController.enabled = !enabled;
    }
}
