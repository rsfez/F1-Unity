using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class StandingsController : MonoBehaviour
{
    [SerializeField] public int standingsRefreshTick = 3000;

    [SerializeField] public int doFirstStandingRefreshAfter = 30000; // First 30 secs are very messy telemetry wise
    private readonly SortedSet<Driver> standings = new();

    private DriverStandingController currentlySelectedDriverController;
    private GPController gpController;
    private long lastStandingsUpdateTime;
    private Timer timer;

    private void Awake()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        gpController = GameObject.FindGameObjectWithTag("GP").GetComponent<GPController>();
    }

    private void FixedUpdate()
    {
        var currentTime = timer.GetTime();
        if (currentTime <= doFirstStandingRefreshAfter ||
            currentTime <= lastStandingsUpdateTime + standingsRefreshTick) return;
        UpdateStandings();
        lastStandingsUpdateTime = currentTime;
    }

    public void Add(Driver driver)
    {
        standings.Add(driver);
    }

    public void HandleDriversStartingFromPits()
    {
        foreach (var rankedDriver in new List<Driver>(standings).Where(rankedDriver => rankedDriver.position == 0))
        {
            standings.Remove(rankedDriver);
            rankedDriver.position = (short)(standings.Last().position + 1);
            standings.Add(rankedDriver);
        }

        Populate();
    }

    public void SetDriverSelected(DriverStandingController driverStandingController)
    {
        if (currentlySelectedDriverController == driverStandingController)
        {
            currentlySelectedDriverController.OnDriverSelected(false);
            currentlySelectedDriverController = null;
        }
        else
        {
            if (currentlySelectedDriverController != null) currentlySelectedDriverController?.OnDriverSelected(false);
            currentlySelectedDriverController = driverStandingController;
            currentlySelectedDriverController.OnDriverSelected(true);
        }

        gpController.camera.GetComponent<DriverCameraController>().SetDriver(currentlySelectedDriverController != null
            ? currentlySelectedDriverController.driver
            : null);
    }

    private void UpdateStandings()
    {
        // Listing all drivers who have someone behind them
        HashSet<Driver> inFrontOfSomeone = new();
        foreach (var driverAhead in gpController.drivers.Values
                     .Select(driver => driver.GetDriverAhead(gpController.drivers))
                     .Where(driverAhead => driverAhead != null))
            inFrontOfSomeone.Add(driverAhead);

        // Detecting PLast (the only driver without anyone behind them)
        Driver pLast = null;
        foreach (var driver in gpController.drivers.Values.Where(driver => !inFrontOfSomeone.Contains(driver)))
        {
            if (pLast != null) return;
            pLast = driver;
        }

        if (pLast == null) return;

        List<Driver> reverseStandings = new()
        {
            pLast
        };
        var current = pLast;
        while ((current = current.GetDriverAhead(gpController.drivers)) != null)
        {
            // Return if driverAhead is already in the list (bad telemetry)
            if (reverseStandings.Contains(current)) return;

            reverseStandings.Add(current);
        }

        // If the standings changed sizes, telemetry is bad
        if (standings.Count != reverseStandings.Count) return;

        short position = 1;
        standings.Clear();
        reverseStandings.Reverse();
        foreach (var driver in reverseStandings)
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

        ReorderStandings();
    }

    private void ReorderStandings()
    {
        List<GameObject> tempList = new();
        foreach (Transform child in transform) tempList.Add(child.gameObject);
        transform.DetachChildren();
        IEnumerable<GameObject> targetOrder = tempList.OrderBy(gameObject =>
            gameObject.GetComponent<DriverStandingController>().GetDriverPosition());
        foreach (var targetGameObject in targetOrder) targetGameObject.transform.SetParent(transform);
    }

    private void Populate()
    {
        foreach (var driver in standings) DriverStandingController.CreateGameObject(transform, driver);
    }
}