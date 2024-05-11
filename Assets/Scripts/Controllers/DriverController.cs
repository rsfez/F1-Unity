using Models;
using Models.Builders;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class DriverController : MonoBehaviour
    {
        public Driver Driver;

        private void Start()
        {
            GetComponent<SpriteRenderer>().color = Driver.Team.Color;
            GetComponentInChildren<TextMeshPro>().text = Driver.Number;
        }

        public static DriverController Create(string abbreviation)
        {
            var driverGameObject = Instantiate(Resources.Load("Prefabs/Driver") as GameObject);
            var driverController = driverGameObject.GetComponent<DriverController>();
            var driver = DriverBuilder.Instance.Build(abbreviation);
            driverGameObject.name = abbreviation;
            driverController.Driver = driver;
            return driverController;
        }
    }
}