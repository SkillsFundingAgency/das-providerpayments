﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.UlnMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler _matcher;
      
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CommitmentEntity>>(), It.IsAny<RawEarning>(), It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });
           

        }
        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithUln(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler(null);
            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
          
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().WithUln(998).Build(),
                new CommitmentBuilder().WithUln(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler(null);
            var priceEpisode = new PriceEpisodeBuilder().Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);


            // Assert
            Assert.IsTrue( matchResult.ErrorCodes.Contains(DataLockErrorCodes.MismatchingUln));
           
        }
        [Test]
        public void ThenNextHanderShouldNotBeCalled()
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder().WithUln(998).Build(),
                new CommitmentBuilder().WithUln(999).Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler(null);

            var priceEpisode = new PriceEpisodeBuilder().Build();
            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            _nextMatcher.Verify(
                       m =>
                           m.Match(It.Is<List<CommitmentEntity>>(x => x[0].Equals(commitments[0])),
                               It.IsAny<RawEarning>(), It.IsAny<MatchResult>()),
                       Times.Never());

        }
    }
}