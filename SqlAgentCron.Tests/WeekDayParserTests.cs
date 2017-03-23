using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlAgentCron.Tests
{
    [TestClass]
    public class WeekDayParserTests
    {
        private WeekDayParser _parser;

        [TestInitialize]
        public void TestInitialize()
        {
            _parser = new WeekDayParser();
        }

        [TestMethod]
        public void GetFrequencyInterval_Empty_List_Should_Return_0()
        {
            int actual = _parser.GetFrequencyInterval(Enumerable.Empty<int>());

            actual.Should().Be(0);
        }

        [TestMethod]
        public void GetFrequencyInterval_0_Should_Return_1()
        {
            int actual = _parser.GetFrequencyInterval(new[] { 0 });

            actual.Should().Be(1);
        }

        [TestMethod]
        public void GetFrequencyInterval_1_Should_Return_2()
        {
            int actual = _parser.GetFrequencyInterval(new[] {1});

            actual.Should().Be(2);
        }

        [TestMethod]
        public void GetFrequencyInterval_1_And_3_Should_Return_10()
        {
            int actual = _parser.GetFrequencyInterval(new[] { 1, 3 });

            actual.Should().Be(10);
        }
    }
}
