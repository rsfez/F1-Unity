using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GPController : MonoBehaviour
{
    [SerializeField]
    public int standingsRefreshTick = 3000;
    [SerializeField]
    public int doFirstStandingRefreshAfter = 30000; // First 30 secs are very messy telemetry wise
    private long lastStandingsUpdateTime = 0;
    public readonly Dictionary<short, Driver> drivers = new();
    private readonly HashSet<Team> teams = new();
    public SortedSet<Driver> standings = new();
    private Timer timer;

    void Start()
    {
        StartCoroutine(LoadDrivers());
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    void FixedUpdate()
    {
        long currentTime = timer.GetTime();
        if (currentTime > doFirstStandingRefreshAfter && currentTime > lastStandingsUpdateTime + standingsRefreshTick)
        {
            UpdateStandings();
            lastStandingsUpdateTime = currentTime;
        }
    }

    private IEnumerator LoadDrivers()
    {
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            DriverController driverController = DriverController.Create(textAsset.name);
            Driver driver = driverController.driver;
            drivers[short.Parse(driver.number)] = driver;
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

    private void UpdateStandings()
    {
        // Listing all drivers who have someone behind them
        HashSet<Driver> inFrontOfSomeone = new();
        foreach (Driver driver in drivers.Values)
        {
            Driver driverAhead = driver.GetDriverAhead(drivers);
            if (driverAhead != null)
            {
                inFrontOfSomeone.Add(driverAhead);
            }
        }

        // Detecting PLast (the only one without anyone behind them)
        Driver pLast = null;
        foreach (Driver driver in drivers.Values)
        {
            if (!inFrontOfSomeone.Contains(driver))
            {
                if (pLast != null) return;
                else pLast = driver;
            }
        }
        if (pLast == null) return;
        List<Driver> reverseStandings = new()
        {
            pLast
        };
        Driver current = pLast;
        while ((current = current.GetDriverAhead(drivers)) != null)
        {
            // Break if P1 Found
            if (current == null) break;

            // Return if driverAhead is already in the list (bad telemetry)
            if (reverseStandings.Contains(current)) return;

            reverseStandings.Add(current);
        }

        // If the standings changed sizes, telemetry is bad
        if (standings.Count != reverseStandings.Count) return;

        short position = 1;
        standings.Clear();
        reverseStandings.Reverse();
        foreach (Driver driver in reverseStandings)
        {
            driver.position = position++;
            standings.Add(driver);
        }
        String standingsString = "";
        foreach (Driver driver in standings)
        {
            standingsString += driver.position + ". " + driver.abbreviation + " <- ";
        }
        // Debug.Log(standingsString);
        PopulateStandings();
    }

    private void PopulateStandings()
    {

        GameObject standingsUI = GameObject.FindWithTag("Standings");
        foreach (Transform child in standingsUI.transform)
        {
            Destroy(child.GameObject());
        }
        foreach (Driver driver in standings)
        {
            GameObject driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject);
            driverStanding.GetComponent<TextMeshProUGUI>().text = driver.abbreviation;
            driverStanding.transform.SetParent(standingsUI.transform);
        }
    }
}
