using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    private String trigram = "XXX";
    private String number = "88";
    private Color color;

    void Start()
    {
        GetComponentInChildren<TextMeshPro>().text = number;
    }
}
