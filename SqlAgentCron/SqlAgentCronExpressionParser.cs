using System;
using System.Linq;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron
{
    public class SqlAgentCronExpressionParser : ICronExpressionParser
    {
        private readonly IWeekDayParser _weekDayParser;

        public SqlAgentCronExpressionParser()
            : this(new WeekDayParser())
        {
        }

        public SqlAgentCronExpressionParser(IWeekDayParser weekDayParser)
        {
            _weekDayParser = weekDayParser;
        }

        public SqlAgentJobSchedule GetSchedule(string cronExpression)
        {
            SqlAgentJobSchedule result = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencySubDayInterval = 1,
                FrequencyInterval = 1
            };

            var cronValues = cronExpression.Split(CronChar.DigitSeperator).ToList();

            var dayOfWeekParts = cronValues[CronPart.DayOfWeek].Split(CronChar.ValueSeperator);
            if (dayOfWeekParts[0] != CronChar.Any)
            {
                result.FrequencyTypes = FrequencyTypes.Weekly;
                result.FrequencyRecurrenceFactor = 1;
                result.FrequencySubDayInterval = 0;
                result.FrequencyInterval = _weekDayParser.GetFrequencyInterval(dayOfWeekParts.Select(p => Convert.ToInt32(p)));
            }

            var monthParts = cronValues[CronPart.Month].Split(CronChar.StepSeperator);
            if (monthParts[0] == CronChar.Any)
            {
                if (result.FrequencyTypes != FrequencyTypes.Weekly)
                {
                    result.FrequencyTypes = FrequencyTypes.Monthly;
                }
                if (monthParts.Length > 1)
                {
                    result.FrequencyRecurrenceFactor = Convert.ToInt32(monthParts[1]);
                }
            }

            var dayParts = cronValues[CronPart.Day].Split(CronChar.StepSeperator);
            if (dayParts[0] == CronChar.Any)
            {
                if (result.FrequencyTypes != FrequencyTypes.Weekly)
                {
                    result.FrequencyTypes = FrequencyTypes.Daily;
                }
                if (dayParts.Length > 1)
                {
                    result.FrequencySubDayInterval = 0;
                    result.FrequencyInterval = Convert.ToInt32(dayParts[1]);
                }
            }
            else
            {
                result.FrequencyInterval = Convert.ToInt32(dayParts[0]);
            }

            var hourParts = cronValues[CronPart.Hour].Split(CronChar.StepSeperator);
            if (hourParts[0] == CronChar.Any)
            {
                result.FrequencySubDayTypes = FrequencySubDayTypes.Hour;
                result.FrequencySubDayInterval = hourParts.Length > 1 
                    ? Convert.ToInt32(hourParts[1]) 
                    : 1;
            }

            var minuteParts = cronValues[CronPart.Minute].Split(CronChar.StepSeperator);
            if (minuteParts[0] == CronChar.Any)
            {
                result.FrequencySubDayTypes = FrequencySubDayTypes.Minute;
                result.FrequencySubDayInterval = minuteParts.Length > 1
                    ? Convert.ToInt32(minuteParts[1])
                    : 1;
            }

            TimeSpan startHour;
            if (!cronValues[CronPart.Hour].StartsWith(CronChar.Any))
            {
                int hour = Convert.ToInt32(cronValues[CronPart.Hour]);
                startHour = new TimeSpan(hour, 0, 0);
            }
            else
            {
                startHour = TimeSpan.Zero;
            }

            TimeSpan startMinute;
            if (!cronValues[CronPart.Minute].StartsWith(CronChar.Any))
            {
                int minute = Convert.ToInt32(cronValues[CronPart.Minute]);
                startMinute = new TimeSpan(0, minute, 0);
            }
            else
            {
                startMinute = TimeSpan.Zero;
            }

            result.ActiveStartTimeOfDay = startHour.Add(startMinute);

            return result;
        }
    }
}