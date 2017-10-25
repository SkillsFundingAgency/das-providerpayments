using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.PathwayMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.PathwayMatchHandler _matcher;
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), 
                    It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>(),
                     It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(),
                    It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });

        
        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithPathwayCode(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.PathwayMatchHandler(null);

            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);


            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
         
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithPathwayCode(998).Build(),
                new CommitmentBuilder().WithPathwayCode(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.PathwayMatchHandler(null);


            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);

            // Assert
            Assert.IsTrue( matchResult.ErrorCodes.Contains(DataLockErrorCodes.MismatchingPathway));
          
        }
        [Test]
        public void ThenNextHanderShouldBeCalled()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithPathwayCode(998).Build(),
                new CommitmentBuilder().WithPathwayCode(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.PathwayMatchHandler(_nextMatcher.Object);

            var priceEpisode = new PriceEpisodeBuilder().Build();
            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode, accounts);


            // Assert
            _nextMatcher.Verify(
                  m =>
                      m.Match(It.Is<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(x => x[0].Equals(commitments[0])),
                          It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()
                          , It.IsAny<List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount>>(),It.IsAny<MatchResult>()),
                  Times.Once());

        }
    }
}