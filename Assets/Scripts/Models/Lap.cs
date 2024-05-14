namespace Models
{
    public class Lap
    {
        public readonly long StartTime;
        public readonly long Time;

        internal Lap(long startTime, long time)
        {
            StartTime = startTime;
            Time = time;
        }

        internal Lap CopyWithAdjustedStartTime(long adjustedStartTime)
        {
            return new Lap(StartTime - adjustedStartTime, Time);
        }
    }
}