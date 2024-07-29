using System;
using System.Collections.Generic;
using Models.Builders;
using UnityEngine;

namespace Models
{
    public class Driver : IComparable<Driver>
    {
        public readonly List<Lap> Laps;
        public readonly string Number, Abbreviation;
        private TelemetryEvent _lastVisitedTelemetryEvent;
        public int CurrentLap;
        public GameObject GameObject;
        public short Position;
        public Team Team;

        public Driver(string number, string abbreviation, short position, List<Lap> laps)
        {
            Number = number;
            Abbreviation = abbreviation;
            Position = position;
            Laps = laps;

            _lastVisitedTelemetryEvent = TelemetryEventBuilder.Instance.Build(abbreviation);
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
            return Abbreviation + " in position: " + Position + ". Behind: " + _lastVisitedTelemetryEvent.DriverAhead;
        }

        public Driver GetDriverAhead(Dictionary<short, Driver> drivers)
        {
            return _lastVisitedTelemetryEvent.DriverAhead == 0 ? null : drivers[_lastVisitedTelemetryEvent.DriverAhead];
        }

        public TelemetryEvent GetLastVisitedTelemetryEvent()
        {
            return _lastVisitedTelemetryEvent;
        }

        public void SetLastVisitedTelemetryEvent(TelemetryEvent lastVisitedTelemetryEvent)
        {
            _lastVisitedTelemetryEvent = lastVisitedTelemetryEvent;

            // Increment currentLap
            if (CurrentLap < Laps.Count && _lastVisitedTelemetryEvent.Time > Laps[CurrentLap].StartTime)
                CurrentLap++;
        }
    }
}