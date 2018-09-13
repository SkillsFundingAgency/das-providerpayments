using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Helpers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting.GivenAnAct1Learner.WithWithdrawnCommitment
{
    [TestFixture]
    public class WithMultipleWithdrawnCommitmentsAndOneActiveButStillDatalocked
    {
        [Test, PaymentsDueAutoData]
        public void ThenThereShouldBeNoRefunds(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            IValidateRawDatalocks commitmentMatcher)
        {
            var parameters = TestData.LoadFrom("LearnerWithMultipleWithdrawnCommitmentAndOneActiveCommitment");

            var datalockOutput = commitmentMatcher.ProcessDatalocks(
                parameters.DatalockOutputs, 
                parameters.DatalockValidationErrors,
                parameters.Commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(
                datalockOutput,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish, CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            var actual = sut.Calculate(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);

            actual.Should().HaveCount(0);
        }
    }
}


