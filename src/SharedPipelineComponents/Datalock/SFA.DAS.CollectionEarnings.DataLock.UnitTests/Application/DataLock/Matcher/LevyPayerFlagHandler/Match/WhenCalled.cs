using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.LevyPayerFlagHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.LevyPayerFlagMatcher _matcher;
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CommitmentEntity>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>(), It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(), It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });

          

        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.LevyPayerFlagMatcher(null);

            var priceEpisode = new PriceEpisodeBuilder().Build();
            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode,accounts);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
         
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().WithFrameworkCode(998).Build(),
                new CommitmentBuilder().WithFrameworkCode(999).Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.LevyPayerFlagMatcher(null);

            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().WithIsLevyPayer(false).Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);


            // Assert
            Assert.IsTrue(matchResult.ErrorCodes.Contains(DataLockErrorCodes.NotLevyPayer));
           
        }

        [Test]
        public void ThenNextHanderShouldBeCalled()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().WithFrameworkCode(998).Build(),
                new CommitmentBuilder().WithFrameworkCode(999).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.LevyPayerFlagMatcher(_nextMatcher.Object);

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode,accounts);

            // Assert
            _nextMatcher.Verify(
                  m =>
                      m.Match(It.Is<List<CommitmentEntity>>(x => x[0].Equals(commitments[0])),
                          It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>(), It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(), It.IsAny<MatchResult>()),
                  Times.Once());

        }
    }
}