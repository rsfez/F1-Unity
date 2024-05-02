using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPController : MonoBehaviour
{
    public readonly Dictionary<short, Driver> drivers = new();
    new public GameObject camera;
    private readonly HashSet<Team> teams = new();

    void Awake()
    {
        camera = GetComponentInChildren<Camera>().gameObject;
    }

    void Start()
    {
        StartCoroutine(LoadDrivers());
    }

    private IEnumerator LoadDrivers()
    {
        StandingsController standings = GameObject.FindWithTag("Standings").GetComponent<StandingsController>();
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            DriverController driverController = DriverController.Create(textAsset.name);
            Driver driver = driverController.driver;
            GameObject driverGameObject = driverController.gameObject;
            driver.gameObject = driverGameObject;
            drivers[short.Parse(driver.number)] = driver;
            AssignDriverToTeam(driver);
            standings.Add(driver);
        }
        standings.HandleDriversStartingFromPits();
        yield return null;
    }

    private void AssignDriverToTeam(Driver driver)
    {
        if (teams.Contains(driver.team))
        {
            teams.TryGetValue(driver.team, out Team team);
            driver.team = team;
        }
        else
        {
            teams.Add(driver.team);
        }
    }
}
