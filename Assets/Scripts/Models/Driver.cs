using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Driver : IComparable<Driver>
{
    public readonly string number, abbreviation;
    public GameObject gameObject;
    public TelemetryEvent lastVisitedTelemetryEvent;
    public short position;
    public Team team;

    private Driver(string number, string abbreviation, short position)
    {
        this.number = number;
        this.abbreviation = abbreviation;
        this.position = position;

        lastVisitedTelemetryEvent = TelemetryEvent.LoadTelemetryEventsFromCSV(abbreviation);
    }

    public int CompareTo(Driver other)
    {
        return other == null ? 1 : position.CompareTo(other.position);
    }

    public static Driver FromCSV(string abbreviation)
    {
        var csv = CsvUtils.Parse("Data/2023/Japan/R/drivers/" + abbreviation);
        var number = csv[0][0];
        var position = (short)float.Parse(csv[14][0]);
        Driver driver = new(
            number,
            abbreviation,
            position
        )
        {
            team = Team.FromCSV(csv)
        };
        return driver;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

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

    public short GetDriverAheadNumber()
    {
        return lastVisitedTelemetryEvent.driverAhead;
    }

    public Driver GetDriverAhead(Dictionary<short, Driver> drivers)
    {
        return lastVisitedTelemetryEvent.driverAhead == 0 ? null : drivers[lastVisitedTelemetryEvent.driverAhead];
    }
}