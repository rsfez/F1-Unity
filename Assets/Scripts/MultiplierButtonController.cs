using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierButtonController : MonoBehaviour
{
    [SerializeField] public short multiplier = 1;
    private Timer _timer;

    private void Awake()
    {
        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponentInChildren<TextMeshProUGUI>().text = "x" + multiplier;
    }

    private void OnClick()
    {
        _timer.SetMultiplier(multiplier);
    }
}