using Models;
using TMPro;
using UnityEngine;

public class DriverController : MonoBehaviour
{
    public Driver driver;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = driver.team.color;
        GetComponentInChildren<TextMeshPro>().text = driver.number;
    }

    public static DriverController Create(string abbreviation)
    {
        var driverGameObject = Instantiate(Resources.Load("Prefabs/Driver") as GameObject);
        var driverController = driverGameObject.GetComponent<DriverController>();
        var driver = Driver.FromCSV(abbreviation);
        driverGameObject.name = abbreviation;
        driverController.driver = driver;
        return driverController;
    }
}
