using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Application;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.StandardMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler _matcher;
        private Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _nextMatcher = new Mock<CollectionEarnings.DataLock.Application.DataLock.Matcher.MatchHandler>(null);

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<Commitment>>(), 
                    It.IsAny<RawEarning>(), It.IsAny<DateTime>(),
                    It.IsAny<MatchResult>()))
                .Returns(new MatchResult { ErrorCodes = new List<string>() });

        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().WithStandardCode(1).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build(),
                new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler(null);

            var priceEpisode = new PriceEpisodeBuilder().WithStandardCode(1).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match((IReadOnlyList<Commitment>) commitments, priceEpisode, (DateTime)new DateTime());


            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
          
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().WithStandardCode(998).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build(),
                new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler(null);

            var priceEpisode = new PriceEpisodeBuilder().WithStandardCode(1).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build();
            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match((IReadOnlyList<Commitment>) commitments, priceEpisode, (DateTime)new DateTime());


            // Assert
            Assert.IsTrue( matchResult.ErrorCodes.Contains(DataLockErrorCodes.MismatchingStandard));
           
        }

        [Test]
        public void ThenNextHanderShouldBeCalled()
        {
            var commitments = new List<Commitment>
            {
                new CommitmentBuilder().WithStandardCode(998).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build(),
                new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().WithStandardCode(1).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build();
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler(_nextMatcher.Object);

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match((IReadOnlyList<Commitment>) commitments, priceEpisode, (DateTime)new DateTime());

            // Assert
            _nextMatcher.Verify(
                  m =>
                      m.Match(It.Is<List<Commitment>>(x => x[0].Equals(commitments[0])),
                          It.IsAny<RawEarning>(), It.IsAny<DateTime>(), It.IsAny<MatchResult>()),
                  Times.Once());

        }
    }
}