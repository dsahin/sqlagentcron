using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron
{
    public class SqlAgentCronExpressionBuilder : ICronExpressionBuilder
    {
        private readonly IWeekDayParser _weekDayParser;

        public SqlAgentCronExpressionBuilder()
            : this(new WeekDayParser())
        {
        }

        public SqlAgentCronExpressionBuilder(IWeekDayParser weekDayParser)
        {
            _weekDayParser = weekDayParser;
        }

        public string GetExpression(SqlAgentJobSchedule sqlAgentJobSchedule)
        {
            var values = GetValues(sqlAgentJobSchedule).ToArray();
            return string.Join(" ", values);
        }

        public virtual IEnumerable<string> GetValues(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Unknown)
            {
                yield break;
            }

            yield return GetMinutePart(schedule);
            yield return GetHourPart(schedule);
            yield return GetDayPart(schedule);
            yield return GetMonthPart(schedule);
            yield return GetDayOfWeekPart(schedule);
        }

        public virtual string GetMinutePart(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Minute)
            {
                if (schedule.FrequencySubDayInterval > 59)
                {
                    
                }

                return schedule.FrequencySubDayInterval > 1 
                    ? $@"*/{schedule.FrequencySubDayInterval}" 
                    : CronChar.Any;
            }
            return schedule.ActiveStartTimeOfDay.Minutes.ToString();
        }

        public virtual string GetHourPart(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Once)
            {
                return schedule.ActiveStartTimeOfDay.Hours.ToString();
            }
            if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Hour)
            {
                if (schedule.FrequencySubDayInterval > 1)
                {
                    return $@"*/{schedule.FrequencySubDayInterval}";
                }
                return schedule.ActiveStartTimeOfDay.Hours.ToString();
            }
            return "*";
        }

        public virtual string GetDayPart(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencyTypes == FrequencyTypes.Daily && schedule.FrequencyInterval > 1)
            {
                return $@"*/{schedule.FrequencyInterval}";
            }
            if (schedule.FrequencyTypes == FrequencyTypes.Monthly && schedule.FrequencyInterval > 1)
            {
                return schedule.FrequencyInterval.ToString();
            }
            return "*";
        }

        public virtual string GetMonthPart(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencyTypes == FrequencyTypes.Monthly || schedule.FrequencyTypes == FrequencyTypes.MonthlyRelative)
            {
                return $@"*/{schedule.FrequencyRecurrenceFactor}";
            }
            return "*";
        }

        public virtual string GetDayOfWeekPart(SqlAgentJobSchedule schedule)
        {
            if (schedule.FrequencyTypes == FrequencyTypes.Weekly && schedule.FrequencyInterval > 0)
            {
                return string.Join(",", _weekDayParser.GetDays(schedule.FrequencyInterval));
            }
            if (schedule.FrequencyTypes == FrequencyTypes.MonthlyRelative && schedule.FrequencyInterval > 0)
            {
                var intervals = string.Join(",", _weekDayParser.GetDays((int)EnumOrder<WeekDays>.ValueAt(schedule.FrequencyInterval - 1)));
                var suffix = GetRelativeIntervalSuffix(schedule.FrequencyRelativeIntervals);
                return $"{intervals}{suffix}";
            }
            return "*";
        }

        public virtual string GetRelativeIntervalSuffix(FrequencyRelativeIntervals frequencyRelativeIntervals)
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
    }
}
