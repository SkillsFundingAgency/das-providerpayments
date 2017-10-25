using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Providers;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.Payments.DCFS.Domain;
using PaymentStatus = SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums.PaymentStatus;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler
{
    public class WhenHandlingSinglePriceEpisode
    {
        private static readonly object[] EndDatesWithExpectedPeriodNumberForMatchAndYearOfCollectionStartDate =
        {
            new object[] {new DateTime(2018, 7, 15), 12, new DateTime(2017, 8, 1)},
            new object[] {new DateTime(2018, 8, 15), 1, new DateTime(2018, 8, 1)},
            new object[] {new DateTime(2018, 5, 30), 10, new DateTime(2017, 8, 1)}
        };

        private static readonly object[] StartAndEndDatesWithFirstPeriodAndExpectedNumberOfPeriodMatches =
        {
            new object[] {new DateTime(2017, 9, 15), new DateTime(2017, 11, 15), 2, 3},
            new object[] {new DateTime(2017, 9, 30), new DateTime(2017, 11, 30), 2, 3},
            new object[] {new DateTime(2017, 11, 15), new DateTime(2018, 6, 21), 4, 8}
        };

        private static readonly object[] StartAndEndDatesWithFirstPeriodExpectedNumberOfPeriodMatchesAndYearOfCollectionStartDate =
        {
            new object[] {new DateTime(2017, 5, 1), new DateTime(2018, 5, 17), 10, 3, new DateTime(2016, 8, 1)},
            new object[] {new DateTime(2017, 5, 1), new DateTime(2018, 5, 17), 1, 10, new DateTime(2017, 8, 1)}
        };

        private static readonly object[] StartAndEndDatesWithExpectedMathsAndEnglishPeriodWithPayableFlag =
        {
            new object[] {new DateTime(2017, 8, 1), new DateTime(2017, 9, 15), 2, true},
            new object[] {new DateTime(2017, 8, 1), new DateTime(2017, 9, 30), 2, true}
        };

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RunDataLockValidationQueryRequest _request;
        private IMatcher _matcher;
        private CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _matcher = MatcherFactory.CreateMatcher();

            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(new DateTime(2017, 8, 1));

            _handler = new CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler(_dateTimeProvider.Object,_matcher);
        }

        [Test]
        public void ThenItShouldGenerateTheCorrectPeriodMatchesForACommitmentWithMultipleStatusesThatDontOverlap()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 10, 14))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-002")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2017, 10, 15))
                    .WithEffectiveTo(new DateTime(2017, 11, 14))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-003")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 11, 15))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(132, response.PriceEpisodePeriodMatches.Length);

            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 1 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 2 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 3 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-002"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 4 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 5 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 6 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 7 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 8 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 9 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 10 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 11 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 12 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));

            var rs = response.PriceEpisodePeriodMatches.Where(m => m.TransactionType == TransactionType.Learning);
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Learning));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18ProviderIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18ProviderIncentive));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgramme16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.FirstDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.SecondDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.BalancingMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.LearningSupport));
        }

        [Test]
        public void ThenItShouldGenerateTheCorrectPeriodMatchesForACommitmentWithMultipleStatusesThatOverlap()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 10, 14))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-002")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2017, 10, 15))
                    .WithEffectiveTo(new DateTime(2017, 9, 15))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-003")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2017, 9, 16))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(132, response.PriceEpisodePeriodMatches.Length);

            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 1 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 2 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 3 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 4 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 5 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 6 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 7 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 8 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 9 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 10 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 11 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 12 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));

            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Learning));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18ProviderIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18ProviderIncentive));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgramme16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.FirstDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.SecondDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.BalancingMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.LearningSupport));
        }

        [Test]
        public void ThenItShouldPickTheLatestCommitmentVersionWhenItOverwritesAPreviousVersion()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 10, 14))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-002")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2017, 10, 15))
                    .WithEffectiveTo(new DateTime(2017, 7, 31))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-003")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(132, response.PriceEpisodePeriodMatches.Length);

            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 1 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 2 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 3 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 4 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 5 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 6 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 7 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 8 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 9 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 10 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 11 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 12 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-003"));

            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Learning));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18ProviderIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18ProviderIncentive));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgramme16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.FirstDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.SecondDisadvantagePayment));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.BalancingMathsAndEnglish));
            Assert.AreEqual(12, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.LearningSupport));
        }

        [Test]
        public void ThenItShouldUseReacivatedCommitmentVersionWhenCommitmentIsReactivedAfterBeingPaused()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-007")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2018, 1, 1))
                    .WithEffectiveTo(new DateTime(2018, 2, 28))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-009")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2018, 3, 1))
                    .WithEffectiveTo(new DateTime(2018, 6, 30))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-012")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2018, 3, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2018, 1, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(77, response.PriceEpisodePeriodMatches.Length);

            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 6 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-007"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 7 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-007"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 8 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 9 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 10 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 11 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 12 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));

            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Learning));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18ProviderIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18ProviderIncentive));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgramme16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.FirstDisadvantagePayment));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.SecondDisadvantagePayment));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.BalancingMathsAndEnglish));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.LearningSupport));
        }

        [Test]
        public void ThenItShouldUseLatestCommitmentVersionOfPausedCommitmentVersionAndNotPayable()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-007")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2018, 1, 1))
                    .WithEffectiveTo(new DateTime(2018, 2, 28))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-009")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2018, 3, 1))
                    .WithEffectiveTo(new DateTime(2018, 6, 30))
                    .Build(),
                new CommitmentBuilder()
                    .WithVersionId("1-012")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 5, 31))
                    .WithPaymentStatus(PaymentStatus.Paused)
                    .WithEffectiveFrom(new DateTime(2018, 7, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2018, 1, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);
            Assert.AreEqual(77, response.PriceEpisodePeriodMatches.Length);

            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 6 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-007"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 7 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-007"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 8 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-009"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 9 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-009"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 10 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-009"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 11 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-009"));
            Assert.AreEqual(11, response.PriceEpisodePeriodMatches.Count(m => m.Period == 12 && !m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-012"));

            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Learning));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.First16To18ProviderIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18EmployerIncentive));
            Assert.AreEqual(0, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Second16To18ProviderIncentive));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgramme16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.FirstDisadvantagePayment));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.SecondDisadvantagePayment));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.BalancingMathsAndEnglish));
            Assert.AreEqual(7, response.PriceEpisodePeriodMatches.Count(m => m.TransactionType == TransactionType.LearningSupport));
        }

        [Test]
        [TestCaseSource(nameof(EndDatesWithExpectedPeriodNumberForMatchAndYearOfCollectionStartDate))]
        public void ThenItShouldGenerateAPeriodMatchWhenThePriceEpisodeDoesNotEndOnTheCensusDate(DateTime priceEpisodeEndDate, int period, DateTime yearOfCollectionStartDate)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionStartDate);

            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 7, 31))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(priceEpisodeEndDate)
                    .Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                PriceEpisodes = priceEpisodes,
                DasAccounts= new[] { new DasAccountBuilder().Build() }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);

            Assert.AreEqual(1, response.PriceEpisodePeriodMatches.Count(m => m.Period == period && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001" && m.TransactionType == TransactionType.Learning));
        }

        [Test]
        [TestCaseSource(nameof(StartAndEndDatesWithFirstPeriodAndExpectedNumberOfPeriodMatches))]
        public void ThenItShouldGenerateTheExpectedNumberOfPeriodMatches(DateTime startDate, DateTime endDate, int firstPeriod, int expectedNumberOfMatches)
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);

            Assert.AreEqual(expectedNumberOfMatches, response.PriceEpisodePeriodMatches.Count(m => m.Period >= firstPeriod && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001" && m.TransactionType == TransactionType.Learning));
        }

        [Test]
        [TestCaseSource(nameof(StartAndEndDatesWithFirstPeriodExpectedNumberOfPeriodMatchesAndYearOfCollectionStartDate))]
        public void ThenItShouldGenerateTheExpectedNumberOfPeriodMatchesForAPriceEpisodeThatSpansOverMultipleAcademicYears(DateTime startDate, DateTime endDate, int firstPeriod, int expectedNumberOfMatches, DateTime yearOfCollectionStartDate)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionStartDate);

            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(startDate)
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);

            Assert.AreEqual(expectedNumberOfMatches, response.PriceEpisodePeriodMatches.Count(m => m.Period >= firstPeriod && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001" && m.TransactionType == TransactionType.Learning));
        }

        [Test]
        [TestCaseSource(nameof(StartAndEndDatesWithExpectedMathsAndEnglishPeriodWithPayableFlag))]
        public void ThenItShouldGenerateTheExpectedMathsAndEnglishOnProgPeriodMatch(DateTime startDate, DateTime endDate, int period, bool payable)
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(startDate)
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);

            Assert.AreEqual(1, response.PriceEpisodePeriodMatches.Count(m => m.Period == period && m.Payable == payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001" && m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
        }

        [Test]
        public void ThenItShouldGenerateTheExpectedMathsAndEnglishBalancingPeriodMatch()
        {

            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder()
                    .WithVersionId("1-001")
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2017, 9, 30))
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder()
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2017, 09, 17))
                    .Build()
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
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.PriceEpisodeMatches.Length);

            Assert.AreEqual(1, response.PriceEpisodePeriodMatches.Count(m => m.Period == 1 && m.Payable && m.CommitmentId == 1 && m.CommitmentVersionId == "1-001" && m.TransactionType == TransactionType.OnProgrammeMathsAndEnglish));
        }
    }
}