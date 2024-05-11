using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers.UI
{
    public class DriverStandingController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] public Color32 colorPrimary;
        [SerializeField] public Color32 colorSecondary;

        private StandingsController _standingsController;
        private TextMeshProUGUI _text;

        public Driver Driver;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _standingsController = GameObject.FindGameObjectWithTag("Standings").GetComponent<StandingsController>();
        }

        private void Update()
        {
            _text.text = GetDisplayedText();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _standingsController.SetDriverSelected(this);
        }

        public static void CreateGameObject(Transform root, Driver driver)
        {
            var driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject, root, true);
            driverStanding.GetComponent<DriverStandingController>().SetDriver(driver);
        }

        public void OnDriverSelected(bool selected)
        {
            _text.color = selected ? colorSecondary : colorPrimary;
        }

        private void SetDriver(Driver driver)
        {
            Driver = driver;
            name = GetDisplayedText();
        }

        public short GetDriverPosition()
        {
            return Driver.Position;
        }

        private string GetDisplayedText()
        {
            return Driver.Position + ". " + Driver.Abbreviation;
        }
    }
}