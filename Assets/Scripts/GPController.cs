using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPController : MonoBehaviour
{
    private HashSet<Team> teams = new HashSet<Team>();
    public SortedSet<Driver> ranking = new SortedSet<Driver>();

    void Start()
    {
        StartCoroutine(LoadDrivers());
    }

    private IEnumerator LoadDrivers()
    {
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("Data/2023/Japan/R/drivers/"))
        {
            DriverController driverController = DriverController.Create(textAsset.name);
            Driver driver = driverController.driver;
            if (teams.Contains(driver.team))
            {
                teams.TryGetValue(driver.team, out Team team);
                driver.team = team;
            }
            else
            {
                teams.Add(driver.team);
            }
            ranking.Add(driver);
        }
        yield return null;
    }
}
