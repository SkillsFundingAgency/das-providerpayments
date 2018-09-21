using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Domain;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tests.DomainTests.GivenAPriceEpisode
{
    [TestFixture]
    public class WhenConstructing
    {
        [TestCase("2-513-1-01/08/2017", "1718")]
        [TestCase("2-513-1-01/08/2018", "1819")]
        [TestCase("2-513-1-21/07/2017", "1617")]
        [TestCase("25-96-01/08/2018", "1819")]
        [TestCase("3-414-1-21/11/2017", "1718")]
        [TestCase("25-138-17/04/2018", "1718")]
        [TestCase("25-82-08/03/2018", "1718")]
        [TestCase("3-462-1-09/11/2017", "1718")]
        [TestCase("3-489-1-30/06/2017", "1617")]
        [TestCase("25-94-28/06/2018", "1718")]
        public void ThenTheAcademicYearIsCorrect(string priceEpisodeIdentifier, string expected)
        {
            var sut = new PriceEpisode(priceEpisodeIdentifier);

            var actual = sut.AcademicYear;
            actual.Should().Be(expected);
        }
    }
}
