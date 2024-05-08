using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutiplicatorButtonController : MonoBehaviour
{
    public short mutiplicator = 1;
    private Timer timer;

    private void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponentInChildren<TextMeshProUGUI>().text = "x" + mutiplicator;
    }

    private void OnClick()
    {
        timer.SetMulitiplicator(mutiplicator);
    }
}
