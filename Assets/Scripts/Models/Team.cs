using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Team
    {
        private readonly string _id;
        public readonly Color Color;

        public readonly HashSet<Driver> Drivers = new();
        public readonly string Name;

        public Team(string name, string id, Color color)
        {
            Name = name;
            _id = id;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (Team)obj;
            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}