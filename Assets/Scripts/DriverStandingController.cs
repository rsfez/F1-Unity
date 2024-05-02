using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DriverStandingController : MonoBehaviour, IPointerClickHandler
{
    private Driver driver;
    private GPController gPController;
    private bool isSelected = false;

    public static GameObject CreateGameObject(Transform root, Driver driver)
    {
        GameObject driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject);
        driverStanding.GetComponent<TextMeshProUGUI>().text = driver.abbreviation;
        driverStanding.transform.SetParent(root);
        driverStanding.GetComponent<DriverStandingController>().SetDriver(driver);
        return driverStanding;
    }

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

    public short GetDriverPosition()
    {
        return driver.position;
    }
}
