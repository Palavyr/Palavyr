using System;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public class TimeUtils
    {
        public string SecondPrecision { get; }
        public string DayPrecision { get; }

        private TimeUtils(string dayPrecision, string secondPrecision)
        {
            DayPrecision = dayPrecision;
            SecondPrecision = secondPrecision;
        }

        public static DateTime CreateNewTimeStamp()
        {
            return DateTime.UtcNow;
        }
    }
}