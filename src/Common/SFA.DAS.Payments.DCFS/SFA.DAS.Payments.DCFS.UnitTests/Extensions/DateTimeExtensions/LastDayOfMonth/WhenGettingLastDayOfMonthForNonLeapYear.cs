using System;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Extensions;

namespace SFA.DAS.Payments.DCFS.UnitTests.Extensions.DateTimeExtensions.LastDayOfMonth
{
    public class WhenGettingLastDayOfMonthForNonLeapYear
    {
        [TestCase(2017, 1, 1, 31)]
        [TestCase(2017, 2, 1, 28)]
        [TestCase(2017, 3, 1, 31)]
        [TestCase(2017, 4, 1, 30)]
        [TestCase(2017, 5, 1, 31)]
        [TestCase(2017, 6, 1, 30)]
        [TestCase(2017, 7, 1, 31)]
        [TestCase(2017, 8, 1, 31)]
        [TestCase(2017, 9, 1, 30)]
        [TestCase(2017, 10, 1, 31)]
        [TestCase(2017, 11, 1, 30)]
        [TestCase(2017, 12, 1, 31)]
        [TestCase(2017, 1, 15, 31)]
        [TestCase(2017, 2, 15, 28)]
        [TestCase(2017, 12, 31, 31)]
        [TestCase(2017, 2, 28, 28)]
        public void ThenItShouldReturnCorrectDate(int year, int month, int day, int expectedDay)
        {
            // Arrange
            var input = new DateTime(year, month, day);

            // Act
            var actual = input.LastDayOfMonth();

            // Assert
            Assert.AreEqual(year, actual.Year);
            Assert.AreEqual(month, actual.Month);
            Assert.AreEqual(expectedDay, actual.Day);
        }
    }
}
