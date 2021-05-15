using System;
using System.Linq;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public class TimeUtils
    {
        public const string DateTimeFormat = "yyyy-dd-M--HH-mm-ss"; // e.g. 2020-09-16--12-34-10

        public string SecondPrecision { get; }
        public string DayPrecision { get; }

        private TimeUtils(string dayPrecision, string secondPrecision)
        {
            DayPrecision = dayPrecision;
            SecondPrecision = secondPrecision;
        }

        public static TimeUtils CreateTimeStamp()
        {
            var secondPrecision = DateTime.Now.ToString(DateTimeFormat);
            var dayPrecision = secondPrecision.Split("--").First();
            
            return new TimeUtils(dayPrecision, secondPrecision);
        }
    }
}