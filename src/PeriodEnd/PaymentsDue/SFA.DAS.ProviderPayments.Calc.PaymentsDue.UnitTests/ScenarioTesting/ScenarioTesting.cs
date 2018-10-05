using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting
{
    [TestFixture]
    public class ScenarioTesting
    {
        [Test]
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
            Mock<IFilterOutCompletionPaymentsWithoutEvidence> completionPaymentFilter,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService paymentsDueCalc,
            DatalockValidationService commitmentMatcher,
            CollectionPeriodEntity collectionPeriod)
        {
            var testData = TestData.LoadFrom(filename);

            var sut = new LearnerPaymentsDueProcessor(LogManager.CreateNullLogger(), datalock, commitmentMatcher, paymentsDueCalc, completionPaymentFilter.Object);

            var parameters = new LearnerData(testData.LearnRefNumber, testData.Uln);
            parameters.RawEarnings.AddRange(testData.RawEarnings);
            parameters.Commitments.AddRange(testData.Commitments);
            parameters.DataLocks.AddRange(testData.DatalockOutputs);
            parameters.DatalockValidationErrors.AddRange(testData.DatalockValidationErrors);
            parameters.HistoricalRequiredPayments.AddRange(testData.PastPayments);
            parameters.RawEarningsMathsEnglish.AddRange(testData.RawEarningsForMathsOrEnglish);

            collectionPeriod.AcademicYear = "1718";
            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(collectionPeriod);

            var actual = sut.GetPayableAndNonPayableEarnings(parameters, testData.Ukprn);

            actual.PayableEarnings.Should().HaveCount(testData.Payments.Count);
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(testData.Payments.Sum(x => x.AmountDue));

            foreach (var payment in testData.Payments)
            {
                actual.PayableEarnings.Should().Contain(x =>
                    x.TransactionType == payment.TransactionType &&
                    x.AmountDue == payment.AmountDue &&
                    x.PriceEpisodeIdentifier == payment.PriceEpisodeIdentifier &&
                    x.DeliveryMonth == payment.DeliveryMonth &&
                    x.DeliveryYear == payment.DeliveryYear);
            }
        }
    }
}
