using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlAgentCron
{
    public class WeekDayParser : IWeekDayParser
    {
        public IEnumerable<int> GetDays(int frequencyInterval)
        {
            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                var currentWeekDay = (int) Math.Pow(2, (int) dayOfWeek);
                if ((frequencyInterval & currentWeekDay) == currentWeekDay)
                {
                    yield return dayOfWeek.GetHashCode();
                }
            }
        }

        public int GetFrequencyInterval(IEnumerable<int> days)
        {
            return days.Aggregate(0, (current, day) => current ^ (int) Math.Pow(2, day));
        }
    }
}