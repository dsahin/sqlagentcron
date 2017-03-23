using System.Collections.Generic;

namespace SqlAgentCron
{
    public interface IWeekDayParser
    {
        IEnumerable<int> GetDays(int frequencyInterval);
        int GetFrequencyInterval(IEnumerable<int> days);
    }
}