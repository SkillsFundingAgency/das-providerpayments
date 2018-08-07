using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
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
    public class GivenALearnerWithAPausedCommitment
    {
        private static readonly IFixture Fixture = new Fixture();
        private static readonly long CommitmentOne = Fixture.Create<long>();
        private static readonly string PriceEpisodeIdentifierForThisYear = "2-444-1-14/09/2017";
        
        private static readonly string LearnAimRefForZprog = Fixture.Create<string>();
        private static readonly string LearnAimRefForMaths = Fixture.Create<string>();
        private static readonly string LearnAimRefForEnglish = Fixture.Create<string>();
        private static readonly long AccountId = Fixture.Create<long>();
        private static readonly string FundingLineType = Fixture.Create<string>();

        private static readonly List<RequiredPayment> PastPayments = new List<RequiredPayment>();
        private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();

        private static readonly List<DatalockOutputEntity> Datalocks = new List<DatalockOutputEntity>();

        private static readonly List<Commitment> Commitments = new List<Commitment>
        {
            new Commitment
            {
                AccountId = AccountId,
                CommitmentId = CommitmentOne,
                StandardCode = 0,
                ProgrammeType = 2,
                FrameworkCode = 444,
                PathwayCode = 1,
                PaymentStatus = 2,
                IsLevyPayer = true,
            },
        };

        private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
            .With(x => x.ProgrammeType, 2)
            .With(x => x.StandardCode, 0)
            .With(x => x.FrameworkCode, 444)
            .With(x => x.PathwayCode, 1)
            .With(x => x.SfaContributionPercentage, 0.9m)
            .With(x => x.FundingLineType, FundingLineType)
            .With(x => x.LearnAimRef, LearnAimRefForZprog)
            .With(x => x.TransactionType01, 70.58824m)
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
            .With(x => x.ApprenticeshipContractType, 1)
            .CreateMany(4)
            .ToList();

        [SetUp]
        public void Setup()
        {
            var pastPayments = Fixture.Build<RequiredPayment>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 1)
                .With(x => x.AmountDue, 70.58824m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForZprog)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);
            pastPayments = Fixture.Build<RequiredPayment>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 13)
                .With(x => x.AmountDue, 27.70588m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForMaths)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);
            pastPayments = Fixture.Build<RequiredPayment>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 13)
                .With(x => x.AmountDue, 27.70588m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForEnglish)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);

            var mathsEarnings = Fixture.Build<RawEarningForMathsOrEnglish>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.StandardCode, 0)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .With(x => x.LearnAimRef, LearnAimRefForMaths)
                .With(x => x.TransactionType01, 0)
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
                .With(x => x.TransactionType13, 27.70588m)
                .With(x => x.TransactionType14, 0)
                .With(x => x.TransactionType15, 0)
                .With(x => x.ApprenticeshipContractType, 1)
                .CreateMany(4)
                .ToList();
            MathsAndEnglishEarnings.AddRange(mathsEarnings);

            var englishEarnings = Fixture.Build<RawEarningForMathsOrEnglish>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.StandardCode, 0)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .With(x => x.LearnAimRef, LearnAimRefForEnglish)
                .With(x => x.TransactionType01, 0)
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
                .With(x => x.TransactionType13, 27.70588m)
                .With(x => x.TransactionType14, 0)
                .With(x => x.TransactionType15, 0)
                .With(x => x.ApprenticeshipContractType, 1)
                .CreateMany(4)
                .ToList();
            MathsAndEnglishEarnings.AddRange(englishEarnings);


            var datalockForNextYearFirstCommitment = Fixture.Build<DatalockOutputEntity>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.Payable, false)
                .CreateMany(5)
                .ToList();
            
            for (var i = 0; i < 4; i++)
            {
                PastPayments[i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                PastPayments[i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                PastPayments[4 + i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                PastPayments[4 + i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                PastPayments[8 + i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                PastPayments[8 + i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                Earnings[i].Period = i + 2;
                Earnings[i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                Earnings[i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                MathsAndEnglishEarnings[i].Period = i + 2;
                MathsAndEnglishEarnings[i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                MathsAndEnglishEarnings[i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                MathsAndEnglishEarnings[4 + i].Period = i + 2;
                MathsAndEnglishEarnings[4 + i].DeliveryYear = (i + 2).DeliveryYearFromPeriod();
                MathsAndEnglishEarnings[4 + i].DeliveryMonth = (i + 2).DeliveryMonthFromPeriod();

                datalockForNextYearFirstCommitment[i].Period = i + 2;
            }

            datalockForNextYearFirstCommitment[4].Period = 6;

            Datalocks.AddRange(datalockForNextYearFirstCommitment);
        }

        [Test, PaymentsDueAutoData]
        public void ThereShouldBeNoRefunds(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            var datalockOutput = datalockValidator.ProcessDatalocks(
                Datalocks,
                new List<DatalockValidationError>(), 
                Commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });
            var datalockResult = datalock.DeterminePayableEarnings(
                datalockOutput, 
                Earnings, 
                MathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.Earnings, datalockResult.PeriodsToIgnore, PastPayments);

            actual.Should().NotContain(x => x.AmountDue < 0);
        }
    }
}
