using Models;
using UnityEngine;

public class DriverCameraController : MonoBehaviour
{
    private Driver _driver;
    private TrackCameraController _trackCameraController;

    private void Awake()
    {
        _trackCameraController = GetComponent<TrackCameraController>();
    }

    private void Update()
    {
        if (_driver == null) return;
        transform.position = new Vector3(_driver.GameObject.transform.position.x,
            _driver.GameObject.transform.position.y, transform.position.z);
    }

    public void SetDriver(Driver driver)
    {
        _driver = driver;
        enabled = driver != null;
        _trackCameraController.enabled = !enabled;
    }
}