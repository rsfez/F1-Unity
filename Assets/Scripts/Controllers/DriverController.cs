using Controllers.Interactors;
using Models;
using Models.Builders;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

namespace Controllers
{
    public class DriverController : MonoBehaviour
    {
        private DriveInteractor _driveInteractor;
        private Driver _driver;
        private Spline _spline;
        private Timer _timer;

        private void Start()
        {
            GetComponent<SpriteRenderer>().color = _driver.Team.Color;
            GetComponentInChildren<TextMeshPro>().text = _driver.Number;
        }

        private void Update()
        {
            _driveInteractor?.Drive();
        }

        public Driver GetDriver()
        {
            return _driver;
        }

        public static DriverController Create(string abbreviation)
        {
            var driverGameObject = Instantiate(Resources.Load("Prefabs/Driver") as GameObject);
            var driverController = driverGameObject.GetComponent<DriverController>();
            var driver = DriverBuilder.Instance.Build(abbreviation);
            var timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            var spline = GameObject.FindWithTag("GP").AddComponent<SplineContainer>().Spline;
            driverGameObject.name = abbreviation;
            driverController._driver = driver;
            driverController._driveInteractor = new DriveInteractor(driverGameObject.transform, driver, timer, spline);
            return driverController;
        }
    }
}