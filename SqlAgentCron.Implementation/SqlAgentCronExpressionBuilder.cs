using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron.Implementation
{
    public class SqlAgentCronExpressionBuilder : ICronExpressionBuilder
    {
        public string GetExpression(ISqlAgentJobSchedule sqlAgentJobSchedule)
        {
            var values = GetValues((SqlAgentJobSchedule)sqlAgentJobSchedule).ToArray();
            return string.Join(" ", values);
        }

        public static IEnumerable<string> GetValues(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Unknown)
            {
                yield break;
            }
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Once)
            {
                yield return schedule.ActiveStartTimeOfDay.Minutes.ToString();
                yield return schedule.ActiveStartTimeOfDay.Hours.ToString();
            }
            else
            {
                if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Second)
                {
                    yield return $@"*/{schedule.FrequencySubDayInterval}";
                }

                bool returnedMinute = false;
                if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Minute)
                {
                    returnedMinute = true;
                    yield return $@"*/{schedule.FrequencySubDayInterval}";
                }

                bool returnedHour = false;
                if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Hour)
                {
                    if (!returnedMinute)
                    {
                        returnedMinute = true;
                        yield return schedule.ActiveStartTimeOfDay.Minutes.ToString();
                    }
                    returnedHour = true;
                    if (schedule.FrequencySubDayInterval > 1)
                    {
                        yield return $@"*/{schedule.FrequencySubDayInterval}";
                    }
                    else
                    {
                        yield return schedule.ActiveStartTimeOfDay.Hours.ToString();
                    }
                }

                if (!returnedMinute)
                {
                    yield return "*";
                }
                if (!returnedHour)
                {
                    yield return "*";
                }
            }

            bool returnedDaily = false;
            if (schedule.FrequencyTypes == FrequencyTypes.Daily && schedule.FrequencyInterval > 1)
            {
                returnedDaily = true;
                yield return $@"*/{schedule.FrequencyInterval}";
            }
            if (schedule.FrequencyTypes == FrequencyTypes.Monthly && schedule.FrequencyInterval > 1)
            {
                returnedDaily = true;
                yield return schedule.FrequencyInterval.ToString();
            }
            if (!returnedDaily)
            {
                yield return "*";
            }

            bool returnedMonhtly = false;
            if (schedule.FrequencyTypes == FrequencyTypes.Monthly || schedule.FrequencyTypes == FrequencyTypes.MonthlyRelative)
            {
                returnedMonhtly = true;
                yield return $@"*/{schedule.FrequencyRecurrenceFactor}";
            }
            if (!returnedMonhtly)
            {
                yield return "*";
            }

            bool returnedWeekly = false;
            if (schedule.FrequencyTypes == FrequencyTypes.Weekly && schedule.FrequencyInterval > 1)
            {
                returnedWeekly = true;
                yield return string.Join(",", GetDayValues(schedule.FrequencyInterval));
            }
            if (schedule.FrequencyTypes == FrequencyTypes.MonthlyRelative && schedule.FrequencyInterval > 1)
            {
                returnedWeekly = true;
                var intervals = string.Join(",", GetDayValues((int) EnumOrder<WeekDays>.ValueAt(schedule.FrequencyInterval -1)));
                var suffix = GetRelativeIntervalSuffix(schedule.FrequencyRelativeIntervals);
                yield return $"{intervals}{suffix}";
            }
            if (!returnedWeekly)
            {
                yield return "*";
            }
        }

        public static string GetRelativeIntervalSuffix(FrequencyRelativeIntervals frequencyRelativeIntervals)
        {
            if (frequencyRelativeIntervals > 0)
            {
                switch (frequencyRelativeIntervals)
                {
                    case FrequencyRelativeIntervals.First:
                        return "#1";
                    case FrequencyRelativeIntervals.Second:
                        return "#2";
                    case FrequencyRelativeIntervals.Third:
                        return "#3";
                    case FrequencyRelativeIntervals.Fourth:
                        return "#4";
                    case FrequencyRelativeIntervals.Last:
                        return "L";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return "";
        }

        public static IEnumerable<int> GetDayValues(int frequencyInterval)
        {
            WeekDays days = (WeekDays) frequencyInterval;

            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof (DayOfWeek)))
            {
                var currentWeekDay = (WeekDays) Enum.Parse(typeof (WeekDays), dayOfWeek.ToString());
                if ((days & currentWeekDay) == currentWeekDay)
                {
                    yield return dayOfWeek.GetHashCode();
                }
            }
        }
    }
}
