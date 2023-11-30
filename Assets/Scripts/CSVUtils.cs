using UnityEngine;
using System.IO;
using System.Linq;

class CSVUtils
{
    public static string[][] Parse(string path)
    {
        string[] lines = ReadCsvFile(path);
        string[][] grid = SplitCsvGrid(lines);

        return grid;
    }

    static string[] ReadCsvFile(string path)
    {
        try
        {
            string fileData = File.ReadAllText(path);
            return fileData.Split('\n');
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading CSV file at path: " + path + "\n" + e.Message);
            return new string[0];
        }
    }

    static public string[][] SplitCsvGrid(string[] lines)
    {
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(",");
            width = Mathf.Max(width, row.Length);
        }

        string[][] outputGrid = new string[lines.Length - 2][];
        for (int y = 1; y < lines.Length - 1; y++)
        {
            outputGrid[y - 1] = lines[y].Split(",").Skip(1).ToArray();
        }

        return outputGrid;
    }
}