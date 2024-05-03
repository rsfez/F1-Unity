using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DriverStandingController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public Color32 colorPrimary;
    [SerializeField]
    public Color32 colorSecondary;
    public Driver driver;
    private TextMeshProUGUI text;
    private StandingsController standingsController;

    public static GameObject CreateGameObject(Transform root, Driver driver)
    {
        var driverStanding = Instantiate(Resources.Load("Prefabs/DriverStanding") as GameObject);
        driverStanding.GetComponent<TextMeshProUGUI>().text = driver.abbreviation;
        driverStanding.transform.SetParent(root);
        driverStanding.GetComponent<DriverStandingController>().SetDriver(driver);
        return driverStanding;
    }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        standingsController = GameObject.FindGameObjectWithTag("Standings").GetComponent<StandingsController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        standingsController.SetDriverSelected(this);
    }

    public void OnDriverSelected(bool selected)
    {
        text.color = selected ? colorSecondary : colorPrimary;
    }

    private void SetDriver(Driver driver)
    {
        this.driver = driver;
        name = driver.position + ". " + driver.abbreviation;
    }

    public short GetDriverPosition()
    {
        return driver.position;
    }
}
