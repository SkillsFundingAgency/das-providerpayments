using System;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Extensions;

namespace SFA.DAS.Payments.DCFS.UnitTests.Extensions.DateTimeExtensions.DaysInMonth
{
    public class WhenGettingDaysInMonthForLeapYear
    {
        [TestCase(2016, 1, 1, 31)]
        [TestCase(2016, 2, 1, 29)]
        [TestCase(2016, 3, 1, 31)]
        [TestCase(2016, 4, 1, 30)]
        [TestCase(2016, 5, 1, 31)]
        [TestCase(2016, 6, 1, 30)]
        [TestCase(2016, 7, 1, 31)]
        [TestCase(2016, 8, 1, 31)]
        [TestCase(2016, 9, 1, 30)]
        [TestCase(2016, 10, 1, 31)]
        [TestCase(2016, 11, 1, 30)]
        [TestCase(2016, 12, 1, 31)]
        [TestCase(2016, 1, 15, 31)]
        [TestCase(2016, 2, 15, 29)]
        public void ThenItShouldReturnCorrectDaysInMonth(int year, int month, int day, int expectedNumberOfDays)
        {
            // Arrange
            var input = new DateTime(year, month, day);

            // Act
            var actualNumberOfDays = input.DaysInMonth();

            // Assert
            Assert.AreEqual(expectedNumberOfDays, actualNumberOfDays);
        }
    }
}
