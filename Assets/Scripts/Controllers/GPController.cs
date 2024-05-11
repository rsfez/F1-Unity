using System.Collections;
using System.Collections.Generic;
using Controllers.UI;
using Models;
using UnityEngine;

namespace Controllers
{
    public class GPController : MonoBehaviour
    {
        private readonly HashSet<Team> _teams = new();
        public readonly Dictionary<short, Driver> Drivers = new();
        private GameObject _camera;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>().gameObject;
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
                var driverController = DriverController.Create(textAsset.name);
                var driver = driverController.Driver;
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