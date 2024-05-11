using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Controllers.UI
{
    public class StandingsController : MonoBehaviour
    {
        [SerializeField] public int standingsRefreshTick = 3000;
        [SerializeField] public int doFirstStandingRefreshAfter = 30000; // First 30 secs are very messy telemetry wise

        private readonly SortedSet<Driver> _standings = new();
        private DriverStandingController _currentlySelectedDriverController;
        private GPController _gpController;
        private long _lastStandingsUpdateTime;
        private Timer _timer;

        private void Awake()
        {
            _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            _gpController = GameObject.FindGameObjectWithTag("GP").GetComponent<GPController>();
        }

        private void FixedUpdate()
        {
            var currentTime = _timer.GetTime();
            if (currentTime <= doFirstStandingRefreshAfter ||
                currentTime <= _lastStandingsUpdateTime + standingsRefreshTick) return;
            UpdateStandings();
            _lastStandingsUpdateTime = currentTime;
        }

        public void Add(Driver driver)
        {
            _standings.Add(driver);
        }

        public void HandleDriversStartingFromPits()
        {
            foreach (var rankedDriver in new List<Driver>(_standings).Where(rankedDriver => rankedDriver.Position == 0))
            {
                _standings.Remove(rankedDriver);
                rankedDriver.Position = (short)(_standings.Last().Position + 1);
                _standings.Add(rankedDriver);
            }

            Populate();
        }

        public void SetDriverSelected(DriverStandingController driverStandingController)
        {
            if (_currentlySelectedDriverController == driverStandingController)
            {
                _currentlySelectedDriverController.OnDriverSelected(false);
                _currentlySelectedDriverController = null;
            }
            else
            {
                if (_currentlySelectedDriverController != null) _currentlySelectedDriverController?.OnDriverSelected(false);
                _currentlySelectedDriverController = driverStandingController;
                _currentlySelectedDriverController.OnDriverSelected(true);
            }

            _gpController.GetCamera().GetComponent<DriverCameraController>().SetDriver(
                _currentlySelectedDriverController != null
                    ? _currentlySelectedDriverController.Driver
                    : null);
        }

        private void UpdateStandings()
        {
            // Listing all drivers who have someone behind them
            HashSet<Driver> inFrontOfSomeone = new();
            foreach (var driverAhead in _gpController.Drivers.Values
                         .Select(driver => driver.GetDriverAhead(_gpController.Drivers))
                         .Where(driverAhead => driverAhead != null))
                inFrontOfSomeone.Add(driverAhead);

            // Detecting PLast (the only driver without anyone behind them)
            Driver pLast = null;
            foreach (var driver in _gpController.Drivers.Values.Where(driver => !inFrontOfSomeone.Contains(driver)))
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
            while ((current = current.GetDriverAhead(_gpController.Drivers)) != null)
            {
                // Return if driverAhead is already in the list (bad telemetry)
                if (reverseStandings.Contains(current)) return;

                reverseStandings.Add(current);
            }

            // If the standings changed sizes, telemetry is bad
            if (_standings.Count != reverseStandings.Count) return;

            short position = 1;
            _standings.Clear();
            reverseStandings.Reverse();
            foreach (var driver in reverseStandings)
            {
                driver.Position = position++;
                _standings.Add(driver);
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
            var tempList = (from Transform child in transform select child.gameObject).ToList();
            transform.DetachChildren();
            IEnumerable<GameObject> targetOrder = tempList.OrderBy(gameObject =>
                gameObject.GetComponent<DriverStandingController>().GetDriverPosition());
            foreach (var targetGameObject in targetOrder) targetGameObject.transform.SetParent(transform);
        }

        private void Populate()
        {
            foreach (var driver in _standings) DriverStandingController.CreateGameObject(transform, driver);
        }
    }
}