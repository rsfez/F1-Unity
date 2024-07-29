using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Models.Builders
{
    public class LapsBuilder : IBuildableFromCsv<List<Lap>>
    {
        private const string Path = "Data/2023/Japan/R/session";
        private static readonly Lazy<LapsBuilder> LazyInstance = new(() => new LapsBuilder());

        private readonly Dictionary<string, List<string[]>> entries = new();
        public static IBuildableFromCsv<List<Lap>> Instance => LazyInstance.Value;

        public List<Lap> Build(params string[] args)
        {
            var abbreviation = args[0];

            if (entries.Count == 0) LoadCsv();

            var lapsStrings = entries[abbreviation];

            var laps = lapsStrings.Select(lapString => new Lap(long.Parse(lapString[0]), long.Parse(lapString[3])))
                .ToList();
            var firstLapStartTime = laps[0].StartTime;
            return laps.Select(lap => lap.CopyWithAdjustedStartTime(firstLapStartTime)).ToList();
        }

        private void LoadCsv()
        {
            var csv = CsvUtils.Parse(Path);
            foreach (var line in csv)
            {
                var lineAbbreviation = line[1];

                entries[lineAbbreviation] = entries.GetValueOrDefault(lineAbbreviation, new List<string[]>());
                entries[lineAbbreviation].Add(line);
            }
        }
    }
}