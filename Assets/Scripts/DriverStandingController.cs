using UnityEngine;
using UnityEngine.EventSystems;

public class DriverStandingController : MonoBehaviour, IPointerClickHandler
{
    private Driver driver;
    private GPController gPController;
    private bool isSelected = false;

    void Awake()
    {
        gPController = GameObject.FindGameObjectWithTag("GP").GetComponent<GPController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gPController.camera.GetComponent<DriverCameraController>().SetDriver(isSelected ? null : driver);
        isSelected = !isSelected;
    }

    public void SetDriver(Driver driver)
    {
        this.driver = driver;
        name = driver.position + ". " + driver.abbreviation;
    }
}
