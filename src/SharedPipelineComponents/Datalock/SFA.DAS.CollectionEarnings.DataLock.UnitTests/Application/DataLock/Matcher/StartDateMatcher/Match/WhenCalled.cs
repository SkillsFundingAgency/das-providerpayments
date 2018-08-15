using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.StartDateMatcher.Match
{
    public class WhenCalled
    {
        private static readonly object[] MatchingStartAndEffectiveFromDates =
        {
            new object[] {new DateTime(2016, 9, 15), new DateTime(2016, 9, 15),  new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 9, 1), new DateTime(2016, 9, 1), new DateTime(2016, 9, 15)}
        };

        private static readonly object[] MismatchingStartOrEffectiveFromDates =
        {
            new object[] {new DateTime(2016, 9, 16), new DateTime(2016, 9, 16), new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 10, 1), new DateTime(2016, 10, 1), new DateTime(2016, 9, 15)},

            new object[] {new DateTime(2016, 9, 15), new DateTime(2016, 9, 16), new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 9, 15), new DateTime(2016, 10, 1), new DateTime(2016, 9, 15)},

            new object[] {new DateTime(2016, 9, 16), new DateTime(2016, 9, 15), new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 10, 1), new DateTime(2016, 9, 15), new DateTime(2016, 9, 15)}
        };

        private CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher _matcher;
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
        [TestCaseSource(nameof(MatchingStartAndEffectiveFromDates))]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided(DateTime commitmentStartDate, DateTime commitmentEffectiveFromDate, DateTime learnerStartDate)
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder()
                    .WithStartDate(commitmentStartDate)
                    .WithEffectiveFrom(commitmentEffectiveFromDate)
                    .Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher(null);
            var priceEpisode = new PriceEpisodeBuilder().WithStartDate(learnerStartDate).Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCodes);
           
        }

        [Test]
        [TestCaseSource(nameof(MismatchingStartOrEffectiveFromDates))]
        public void ThenErrorCodeReturnedForMismatchingDataProvided(DateTime commitmentStartDate, DateTime commitmentEffectiveFromDate, DateTime learnerStartDate)
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder()
                    .WithStartDate(commitmentStartDate)
                    .WithEffectiveFrom(commitmentEffectiveFromDate)
                    .Build()
            };
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher(null);
            var priceEpisode = new PriceEpisodeBuilder().WithStartDate(learnerStartDate).Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);


            // Assert
            Assert.IsTrue( matchResult.ErrorCodes.Contains(DataLockErrorCodes.EarlierStartDate));
            
        }
        [Test]
        [TestCaseSource(nameof(MismatchingStartOrEffectiveFromDates))]
        public void ThenNextHanderShouldNotBeCalled(DateTime commitmentStartDate, DateTime commitmentEffectiveFromDate, DateTime learnerStartDate)
        {
            // Arrange
            var commitments = new List<CommitmentEntity>
            {
                new CommitmentBuilder()
                    .WithStartDate(commitmentStartDate)
                    .WithEffectiveFrom(commitmentEffectiveFromDate)
                    .Build()
            };

            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher(_nextMatcher.Object);

            var priceEpisode = new PriceEpisodeBuilder().WithStartDate(learnerStartDate).Build();

            var accounts = new List<CollectionEarnings.DataLock.Application.DasAccount.DasAccount> { new DasAccountBuilder().Build() };
            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);


            // Assert

            _nextMatcher.Verify(
                        m =>
                            m.Match(It.Is<List<CommitmentEntity>>(x => x[0].Equals(commitments[0])),
                                It.IsAny<RawEarning>()
                                ,It.IsAny<MatchResult>()),
                        Times.Never());
        }
    }
}