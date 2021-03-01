using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public class DateFormat : IDateFormat
    {
        readonly string _timeZone;
        public DateFormat(string timeZone)
        {
            _timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        }
        public DateTime GetDate()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZone);
            DateTime cstTime = TimeZoneInfo.ConvertTime(DateTime.Now, cstZone);
            return cstTime;
        }
    }
}
