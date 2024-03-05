using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LeftPanelController : MonoBehaviour
{
    public float width = 600f;

    public VisualTreeAsset driverStanding;

    private UIDocument root;
    private Label timerLabel;
    private Timer timer;
    private Button start, x1, x2, x4, x8;
    private ListView standingsListView;
    private List<Driver> standings;

    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        root = GetComponent<UIDocument>();
        // StartCoroutine(SetupWidth());
        root.rootVisualElement.style.width = width;
        timerLabel = root.rootVisualElement.Q("Timer") as Label;
        start = root.rootVisualElement.Q("StartPause") as Button;
        start.RegisterCallback<ClickEvent>(OnStartClick);

        x1 = root.rootVisualElement.Q("x1") as Button;
        x1.RegisterCallback(GetOnSpeedButtonClick(1));
        x2 = root.rootVisualElement.Q("x2") as Button;
        x2.RegisterCallback(GetOnSpeedButtonClick(2));

        x4 = root.rootVisualElement.Q("x4") as Button;
        x4.RegisterCallback(GetOnSpeedButtonClick(4));

        x8 = root.rootVisualElement.Q("x8") as Button;
        x8.RegisterCallback(GetOnSpeedButtonClick(8));

        standingsListView = root.rootVisualElement.Q("Standings") as ListView;
        standingsListView.makeItem = () => driverStanding.CloneTree();
        standingsListView.bindItem = (element, i) =>
        {
            (element.Q("Abbreviation") as Label).text = (standingsListView.itemsSource[i] as Driver).abbreviation;
        };
        standingsListView.itemsSource = standings;
    }

    void Update()
    {
        timerLabel.text = timer.GetTimerText();
    }

    public void UpdateStandings(SortedSet<Driver> standings)
    {
        this.standings = standings.ToList();
    }

    private void OnStartClick(ClickEvent clickEvent)
    {
        if (timer.IsRunning())
        {
            timer.PauseTimer();
            start.text = "Start";
        }
        else
        {
            timer.StartTimer();
            start.text = "Pause";
        }
    }

    private EventCallback<ClickEvent> GetOnSpeedButtonClick(short speed)
    {
        return (ClickEvent clickEvent) => { timer.SetMulitiplicator(speed); };
    }

    private IEnumerator SetupWidth()
    {
        while (float.IsNaN(root.rootVisualElement.resolvedStyle.width))
        {
            yield return null;
        }
        root.rootVisualElement.style.width = width;
        Debug.Log("Computed width: " + root.rootVisualElement.resolvedStyle.width);
    }
}
