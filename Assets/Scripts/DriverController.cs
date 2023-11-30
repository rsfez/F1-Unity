using System;
using TMPro;
using UnityEngine;

public class DriverController : MonoBehaviour
{
    public Driver driver;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = driver.team.color;
        GetComponentInChildren<TextMeshPro>().text = driver.number;
    }

    public static DriverController Create(String abbreviation)
    {
        GameObject driverGameObject = Instantiate(Resources.Load("Prefabs/Driver") as GameObject);
        DriverController driverController = driverGameObject.GetComponent<DriverController>();
        Driver driver = Driver.FromCSV(abbreviation);
        driverController.driver = driver;
        return driverController;
    }
}
