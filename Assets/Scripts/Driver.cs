using System.Collections;
using UnityEngine;

public class Driver
{
    public readonly string number, abbreviation, id;
    public short position;
    public Team team;
    public readonly TelemetrySession session;

    public Driver(string number, string abbreviation, string id, short position)
    {
        this.number = number;
        this.abbreviation = abbreviation;
        this.id = id;
        this.position = position;

        TelemetryEvent[] telemetryEvents = LoadTelemetryEventsFromCSV();
        session = new TelemetrySession(telemetryEvents);
    }

    public static Driver FromCSV(string abbreviation)
    {
        string[][] csv = CSVUtils.Parse("Data/2023/Japan/R/drivers/" + abbreviation);
        string number = csv[0][0];
        string id = csv[3][0];
        short position = (short)float.Parse(csv[12][0]);
        Driver driver = new Driver(
            number,
            abbreviation,
            id,
            position
        );
        driver.team = Team.FromCSV(csv);
        return driver;
    }

    private TelemetryEvent[] LoadTelemetryEventsFromCSV()
    {
        string[][] csv = CSVUtils.Parse("Data/2023/Japan/R/telemetry/" + abbreviation);
        ArrayList telemetryEvents = new ArrayList();
        foreach (string[] line in csv)
        {
            telemetryEvents.Add(TelemetryEvent.GetFromCSVLine(line));
        }
        return (TelemetryEvent[])telemetryEvents.ToArray(typeof(TelemetryEvent));
    }
}