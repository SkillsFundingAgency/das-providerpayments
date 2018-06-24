using System.Linq;
using AutoFixture.NUnit3;
using Castle.Core.Logging;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting
{
    [TestFixture]
    public class ScenarioTesting
    {
        [Theory]
        [PaymentsDueInlineAutoData("DuplicateDataLocks")]
        [PaymentsDueInlineAutoData("IncentivePaidButNotOnprog")]
        [PaymentsDueInlineAutoData("SecondIncentivePaidWithOnprog")]
        [PaymentsDueInlineAutoData("MathsEnglishWIthNoOnprogInYear")]
        [PaymentsDueInlineAutoData("OverlappingDatalocks")]
        [PaymentsDueInlineAutoData("Act1MathsEnglishWIthNoOnprogInYear")]
        [PaymentsDueInlineAutoData("Act1MathsEnglish")]
        public void ThenThePaymentsGeneratedShouldMatchTheExpectedPayments(
            string filename,
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            IDetermineWhichEarningsShouldBePaid datalock,
            PaymentsDueCalculationService sut,
            CollectionPeriodEntity collectionPeriod)
        {
            var parameters = TestData.LoadFrom(filename);

            var commitmentMatcher = new DatalockValidationService(NullLogger.Instance);
            var datalockOutput = commitmentMatcher.ProcessDatalocks(
                parameters.DatalockOutputs,
                parameters.DatalockValidationErrors,
                parameters.Commitments);

            collectionPeriod.AcademicYear = "1718";
            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(collectionPeriod);

            var datalockResult = datalock.DeterminePayableEarnings(
                datalockOutput,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish);

            var actual = sut.Calculate(
                datalockResult.Earnings,
                datalockResult.PeriodsToIgnore,
                parameters.PastPayments);

            actual.Should().HaveCount(parameters.Payments.Count);
            actual.Sum(x => x.AmountDue).Should().Be(parameters.Payments.Sum(x => x.AmountDue));

            foreach (var payment in parameters.Payments)
            {
                actual.Should().Contain(x =>
                    x.TransactionType == payment.TransactionType &&
                    x.AmountDue == payment.AmountDue &&
                    x.PriceEpisodeIdentifier == payment.PriceEpisodeIdentifier &&
                    x.DeliveryMonth == payment.DeliveryMonth &&
                    x.DeliveryYear == payment.DeliveryYear);
            }
        }
    }
}
