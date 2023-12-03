using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
public class Team
{
    public readonly string name, id;
    public readonly Color color;

    public readonly HashSet<Driver> drivers = new HashSet<Driver>();

    public Team(string name, string id, Color color)
    {
        this.name = name;
        this.id = id;
        this.color = color;
    }

    public static Team FromCSV(string[][] csv)
    {
        string name = csv[4][0];
        string id = csv[6][0];
        Color color = ColorUtils.HexToColor(csv[5][0]);
        return new Team(name, id, color);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Team)obj;
        return id == other.id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}