using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Application;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.UkprnMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler _matcher;
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<Commitment>>(), 
                                    It.IsAny<RawEarning>(), It.IsAny<DateTime>(), It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });
           
        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().Withukprn(999).Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler(null);

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, new DateTime());


            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().Withukprn(998).Build(),
                new CommitmentBuilder().Withukprn(999).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler(null);

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, new DateTime());


            // Assert
            Assert.IsTrue(matchResult.ErrorCodes.Contains(DataLockErrorCodes.MismatchingUkprn));
        }

        [Test]
        public void ThenNextHanderShouldNotBeCalled()
        {
            // Arrange
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().Withukprn(998).Build(),
                new CommitmentBuilder().Withukprn(999).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler(_nextMatcher.Object);

            // Act
            _matcher.Match(commitments, priceEpisode, new DateTime());

            // Assert
            _nextMatcher.Verify(
                       m =>
                           m.Match(It.Is<List<Commitment>>(x => x[0].Equals(commitments[0])),
                               It.IsAny<RawEarning>(), It.IsAny<DateTime>(), It.IsAny<MatchResult>()),
                       Times.Never());
        }
    }
}