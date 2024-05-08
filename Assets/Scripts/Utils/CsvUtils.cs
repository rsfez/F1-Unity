using System;
using System.Linq;
using UnityEngine;

namespace Utils
{
    internal static class CsvUtils
    {
        public static string[][] Parse(string path)
        {
            var lines = ReadCsvFile(path);
            var grid = SplitCsvGrid(lines);

            return grid;
        }

        private static string[] ReadCsvFile(string path)
        {
            try
            {
                var fileData = Resources.Load<TextAsset>(path).text;
                return fileData.Split('\n');
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading CSV file at path: " + path + "\n" + e.Message);
                return Array.Empty<string>();
            }
        }

        private static string[][] SplitCsvGrid(string[] lines)
        {
            var outputGrid = new string[lines.Length - 2][];
            for (var y = 1; y < lines.Length - 1; y++) outputGrid[y - 1] = lines[y].Split(",").Skip(1).ToArray();

            return outputGrid;
        }
    }
}