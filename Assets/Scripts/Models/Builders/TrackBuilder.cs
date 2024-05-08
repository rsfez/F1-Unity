using System;
using UnityEngine;
using Utils;

namespace Models.Builders
{
    public class TrackBuilder : IBuildableFromCsv<Vector3[]>
    {
        private const string Path = "Data/2023/Japan/R/track";
        private static readonly Lazy<TrackBuilder> LazyInstance = new(() => new TrackBuilder());
        public static IBuildableFromCsv<Vector3[]> Instance => LazyInstance.Value;

        public Vector3[] Build(params string[] args)
        {
            var csv = CsvUtils.Parse(Path);
            var points = new Vector3[csv.Length];

            for (var y = 0; y < csv.Length; y++)
            {
                var csvLine = csv[y];
                var point = new Vector3(int.Parse(csvLine[0]), int.Parse(csvLine[1]), 0);
                points[y] = point;
            }

            return points;
        }
    }
}