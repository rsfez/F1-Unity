using System;
using System.Collections.Generic;


public class Driver : IComparable<Driver>
{
    public readonly string number, abbreviation;
    public short position;
    public Team team;
    public TelemetryEvent lastVisitedTelemetryEvent;

    public Driver(string number, string abbreviation, short position)
    {
        this.number = number;
        this.abbreviation = abbreviation;
        this.position = position;

        lastVisitedTelemetryEvent = TelemetryEvent.LoadTelemetryEventsFromCSV(abbreviation);
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

    public override string ToString()
    {
        return abbreviation + " in position: " + position + ". Behind: " + lastVisitedTelemetryEvent.driverAhead;
    }

    public int CompareTo(Driver other)
    {
        if (other == null) return 1;
        return position.CompareTo(other.position);
    }

    public short GetDriverAheadNumber()
    {
        return lastVisitedTelemetryEvent.driverAhead;
    }

    public Driver GetDriverAhead(Dictionary<short, Driver> drivers)
    {
        if (lastVisitedTelemetryEvent.driverAhead == 0) return null;
        return drivers[lastVisitedTelemetryEvent.driverAhead];
    }
}