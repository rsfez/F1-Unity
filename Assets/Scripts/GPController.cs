using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPController : MonoBehaviour
{
    private HashSet<Team> teams = new HashSet<Team>();
    public SortedSet<Driver> standings = new SortedSet<Driver>();

    void Start()
    {
        StartCoroutine(LoadDrivers());
    }

    private IEnumerator LoadDrivers()
    {
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            DriverController driverController = DriverController.Create(textAsset.name);
            Driver driver = driverController.driver;
            AssignDriverToTeam(driver);
            standings.Add(driver);
        }
        HandleDriversStartingFromPits();
        PopulateStandings();
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

    private void HandleDriversStartingFromPits()
    {
        foreach (Driver rankedDriver in new List<Driver>(standings))
        {
            if (rankedDriver.position == 0)
            {
                standings.Remove(rankedDriver);
                rankedDriver.position = (short)(standings.Last<Driver>().position + 1);
                standings.Add(rankedDriver);
            }
        }
    }

    private void PopulateStandings()
    {
        LeftPanelController leftPanelController = GameObject.FindWithTag("LeftPanel").GetComponent<LeftPanelController>();
        leftPanelController.UpdateStandings(standings);
    }
}
