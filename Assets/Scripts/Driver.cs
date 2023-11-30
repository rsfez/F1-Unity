using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    private String trigram = "XXX";
    private String number = "00";
    private Color color;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
        GetComponentInChildren<TextMeshPro>().text = number;
    }

    void SetParams(String trigram, String number, Color color)
    {
        this.trigram = trigram;
        this.number = number;
        this.color = color;
    }

    public static Driver Create(String trigram, String number, Color color)
    {
        GameObject driverGameObject = Instantiate(Resources.Load("Prefabs/Driver") as GameObject);
        Driver driver = driverGameObject.GetComponent<Driver>();
        driver.SetParams(trigram, number, color);
        return driver;
    }
}
