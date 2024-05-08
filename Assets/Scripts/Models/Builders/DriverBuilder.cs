using System;
using Utils;

namespace Models.Builders
{
    public class DriverBuilder : IBuildableFromCsv<Driver>
    {
        private const string PathPrefix = "Data/2023/Japan/R/drivers/";
        private static readonly Lazy<DriverBuilder> LazyInstance = new(() => new DriverBuilder());
        public static IBuildableFromCsv<Driver> Instance => LazyInstance.Value;

        public Driver Build(params string[] args)
        {
            var abbreviation = args[0];
            var csv = CsvUtils.Parse(PathPrefix + abbreviation);
            var number = csv[0][0];
            var position = (short)float.Parse(csv[14][0]);

            var teamName = csv[4][0];
            var teamId = csv[6][0];
            var teamColor = ColorUtils.HexToColor(csv[5][0]);
            var team = new Team(teamName, teamId, teamColor);

            Driver driver = new(
                number,
                abbreviation,
                position
            )
            {
                Team = team
            };
            return driver;
        }
    }
}