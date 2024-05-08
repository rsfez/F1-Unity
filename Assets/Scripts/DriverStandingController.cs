using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DriverStandingController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Color32 colorPrimary;

    [SerializeField] public Color32 colorSecondary;

    public Driver driver;
    private StandingsController standingsController;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        standingsController = GameObject.FindGameObjectWithTag("Standings").GetComponent<StandingsController>();
    }

    private void Update()
    {
        text.text = GetDisplayedText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        standingsController.SetDriverSelected(this);
    }

    public static GameObject CreateGameObject(Transform root, Driver driver)
    {
        var driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject);
        // driverStanding.GetComponent<TextMeshProUGUI>().text = ;
        driverStanding.transform.SetParent(root);
        driverStanding.GetComponent<DriverStandingController>().SetDriver(driver);
        return driverStanding;
    }

    public void OnDriverSelected(bool selected)
    {
        text.color = selected ? colorSecondary : colorPrimary;
    }

    private void SetDriver(Driver driver)
    {
        this.driver = driver;
        name = GetDisplayedText();
    }

    public short GetDriverPosition()
    {
        return driver.position;
    }

    private string GetDisplayedText()
    {
        return driver.position + ". " + driver.abbreviation;
    }
}