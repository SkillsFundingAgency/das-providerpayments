using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Providers;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler
{
    public class WhenHandling
    {
        private static readonly object[] PriceEpisodesWithMismatchingUln =
        {
            new object[] {new PriceEpisodeBuilder().WithUln(1000000018).Build()},
            new object[] {new PriceEpisodeBuilder().WithUln(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingFramework =
        {
            new object[] {new PriceEpisodeBuilder().WithFrameworkCode(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithFrameworkCode(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingProgramme =
        {
            new object[] {new PriceEpisodeBuilder().WithProgrammeType(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithProgrammeType(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingPathway =
        {
            new object[] {new PriceEpisodeBuilder().WithPathwayCode(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithPathwayCode(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingPrice =
        {
            new object[] {new PriceEpisodeBuilder().WithNegotiatedPrice(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithNegotiatedPrice(null).Build()}
        };

        private static readonly object[] MismatchingStartAndEffectiveFromDates =
       {
            new object[] {new DateTime(2016, 9, 1), new DateTime(2016, 9, 1)},
            new object[] {new DateTime(2016, 8, 1), new DateTime(2016, 9, 1)}
        };

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RunDataLockValidationQueryRequest _request;
        private IMatcher _matcher;

        private
            CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _matcher = MatcherFactory.CreateMatcher();

            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(new DateTime(2016, 8, 1));

            _handler = new CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler(_dateTimeProvider.Object, _matcher);
        }


        [Test]
        public void ThenNoErrorExpectedForMatchingCommitmentAndPriceEpisodeData()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        public void ThenErrorExpectedForNoUkprnMatch()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().WithUkprn(10007458).Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUkprn));
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingUln))]
        public void ThenErrorExpectedForNoUlnMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUln));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        public void ThenErrorExpectedForNoStandardMatch()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().WithStandardCode(999).Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().WithStandardCode(998).Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingStandard));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Count(x => x.IsSuccess));
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingFramework))]
        public void ThenErrorExpectedForNoFrameworkMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingFramework));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Count(x => x.IsSuccess));
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingProgramme))]
        public void ThenErrorExpectedForNoProgrammeMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingProgramme));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Count(x => x.IsSuccess));
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingPathway))]
        public void ThenErrorExpectedForNoPathwayMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPathway));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Count(x => x.IsSuccess));
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingPrice))]
        public void ThenErrorExpectedForNoPriceMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPrice));
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        [TestCaseSource(nameof(MismatchingStartAndEffectiveFromDates))]
        public void ThenErrorExpectedForNoStartDateOrEarlierStartDate(DateTime startDate, DateTime effectiveFrom)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder()
                        .WithStartDate(startDate)
                        .WithEffectiveFrom(effectiveFrom)
                        .Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder()
                        .WithStartDate(new DateTime(2016, 8, 31))
                        .Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.EarlierStartDate));
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(121, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        public void ThenErrorExpectedForMultiplePriceMatchingCommitments()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithCommitmentId(2).Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build()}
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MultipleMatches));
            Assert.AreEqual(0, response.PriceEpisodeMatches.Count(x => x.IsSuccess));
            Assert.AreEqual(242, response.PriceEpisodePeriodMatches.Length);
        }

        [Test]
        public void ThenMultipleErrorsExpectedForMultiplePriceEpisodesProvided()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder().Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn002").WithUkprn(10007458).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn003").WithUln(1000000018).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn004").WithUln(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn005").WithStandardCode(998).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn006").WithFrameworkCode(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn007").WithFrameworkCode(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn008").WithProgrammeType(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn009").WithProgrammeType(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn010").WithPathwayCode(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn011").WithPathwayCode(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn012").WithNegotiatedPrice(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn013").WithNegotiatedPrice(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn014").WithStartDate(new DateTime(2016, 8, 31)).Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                PriceEpisodes = priceEpisodes,
                DasAccounts = new[] { new DasAccountBuilder().Build() }

            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(13, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Count(l =>
                                                                    l.CommitmentId == commitments[0].CommitmentId &&
                                                                    l.Ukprn == priceEpisodes[0].Ukprn &&
                                                                    l.LearnerReferenceNumber == priceEpisodes[0].LearnerReferenceNumber &&
                                                                    priceEpisodes[0].AimSequenceNumber.HasValue &&
                                                                    l.AimSequenceNumber == priceEpisodes[0].AimSequenceNumber.Value));
        }

        [Test]
        public void ThenFirstIncentiveThreshholdDateIsExpectedForMatchingCommitmentAndPriceEpisodeData()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2016, 8, 1))
                    .WithEndDate(new DateTime(2017, 08, 01))
                    .WithEffectiveFrom(new DateTime(2016, 8, 1))
                    .WithEffectiveTo(new DateTime(2016, 11, 14))
                    .WithVersionId("1-001")
                    .WithPaymentStatus(Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build(),
                new CommitmentBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2016, 8, 1))
                    .WithEndDate(new DateTime(2017, 08, 01))
                    .WithEffectiveFrom(new DateTime(2016, 11, 15))
                    .WithVersionId("1-002")
                    .WithPaymentStatus(Tools.Enums.PaymentStatus.Cancelled)
                    .WithAgreedCost(7500)
                    .Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2016,08,01))
                    .WithEndDate(new DateTime(2017,08,08))
                    .WithFirstIncentiveThreshholdDate(new DateTime(2016,11,04))
                    .WithNegotiatedPrice(7500)
                    .Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);
            
            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.IsTrue(response.PriceEpisodePeriodMatches.Single(x=> x.Period == 4 && x.TransactionType==Payments.DCFS.Domain.TransactionType.First16To18EmployerIncentive).Payable);
            Assert.IsTrue(response.PriceEpisodePeriodMatches.Single(x => x.Period == 4 && x.TransactionType == Payments.DCFS.Domain.TransactionType.First16To18ProviderIncentive).Payable);

            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 4 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning).Payable);
            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 4 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Balancing).Payable);
            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 4 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Completion).Payable);

        }

        [Test]
        public void ThenSecondIncentiveThreshholdDateIsExpectedForMatchingCommitmentAndPriceEpisodeData()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2016, 8, 1))
                    .WithEndDate(new DateTime(2017, 08, 10))
                    .WithEffectiveFrom(new DateTime(2016, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 06, 10))
                    .WithVersionId("1-001")
                    .WithPaymentStatus(Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build(),
                new CommitmentBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2016, 8, 1))
                    .WithEndDate(new DateTime(2017, 08, 10))
                    .WithEffectiveFrom(new DateTime(2017, 06, 11))
                    .WithVersionId("1-002")
                    .WithPaymentStatus(Tools.Enums.PaymentStatus.Cancelled)
                    .WithAgreedCost(7500)
                    .Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2016,08,01))
                    .WithEndDate(new DateTime(2017,08,10))
                    .WithSecondIncentiveThreshholdDate(new DateTime(2017,06,04))
                    .WithNegotiatedPrice(7500)
                    .Build()
                },
                DasAccounts = new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.IsTrue(response.PriceEpisodePeriodMatches.Single(x => x.Period == 11 && x.TransactionType ==Payments.DCFS.Domain.TransactionType.Second16To18EmployerIncentive).Payable);
            Assert.IsTrue(response.PriceEpisodePeriodMatches.Single(x => x.Period == 11 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Second16To18ProviderIncentive).Payable);
            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 11 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning).Payable);
            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 11 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Balancing).Payable);
            Assert.IsFalse(response.PriceEpisodePeriodMatches.Single(x => x.Period == 11 && x.TransactionType == Payments.DCFS.Domain.TransactionType.Completion).Payable);


        }
    }
}
