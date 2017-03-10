using System;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAgentCron.Implementation.Tests
{
    [TestClass]
    public class SqlAgentCronExpressionBuilderTests
    {
        private SqlAgentCronExpressionBuilder _builder;

        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new SqlAgentCronExpressionBuilder();
            
        }

        [TestMethod]
        public void Every_12_AM_Schedule_Should_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 * * *", expression);
        }

        [TestMethod]
        public void Every_Five_Days_At_12_AM_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencyInterval = 5,
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 */5 * *", expression);
        }

        [TestMethod]
        public void Every_12_PM_Schedule_Should_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Daily,
                ActiveStartTimeOfDay = new TimeSpan(12, 0, 0)
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 12 * * *", expression);
        }

        [TestMethod]
        public void Every_Ten_Seconds_Schedule_Should_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencySubDayInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Second,
                FrequencyTypes = FrequencyTypes.Daily
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("*/10 * * * * *", expression);
        }

        [TestMethod]
        public void Every_Ten_Minute_Schedule_Should_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencySubDayInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Minute,
                FrequencyTypes = FrequencyTypes.Daily
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("*/10 * * * *", expression);
        }

        [TestMethod]
        public void Every_Ten_Hour_Schedule_Should_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencySubDayInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.Daily
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 */10 * * *", expression);
        }

        [TestMethod]
        public void Every_Week_Monday_And_Wednesday_At_12_AM_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencyInterval = 10,
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Weekly
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 * * 1,3", expression);
        }

        [TestMethod]
        public void Every_3_Month_30_th_Day_At_12_AM_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencyInterval = 30,
                FrequencyRecurrenceFactor = 3,
                FrequencySubDayTypes = FrequencySubDayTypes.Once,
                FrequencyTypes = FrequencyTypes.Monthly
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 30 */3 *", expression);
        }

        [TestMethod]
        public void Every_3_Month_Second_WeekDays_Every_Hour_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencyInterval = 9,
                FrequencyRecurrenceFactor = 3,
                FrequencyRelativeIntervals = FrequencyRelativeIntervals.Second,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.MonthlyRelative
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 * */3 1,2,3,4,5#2", expression);
        }

        [TestMethod]
        public void Every_3_Month_Last_WeekEnds_Every_Hour_Returns()
        {
            var sqlAgentJobSchedule = new SqlAgentJobSchedule
            {
                FrequencyInterval = 10,
                FrequencyRecurrenceFactor = 3,
                FrequencyRelativeIntervals = FrequencyRelativeIntervals.Last,
                FrequencySubDayInterval = 1,
                FrequencySubDayTypes = FrequencySubDayTypes.Hour,
                FrequencyTypes = FrequencyTypes.MonthlyRelative
            };

            var expression = _builder.GetExpression(sqlAgentJobSchedule);

            Assert.AreEqual("0 0 * */3 0,6L", expression);
        }
    }
}
