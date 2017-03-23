using System;
using FluentAssertions;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAgentCron.Tests
{
    [TestClass]
    public class SqlAgentCronExpressionParserTests
    {
        private SqlAgentCronExpressionParser _expressionParser;

        [TestInitialize]
        public void TestInitialize()
        {
            _expressionParser = new SqlAgentCronExpressionParser();
        }

        [TestMethod]
        public void Every_Day_1_30_AM_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("30 1 * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
                FrequencyInterval = 1,
                FrequencySubDayInterval = 1,
                ActiveStartTimeOfDay = new TimeSpan(1, 30, 0)
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Five_Days_At_1_30_AM_Returns()
        {
            var actual = _expressionParser.GetSchedule("30 1 */5 * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
                FrequencyInterval = 5,
                FrequencySubDayInterval = 0,
                ActiveStartTimeOfDay = new TimeSpan(1, 30, 0)
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Day_12_PM_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 12 * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
                FrequencyInterval = 1,
                FrequencySubDayInterval = 1,
                ActiveStartTimeOfDay = new TimeSpan(12, 0, 0)
            };

            expected.ShouldBeEquivalentTo(actual);
        }

        [TestMethod]
        public void Every_Minute_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("* * * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Minute,
                FrequencyTypes = FrequencyTypes.Daily,
                FrequencyInterval = 1
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Ten_Minute_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("*/10 * * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencySubDayInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Minute,
                FrequencyTypes = FrequencyTypes.Daily,
                FrequencyInterval = 1
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Hour_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 * * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 1,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.Daily
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Ten_Hour_Schedule_Should_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 */10 * * *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 1,
                FrequencySubDayInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.Daily
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Week_Sunday_At_1_30_AM_Returns()
        {
            var actual = _expressionParser.GetSchedule("30 1 * * 0");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 1,
                FrequencyRecurrenceFactor = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Weekly,
                ActiveStartTimeOfDay = new TimeSpan(1, 30, 0)
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Week_Sunday_At_Every_Minute_Returns()
        {
            var actual = _expressionParser.GetSchedule("* * * * 0");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 1,
                FrequencyRecurrenceFactor = 1,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Minute,
                FrequencyTypes = FrequencyTypes.Weekly
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Week_Sunday_At_Every_Hour_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 * * * 0");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 1,
                FrequencyRecurrenceFactor = 1,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.Weekly
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void Every_Week_Monday_And_Wednesday_At_1_30_AM_Returns()
        {
            var actual = _expressionParser.GetSchedule("30 1 * * 1,3");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 10,
                FrequencyRecurrenceFactor = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Weekly,
                ActiveStartTimeOfDay = new TimeSpan(1, 30, 0)
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void At_every_minute_on_day_of_month_30_in_every_3rd_month()
        {
            var actual = _expressionParser.GetSchedule("* * 30 */3 *");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 30,
                FrequencyRecurrenceFactor = 3,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Minute,
                FrequencyTypes = FrequencyTypes.Monthly
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod, Ignore]
        public void Every_3_Month_Second_WeekDays_Every_Hour_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 * * */3 1,2,3,4,5#2");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 9,
                FrequencyRecurrenceFactor = 3,
                FrequencyRelativeIntervals = FrequencyRelativeIntervals.Second,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.MonthlyRelative
            };

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod, Ignore]
        public void Every_3_Month_Last_WeekEnds_Every_Hour_Returns()
        {
            var actual = _expressionParser.GetSchedule("0 0 * */3 0,6L");

            var expected = new SqlAgentJobSchedule
            {
                FrequencyInterval = 10,
                FrequencyRecurrenceFactor = 3,
                FrequencyRelativeIntervals = FrequencyRelativeIntervals.Last,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.MonthlyRelative
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}