using System.Globalization;
using UnityEngine;

namespace Utils
{
    public static class ColorUtils
    {
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");
            hex = hex.Replace("#", "");
            byte a = 255;
            var r = byte.Parse(hex[..2], NumberStyles.HexNumber);
            var g = byte.Parse(hex[2..4], NumberStyles.HexNumber);
            var b = byte.Parse(hex[4..6], NumberStyles.HexNumber);

            // Only use alpha if the string has enough characters
            if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);

            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}