using System;
using System.Collections;

public class Driver : IComparable<Driver>
{
    public readonly string number, abbreviation;
    public short position;
    public Team team;
    public readonly TelemetryEvent currentTelemetryEvent;

    public Driver(string number, string abbreviation, short position)
    {
        this.number = number;
        this.abbreviation = abbreviation;
        this.position = position;

        currentTelemetryEvent = LoadTelemetryEventsFromCSV();
    }

    public static Driver FromCSV(string abbreviation)
    {
        string[][] csv = CSVUtils.Parse("Data/2023/Japan/R/drivers/" + abbreviation);
        string number = csv[0][0];
        short position = (short)float.Parse(csv[14][0]);
        Driver driver = new Driver(
            number,
            abbreviation,
            position
        );
        driver.team = Team.FromCSV(csv);
        return driver;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Driver)obj;
        return number == other.number;
    }

    public override int GetHashCode()
    {
        return number.GetHashCode();
    }

    public int CompareTo(Driver other)
    {
        if (other == null) return 1;
        return position.CompareTo(other.position);
    }

    private TelemetryEvent LoadTelemetryEventsFromCSV()
    {
        string[][] csv = CSVUtils.Parse("Data/2023/Japan/R/telemetry/" + abbreviation);
        TelemetryEvent previousTelemetryEvent = null;
        TelemetryEvent firstTelemetryEvent = null;
        foreach (string[] line in csv)
        {
            TelemetryEvent currentTelemetryEvent = TelemetryEvent.GetFromCSVLine(line);
            currentTelemetryEvent.previous = previousTelemetryEvent;
            if (previousTelemetryEvent != null) {
                previousTelemetryEvent.next = currentTelemetryEvent;
            }
            previousTelemetryEvent = currentTelemetryEvent;
            firstTelemetryEvent ??= currentTelemetryEvent;
        }
        return firstTelemetryEvent;
    }
}