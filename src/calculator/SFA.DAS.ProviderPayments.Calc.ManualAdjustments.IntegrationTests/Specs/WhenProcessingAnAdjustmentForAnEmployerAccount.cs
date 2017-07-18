using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.Specs
{
    public class WhenProcessingAnAdjustmentForAnEmployerAccount
    {
        private ManualAdjustmentsTask _task;
        private TestExternalTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _task = new ManualAdjustmentsTask();

            _taskContext = new TestExternalTaskContext();
        }

        [Test]
        public void ThenItShouldCreateReversingRequiredPayment()
        {
            // Arrange
            TestDataHelper.WriteOpenCollectionPeriod("R02", 9, 2016);

            var manualAdjustmentRequested = TestDataSets.GetManualAdjustmentEntity();
            TestDataHelper.WriteAdjustment(manualAdjustmentRequested);

            var requiredPaymentMadePreviously = TestDataSets.GetRequiredPaymentEntity(manualAdjustmentRequested.RequiredPaymentIdToReverse.ToString());
            requiredPaymentMadePreviously.CollectionPeriodName = "1617";
            requiredPaymentMadePreviously.CollectionPeriodMonth = 8;
            requiredPaymentMadePreviously.CollectionPeriodYear = 2016;
            TestDataHelper.WriteRequiredPayment(requiredPaymentMadePreviously);

            var paymentsMadePerviously = TestDataSets.GetPayments(requiredPaymentMadePreviously, true, true);
            foreach (var payment in paymentsMadePerviously)
            {
                TestDataHelper.WritePayment(payment);
            }

            // Act
            TestDataHelper.CopyDataToTransient();
            _task.Execute(_taskContext);

            // Assert
            var actualRequiredPayments = TestDataHelper.GetRequiredPayments();
            Assert.AreEqual(1, actualRequiredPayments.Length);
            Assert.AreEqual(requiredPaymentMadePreviously.AccountId, actualRequiredPayments[0].AccountId);
            Assert.AreEqual(requiredPaymentMadePreviously.AccountVersionId, actualRequiredPayments[0].AccountVersionId);
            Assert.AreEqual(requiredPaymentMadePreviously.AimSeqNumber, actualRequiredPayments[0].AimSeqNumber);
            Assert.AreEqual(-requiredPaymentMadePreviously.AmountDue, actualRequiredPayments[0].AmountDue);
            Assert.AreEqual(requiredPaymentMadePreviously.ApprenticeshipContractType, actualRequiredPayments[0].ApprenticeshipContractType);
            Assert.AreEqual(requiredPaymentMadePreviously.CommitmentId, actualRequiredPayments[0].CommitmentId);
            Assert.AreEqual(requiredPaymentMadePreviously.CommitmentVersionId, actualRequiredPayments[0].CommitmentVersionId);
            Assert.AreEqual(requiredPaymentMadePreviously.DeliveryMonth, actualRequiredPayments[0].DeliveryMonth);
            Assert.AreEqual(requiredPaymentMadePreviously.DeliveryYear, actualRequiredPayments[0].DeliveryYear);
            Assert.AreEqual(requiredPaymentMadePreviously.FrameworkCode, actualRequiredPayments[0].FrameworkCode);
            Assert.AreEqual(requiredPaymentMadePreviously.FundingLineType, actualRequiredPayments[0].FundingLineType);
            Assert.AreEqual(requiredPaymentMadePreviously.IlrSubmissionDateTime, actualRequiredPayments[0].IlrSubmissionDateTime);
            Assert.AreEqual(requiredPaymentMadePreviously.LearnRefNumber, actualRequiredPayments[0].LearnRefNumber);
            Assert.AreEqual(requiredPaymentMadePreviously.PathwayCode, actualRequiredPayments[0].PathwayCode);
            Assert.AreEqual(requiredPaymentMadePreviously.PriceEpisodeIdentifier, actualRequiredPayments[0].PriceEpisodeIdentifier);
            Assert.AreEqual(requiredPaymentMadePreviously.ProgrammeType, actualRequiredPayments[0].ProgrammeType);
            Assert.AreEqual(requiredPaymentMadePreviously.SfaContributionPercentage, actualRequiredPayments[0].SfaContributionPercentage);
            Assert.AreEqual(requiredPaymentMadePreviously.StandardCode, actualRequiredPayments[0].StandardCode);
            Assert.AreEqual(requiredPaymentMadePreviously.TransactionType, actualRequiredPayments[0].TransactionType);
            Assert.AreEqual(requiredPaymentMadePreviously.Ukprn, actualRequiredPayments[0].Ukprn);
            Assert.AreEqual(requiredPaymentMadePreviously.Uln, actualRequiredPayments[0].Uln);
            Assert.AreEqual(requiredPaymentMadePreviously.UseLevyBalance, actualRequiredPayments[0].UseLevyBalance);
        }
    }
}
