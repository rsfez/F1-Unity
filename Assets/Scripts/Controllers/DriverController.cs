using Controllers.Builders;
using Controllers.Interactors;
using Models;
using Models.Builders;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

namespace Controllers
{
    public class DriverController : MonoBehaviour, IBuildableController
    {
        private DriveInteractor _driveInteractor;
        private Driver _driver;
        private Spline _spline;
        private Timer _timer;

        private void Awake()
        {
            _spline = GameObject.FindWithTag("GP").AddComponent<SplineContainer>().Spline;
            _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().color = _driver.Team.Color;
            GetComponentInChildren<TextMeshPro>().text = _driver.Number;
        }

        private void Update()
        {
            _driveInteractor?.Drive();
        }

        public void Setup(params string[] args)
        {
            var abbreviation = args[0];
            var driver = DriverBuilder.Instance.Build(abbreviation);
            gameObject.name = abbreviation;
            _driver = driver;
            _driveInteractor = new DriveInteractor(transform, driver, _timer, _spline);
        }

        public Driver GetDriver()
        {
            return _driver;
        }
    }
}