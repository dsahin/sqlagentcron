using System;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron
{
    public class SqlAgentJobSchedule
    {
        public DateTime ActiveEndDate { get; set; }
        public TimeSpan ActiveEndTimeOfDay { get; set; }
        public DateTime ActiveStartDate { get; set; }
        public TimeSpan ActiveStartTimeOfDay { get; set; }

        public FrequencyTypes FrequencyTypes { get; set; }
        public int FrequencyInterval { get; set; }
        public int FrequencyRecurrenceFactor { get; set; }
        public FrequencyRelativeIntervals FrequencyRelativeIntervals { get; set; }
        public int FrequencySubDayInterval { get; set; }
        public FrequencySubDayTypes FrequencySubDayTypes { get; set; }
    }
}