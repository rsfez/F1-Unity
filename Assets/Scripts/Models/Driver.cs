using System;
using System.Collections.Generic;
using Models.Builders;
using UnityEngine;

namespace Models
{
    public class Driver : IComparable<Driver>
    {
        public readonly string Number, Abbreviation;
        public GameObject GameObject;
        public TelemetryEvent LastVisitedTelemetryEvent;
        public short Position;
        public Team Team;

        public Driver(string number, string abbreviation, short position)
        {
            Number = number;
            Abbreviation = abbreviation;
            Position = position;

            LastVisitedTelemetryEvent = TelemetryEventBuilder.Instance.Build(abbreviation);
        }

        public int CompareTo(Driver other)
        {
            return other == null ? 1 : Position.CompareTo(other.Position);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (Driver)obj;
            return Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public override string ToString()
        {
            return Abbreviation + " in position: " + Position + ". Behind: " + LastVisitedTelemetryEvent.DriverAhead;
        }

        public Driver GetDriverAhead(Dictionary<short, Driver> drivers)
        {
            return LastVisitedTelemetryEvent.DriverAhead == 0 ? null : drivers[LastVisitedTelemetryEvent.DriverAhead];
        }
    }
}