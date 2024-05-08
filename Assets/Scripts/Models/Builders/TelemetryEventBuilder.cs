using System;
using UnityEngine;
using Utils;

namespace Models.Builders
{
    public class TelemetryEventBuilder : IBuildableFromCsv<TelemetryEvent>
    {
        private static readonly Lazy<TelemetryEventBuilder> LazyInstance = new(() => new TelemetryEventBuilder());
        public static IBuildableFromCsv<TelemetryEvent> Instance => LazyInstance.Value;

        public TelemetryEvent Build(params string[] args)
        {
            var driverAbbreviation = args[0];
            var csv = CsvUtils.Parse("Data/2023/Japan/R/telemetry/" + driverAbbreviation);
            TelemetryEvent previousTelemetryEvent = null;
            TelemetryEvent firstTelemetryEvent = null;
            foreach (var line in csv)
            {
                var currentTelemetryEvent = GetFromCsvLine(line);
                currentTelemetryEvent.Previous = previousTelemetryEvent;
                if (previousTelemetryEvent != null) previousTelemetryEvent.Next = currentTelemetryEvent;
                previousTelemetryEvent = currentTelemetryEvent;
                firstTelemetryEvent ??= currentTelemetryEvent;
            }

            return firstTelemetryEvent;
        }

        private static TelemetryEvent GetFromCsvLine(string[] line)
        {
            short driverAhead;
            driverAhead = short.TryParse(line[9], out driverAhead) ? driverAhead : (short)0;
            return new TelemetryEvent(
                new Vector3(float.Parse(line[7]), float.Parse(line[8]), 0),
                int.Parse(line[0]),
                driverAhead
            );
        }
    }
}