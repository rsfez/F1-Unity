using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class StandingsController : MonoBehaviour
{
    [SerializeField]
    public int standingsRefreshTick = 3000;
    [SerializeField]
    public int doFirstStandingRefreshAfter = 30000; // First 30 secs are very messy telemetry wise
    private long lastStandingsUpdateTime = 0;
    public SortedSet<Driver> standings = new();
    private Timer timer;
    private GPController gpController;

    void Awake()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        gpController = GameObject.FindGameObjectWithTag("GP").GetComponent<GPController>();
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

    public void Add(Driver driver)
    {
        standings.Add(driver);
    }

    public void HandleDriversStartingFromPits()
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
        Populate();
    }

    private void UpdateStandings()
    {
        // Listing all drivers who have someone behind them
        HashSet<Driver> inFrontOfSomeone = new();
        foreach (Driver driver in gpController.drivers.Values)
        {
            Driver driverAhead = driver.GetDriverAhead(gpController.drivers);
            if (driverAhead != null)
            {
                inFrontOfSomeone.Add(driverAhead);
            }
        }

        // Detecting PLast (the only driver without anyone behind them)
        Driver pLast = null;
        foreach (Driver driver in gpController.drivers.Values)
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
        while ((current = current.GetDriverAhead(gpController.drivers)) != null)
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
        // String standingsString = "";
        // foreach (Driver driver in standings)
        // {
        //     standingsString += driver.position + ". " + driver.abbreviation + " <- ";
        // }
        // Debug.Log(standingsString);
        Populate();
    }

    private void Populate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.GameObject());
        }
        foreach (Driver driver in standings)
        {
            GameObject driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject);
            driverStanding.GetComponent<TextMeshProUGUI>().text = driver.abbreviation;
            driverStanding.transform.SetParent(transform);
        }
    }
}
