using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting
{
    [TestFixture]
    public class GivenALearnerWithAWithdrawnCommitment
    {
        [TestFixture]
        public class AndAnIlrWithFewerEarningsThanPayments
        {
            private static readonly IFixture Fixture = new Fixture();
            private static readonly long CommitmentOne = Fixture.Create<long>();
            private static readonly string PriceEpisodeIdentifierForThisYear = "25-96-21/11/2017";

            private static readonly string LearnAimRefForZprog = Fixture.Create<string>();
            private static readonly string LearnAimRefForMaths = Fixture.Create<string>();
            private static readonly string LearnAimRefForEnglish = Fixture.Create<string>();
            private static readonly long AccountId = Fixture.Create<long>();
            private static readonly string FundingLineType = Fixture.Create<string>();

            private static readonly List<RequiredPayment> PastPayments = new List<RequiredPayment>();

            private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
                new List<RawEarningForMathsOrEnglish>();

            private static readonly List<DatalockOutputEntity> Datalocks = new List<DatalockOutputEntity>();

            private static readonly List<Commitment> Commitments = new List<Commitment>
            {
                new Commitment
                {
                    AccountId = AccountId,
                    CommitmentId = CommitmentOne,
                    StandardCode = 96,
                    ProgrammeType = 0,
                    FrameworkCode = 0,
                    PathwayCode = 0,
                    PaymentStatus = 3,
                    IsLevyPayer = true,
                },
            };

            private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.ProgrammeType, 25)
                .With(x => x.StandardCode, 96)
                .With(x => x.FrameworkCode, 0)
                .With(x => x.PathwayCode, 0)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .With(x => x.LearnAimRef, LearnAimRefForZprog)
                .With(x => x.TransactionType01, 250.00000m)
                .With(x => x.TransactionType02, 0)
                .With(x => x.TransactionType03, 0)
                .With(x => x.TransactionType04, 0)
                .With(x => x.TransactionType05, 0)
                .With(x => x.TransactionType06, 0)
                .With(x => x.TransactionType07, 0)
                .With(x => x.TransactionType08, 0)
                .With(x => x.TransactionType09, 0)
                .With(x => x.TransactionType10, 0)
                .With(x => x.TransactionType11, 0)
                .With(x => x.TransactionType12, 0)
                .With(x => x.TransactionType13, 0)
                .With(x => x.TransactionType14, 0)
                .With(x => x.TransactionType15, 0)
                .With(x => x.ApprenticeshipContractType, ApprenticeshipContractType.Levy)
                .CreateMany(3)
                .ToList();

            [SetUp]
            public void Setup()
            {
                var pastPayments = Fixture.Build<RequiredPayment>()
                    .With(x => x.CommitmentId, CommitmentOne)
                    .With(x => x.AccountId, AccountId)
                    .With(x => x.TransactionType, 1)
                    .With(x => x.AmountDue, 0)
                    .With(x => x.StandardCode, 96)
                    .With(x => x.ProgrammeType, 25)
                    .With(x => x.FrameworkCode, 0)
                    .With(x => x.PathwayCode, 0)
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                    .With(x => x.LearnAimRef, LearnAimRefForZprog)
                    .With(x => x.ApprenticeshipContractType, ApprenticeshipContractType.Levy)
                    .With(x => x.SfaContributionPercentage, 0.9m)
                    .With(x => x.FundingLineType, FundingLineType)
                    .CreateMany(13)
                    .ToList();
                PastPayments.AddRange(pastPayments);
                
                var datalockForNextYearFirstCommitment = Fixture.Build<DatalockOutputEntity>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                    .With(x => x.CommitmentId, CommitmentOne)
                    .With(x => x.Payable, false)
                    .CreateMany(4)
                    .ToList();

                Earnings[0].Period = 4;
                Earnings[0].DeliveryMonth = 4.DeliveryMonthFromPeriod();
                Earnings[0].DeliveryYear = 4.DeliveryYearFromPeriod();

                Earnings[1].Period = 5;
                Earnings[1].DeliveryMonth = 5.DeliveryMonthFromPeriod();
                Earnings[1].DeliveryYear = 5.DeliveryYearFromPeriod();

                Earnings[2].Period = 6;
                Earnings[2].DeliveryMonth = 6.DeliveryMonthFromPeriod();
                Earnings[2].DeliveryYear = 6.DeliveryYearFromPeriod();


                for (var i = 0; i < 4; i++)
                {
                    datalockForNextYearFirstCommitment[i].Period = i + 4;
                }

                PastPayments[0].DeliveryMonth = 11;
                PastPayments[0].DeliveryYear = 2017;
                PastPayments[0].TransactionType = 1;
                PastPayments[0].AmountDue = 250;

                PastPayments[1].DeliveryMonth = 12;
                PastPayments[1].DeliveryYear = 2017;
                PastPayments[1].TransactionType = 1;
                PastPayments[1].AmountDue = 250;

                PastPayments[2].DeliveryMonth = 1;
                PastPayments[2].DeliveryYear = 2018;
                PastPayments[2].TransactionType = 1;
                PastPayments[2].AmountDue = 250;
                
                PastPayments[3].DeliveryMonth = 2;
                PastPayments[3].DeliveryYear = 2018;
                PastPayments[3].TransactionType = 1;
                PastPayments[3].AmountDue = 250;

                PastPayments[4].DeliveryMonth = 3;
                PastPayments[4].DeliveryYear = 2018;
                PastPayments[4].TransactionType = 13;
                PastPayments[4].AmountDue = 117.75000m;
                PastPayments[4].LearnAimRef = LearnAimRefForEnglish;

                PastPayments[5].DeliveryMonth = 3;
                PastPayments[5].DeliveryYear = 2018;
                PastPayments[5].TransactionType = 1;
                PastPayments[5].AmountDue = 250;

                PastPayments[6].DeliveryMonth = 4;
                PastPayments[6].DeliveryYear = 2018;
                PastPayments[6].TransactionType = 13;
                PastPayments[6].AmountDue = 117.75000m;
                PastPayments[6].LearnAimRef = LearnAimRefForEnglish;

                PastPayments[7].DeliveryMonth = 4;
                PastPayments[7].DeliveryYear = 2018;
                PastPayments[7].TransactionType = 13;
                PastPayments[7].AmountDue = 157.00000m;
                PastPayments[7].LearnAimRef = LearnAimRefForMaths;


                PastPayments[8].DeliveryMonth = 4;
                PastPayments[8].DeliveryYear = 2018;
                PastPayments[8].TransactionType = 1;
                PastPayments[8].AmountDue = 250;

                PastPayments[9].DeliveryMonth = 2;
                PastPayments[9].DeliveryYear = 2018;
                PastPayments[9].TransactionType = 1;
                PastPayments[9].AmountDue = -250;

                PastPayments[10].DeliveryMonth = 3;
                PastPayments[10].DeliveryYear = 2018;
                PastPayments[10].TransactionType = 13;
                PastPayments[10].AmountDue = -117.75000m;
                PastPayments[10].LearnAimRef = LearnAimRefForEnglish;


                PastPayments[11].DeliveryMonth = 4;
                PastPayments[11].DeliveryYear = 2018;
                PastPayments[11].TransactionType = 13;
                PastPayments[11].AmountDue = -157.00000m;
                PastPayments[11].LearnAimRef = LearnAimRefForMaths;


                PastPayments[12].DeliveryMonth = 4;
                PastPayments[12].DeliveryYear = 2018;
                PastPayments[12].TransactionType = 13;
                PastPayments[12].AmountDue = -117.75000m;
                PastPayments[12].LearnAimRef = LearnAimRefForEnglish;


                Datalocks.AddRange(datalockForNextYearFirstCommitment);
            }

            [Test, PaymentsDueAutoData]
            public void ThenThereShouldBeNoRefundsForThePeriodsWithdrawnFromTheIlr(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService commitmentMatcher)
            {
                var datalockOutput = commitmentMatcher.GetSuccessfulDatalocks(
                    Datalocks, 
                    new List<DatalockValidationError>(), 
                    Commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

                var datalockResult = datalock.DeterminePayableEarnings(
                    datalockOutput,
                    Earnings,
                    MathsAndEnglishEarnings);

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, PastPayments);

                actual.Should().HaveCount(2);
            }

            [Test, PaymentsDueAutoData]
            public void ThenTheRefundAmountShouldBeCorrect(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService commitmentMatcher)
            {
                var datalockOutput = commitmentMatcher.GetSuccessfulDatalocks(Datalocks, 
                    new List<DatalockValidationError>(),
                    Commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });
                var datalockResult = datalock.DeterminePayableEarnings(
                    datalockOutput,
                    Earnings,
                    MathsAndEnglishEarnings);

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, PastPayments);

                actual.Sum(x => x.AmountDue).Should().Be(-500);
            }
        }
    }
}
