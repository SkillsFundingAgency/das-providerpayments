using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using Moq;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.MultipleMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler _matcher;
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CommitmentEntity>>(), 
                It.IsAny<RawEarning>(),
                It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(),
                It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });

          
        }

        [Test]
        public void ThenNullReturnedForSingleMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithVersionId("1-002").Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler(null);
            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
      
        }

        [Test]
        public void ThenErrorCodeReturnedForMultipleMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithVersionId("2-001").Build(),
                new CommitmentBuilder().WithCommitmentId(2).Build(),
                new CommitmentBuilder().WithCommitmentId(2).WithVersionId("2-001").Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler(null);
            var priceEpisode = new PriceEpisodeBuilder().Build();
            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);

            // Assert
            Assert.IsTrue( matchResult.ErrorCodes.Contains(DataLockErrorCodes.MultipleMatches));
           
        }

        [Test]
        public void ThenNextHanderShouldNotBeCalled()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithVersionId("1-002").Build(),
                new CommitmentBuilder().WithCommitmentId(2).Build(),
                new CommitmentBuilder().WithCommitmentId(2).WithVersionId("2-002").Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler(_nextMatcher.Object);

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);


            // Assert
            // Assert
            _nextMatcher.Verify(
                  m =>
                      m.Match(It.Is<List<CommitmentEntity>>(x => x[0].Equals(commitments[0])),
                          It.IsAny<RawEarning>(),
                          It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(),
                          It.IsAny<MatchResult>()),
                  Times.Never());


        }
    }
}