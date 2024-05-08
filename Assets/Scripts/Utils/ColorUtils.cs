using System.Globalization;
using UnityEngine;

public static class ColorUtils
{
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

        // Only use alpha if the string has enough characters
        if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
}