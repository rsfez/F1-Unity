using System.Collections;
using System.Collections.Generic;
using Controllers.Builders;
using Controllers.UI;
using Models;
using UnityEngine;

namespace Controllers
{
    public class GpController : MonoBehaviour
    {
        private readonly HashSet<Team> _teams = new();
        public readonly Dictionary<short, Driver> Drivers = new();
        private GameObject _camera;
        private Timer _timer;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>().gameObject;
            _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        }

        private void Start()
        {
            StartCoroutine(LoadDrivers());
        }

        public GameObject GetCamera()
        {
            return _camera;
        }

        private IEnumerator LoadDrivers()
        {
            var standings = GameObject.FindWithTag("Standings").GetComponent<StandingsController>();
            foreach (var textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
            {
                var driverController = DriverControllerBuilder.Instance.Build(textAsset.name);
                var driver = driverController.GetDriver();
                _timer.TryUpdateTotalLaps(driver.Laps.Count);
                var driverGameObject = driverController.gameObject;
                driver.GameObject = driverGameObject;
                Drivers[short.Parse(driver.Number)] = driver;
                AssignDriverToTeam(driver);
                standings.Add(driver);
            }

            standings.HandleDriversStartingFromPits();
            yield return null;
        }

        private void AssignDriverToTeam(Driver driver)
        {
            if (_teams.Add(driver.Team)) return;
            _teams.TryGetValue(driver.Team, out var team);
            driver.Team = team;
        }
    }
}