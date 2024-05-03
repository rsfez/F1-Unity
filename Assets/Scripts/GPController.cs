using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPController : MonoBehaviour
{
    public new GameObject camera;
    public readonly Dictionary<short, Driver> drivers = new();
    private readonly HashSet<Team> teams = new();

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>().gameObject;
    }

    private void Start()
    {
        StartCoroutine(LoadDrivers());
    }

    private IEnumerator LoadDrivers()
    {
        var standings = GameObject.FindWithTag("Standings").GetComponent<StandingsController>();
        foreach (var textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            var driverController = DriverController.Create(textAsset.name);
            var driver = driverController.driver;
            var driverGameObject = driverController.gameObject;
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
            teams.TryGetValue(driver.team, out var team);
            driver.team = team;
        }
        else
        {
            teams.Add(driver.team);
        }
    }
}