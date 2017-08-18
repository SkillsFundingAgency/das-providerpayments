using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests.TestComponents;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

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

            var requiredPaymentMadePreviously = TestDataSets.GetRequiredPaymentEntity(manualAdjustmentRequested.RequiredPaymentIdToReverse.ToString(), true);
            requiredPaymentMadePreviously.CollectionPeriodName = "1617";
            requiredPaymentMadePreviously.CollectionPeriodMonth = 8;
            requiredPaymentMadePreviously.CollectionPeriodYear = 2016;
            TestDataHelper.WriteRequiredPayment(requiredPaymentMadePreviously);

            var paymentsMadePerviously = TestDataSets.GetPayments(requiredPaymentMadePreviously, true, true);
            foreach (var payment in paymentsMadePerviously)
            {
                TestDataHelper.WritePayment(payment);
            }

            TestDataHelper.WriteEmployerAccount(requiredPaymentMadePreviously.AccountId, 10000);

            // Act
            TestDataHelper.CopyDataToTransient();
            _task.Execute(_taskContext);

            // Assert
            var actualRequiredPayments = TestDataHelper.GetRequiredPayments();
            Assert.AreEqual(1, actualRequiredPayments.Length);
            AssertPaymentsDueResults(actualRequiredPayments, requiredPaymentMadePreviously);

            var historicalActualRequiredPayments = TestDataHelper.GetHistoryRequiredPayments().Where(x=> x.AmountDue < 0);
            Assert.AreEqual(1, actualRequiredPayments.Length);
            AssertPaymentsDueResults(historicalActualRequiredPayments.ToArray(), requiredPaymentMadePreviously);

            var actualAdjustment = TestDataHelper.GetManualAdjustment(manualAdjustmentRequested.RequiredPaymentIdToReverse);
            Assert.AreEqual(actualAdjustment.DateUploaded.Date, manualAdjustmentRequested.DateUploaded.Date);
            Assert.AreEqual(actualAdjustment.ReasonForReversal, manualAdjustmentRequested.ReasonForReversal);
            Assert.AreEqual(actualAdjustment.RequestorName, manualAdjustmentRequested.RequestorName);
            Assert.AreEqual(actualAdjustment.RequiredPaymentIdToReverse, manualAdjustmentRequested.RequiredPaymentIdToReverse);
            Assert.IsNotNull(actualAdjustment.RequiredPaymentIdForReversal);
            Assert.AreNotEqual(actualAdjustment.RequiredPaymentIdForReversal,manualAdjustmentRequested.RequiredPaymentIdForReversal);
        }

        [Test]
        public void ThenItShouldCreateReversingPayments()
        {
            // Arrange
            TestDataHelper.WriteOpenCollectionPeriod("R02", 9, 2016);

            var manualAdjustmentRequested = TestDataSets.GetManualAdjustmentEntity();
            TestDataHelper.WriteAdjustment(manualAdjustmentRequested);

            var requiredPaymentMadePreviously = TestDataSets.GetRequiredPaymentEntity(manualAdjustmentRequested.RequiredPaymentIdToReverse.ToString(), true);
            requiredPaymentMadePreviously.CollectionPeriodName = "1617";
            requiredPaymentMadePreviously.CollectionPeriodMonth = 8;
            requiredPaymentMadePreviously.CollectionPeriodYear = 2016;
            TestDataHelper.WriteRequiredPayment(requiredPaymentMadePreviously);

            var paymentsMadePerviously = TestDataSets.GetPayments(requiredPaymentMadePreviously, true, true);
            foreach (var payment in paymentsMadePerviously)
            {
                TestDataHelper.WritePayment(payment);
            }

            TestDataHelper.WriteEmployerAccount(requiredPaymentMadePreviously.AccountId, 10000);

            // Act
            TestDataHelper.CopyDataToTransient();
            _task.Execute(_taskContext);

            // Assert
            var actualPayments = TestDataHelper.GetPayments();
            Assert.AreEqual(3, actualPayments.Length);

            AssertResults(actualPayments, paymentsMadePerviously);

            var historicalPayments = TestDataHelper.GetHistoricalPayments();
       
            AssertResults(historicalPayments.Where(x=> x.Amount < 0).ToArray(), paymentsMadePerviously,false);

        }

        [Test]
        public void ThenItShouldAdjustEmployersAccountBalance()
        {
            // Arrange
            TestDataHelper.WriteOpenCollectionPeriod("R02", 9, 2016);

            var manualAdjustmentRequested = TestDataSets.GetManualAdjustmentEntity();
            TestDataHelper.WriteAdjustment(manualAdjustmentRequested);

            var requiredPaymentMadePreviously = TestDataSets.GetRequiredPaymentEntity(manualAdjustmentRequested.RequiredPaymentIdToReverse.ToString(), true);
            requiredPaymentMadePreviously.CollectionPeriodName = "1617";
            requiredPaymentMadePreviously.CollectionPeriodMonth = 8;
            requiredPaymentMadePreviously.CollectionPeriodYear = 2016;
            TestDataHelper.WriteRequiredPayment(requiredPaymentMadePreviously);

            var paymentsMadePerviously = TestDataSets.GetPayments(requiredPaymentMadePreviously, true, true);
            foreach (var payment in paymentsMadePerviously)
            {
                TestDataHelper.WritePayment(payment);
            }

            TestDataHelper.WriteEmployerAccount(requiredPaymentMadePreviously.AccountId, 10000);

            // Act
            TestDataHelper.CopyDataToTransient();
            _task.Execute(_taskContext);

            // Assert
            var actualAccountBalance = TestDataHelper.GetEmployerAccountBalance(requiredPaymentMadePreviously.AccountId);
            var expectedAccountBalance = 10000 + paymentsMadePerviously.Single(p => p.FundingSource == 1).Amount;

            Assert.AreEqual(expectedAccountBalance, actualAccountBalance);
        }

            
        [Test]
        public void ThenItShouldCreateReversingRequiredPaymentForTwoPayments()
        {
            // Arrange
            SetupManualAdjustment("1");
            SetupManualAdjustment("2");


            // Act
            TestDataHelper.CopyDataToTransient();
            _task.Execute(_taskContext);

            //// Assert
           var actualRequiredPayments = TestDataHelper.GetRequiredPayments();
            Assert.AreEqual(actualRequiredPayments.Length, 2);

        }


        private void SetupManualAdjustment(string accountId)
        {
            TestDataHelper.WriteOpenCollectionPeriod("R02", 9, 2016);

            var manualAdjustmentRequested1 = TestDataSets.GetManualAdjustmentEntity();
            TestDataHelper.WriteAdjustment(manualAdjustmentRequested1);

            var requiredPaymentMadePreviously = TestDataSets.GetRequiredPaymentEntity(manualAdjustmentRequested1.RequiredPaymentIdToReverse.ToString(), true);
            requiredPaymentMadePreviously.AccountId = accountId;
            requiredPaymentMadePreviously.CollectionPeriodName = "1617";
            requiredPaymentMadePreviously.CollectionPeriodMonth = 8;
            requiredPaymentMadePreviously.CollectionPeriodYear = 2016;
            TestDataHelper.WriteRequiredPayment(requiredPaymentMadePreviously);

            var paymentsMadePerviously = TestDataSets.GetPayments(requiredPaymentMadePreviously, true, false);
            foreach (var payment in paymentsMadePerviously)
            {
                TestDataHelper.WritePayment(payment);
            }

            TestDataHelper.WriteEmployerAccount(requiredPaymentMadePreviously.AccountId, 10000);

        }

        private void AssertPaymentsDueResults(TestComponents.Entities.RequiredPaymentEntity[] actualRequiredPayments,
                                             TestComponents.Entities.RequiredPaymentEntity requiredPaymentMadePreviously,
                                             bool assertCollectionPeriod = true)
        {
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
            Assert.AreEqual(requiredPaymentMadePreviously.LearnAimRef, actualRequiredPayments[0].LearnAimRef);
            Assert.AreEqual(requiredPaymentMadePreviously.LearningStartDate, actualRequiredPayments[0].LearningStartDate);

        }

        private void AssertResults(TestComponents.Entities.PaymentEntity[] actualPayments, 
                                    TestComponents.Entities.PaymentEntity[] paymentsMadePerviously,
                                    bool assertCollectionPeriod = true)
        {

            var actualLevyPayment = actualPayments.SingleOrDefault(p => p.FundingSource == 1);
            var expectedLevyPayment = paymentsMadePerviously.Single(p => p.FundingSource == 1);
            Assert.IsNotNull(actualLevyPayment);
            Assert.AreEqual(expectedLevyPayment.DeliveryMonth, actualLevyPayment.DeliveryMonth);
            Assert.AreEqual(expectedLevyPayment.DeliveryYear, actualLevyPayment.DeliveryYear);
            if (assertCollectionPeriod)
            {
                Assert.AreEqual(9, actualLevyPayment.CollectionPeriodMonth);
                Assert.AreEqual(2016, actualLevyPayment.CollectionPeriodYear);
                Assert.AreEqual("1617-R02", actualLevyPayment.CollectionPeriodName);
            }
            Assert.AreEqual(expectedLevyPayment.TransactionType, actualLevyPayment.TransactionType);
            Assert.AreEqual(-expectedLevyPayment.Amount, actualLevyPayment.Amount);

            var actualGovernmentPayment = actualPayments.SingleOrDefault(p => p.FundingSource == 2);
            var expectedGovernmentPayment = paymentsMadePerviously.Single(p => p.FundingSource == 2);
            Assert.IsNotNull(actualGovernmentPayment);
            Assert.AreEqual(expectedGovernmentPayment.DeliveryMonth, actualGovernmentPayment.DeliveryMonth);
            Assert.AreEqual(expectedGovernmentPayment.DeliveryYear, actualGovernmentPayment.DeliveryYear);
            if (assertCollectionPeriod)
            {
                Assert.AreEqual(9, actualGovernmentPayment.CollectionPeriodMonth);
                Assert.AreEqual(2016, actualGovernmentPayment.CollectionPeriodYear);
                Assert.AreEqual("1617-R02", actualGovernmentPayment.CollectionPeriodName);
            }
            Assert.AreEqual(expectedGovernmentPayment.TransactionType, actualGovernmentPayment.TransactionType);
            Assert.AreEqual(-expectedGovernmentPayment.Amount, actualGovernmentPayment.Amount);

            var actualEmployerPayment = actualPayments.SingleOrDefault(p => p.FundingSource == 3);
            var expectedEmployerPayment = paymentsMadePerviously.Single(p => p.FundingSource == 3);
            Assert.IsNotNull(actualEmployerPayment);
            Assert.AreEqual(expectedEmployerPayment.DeliveryMonth, actualEmployerPayment.DeliveryMonth);
            Assert.AreEqual(expectedEmployerPayment.DeliveryYear, actualEmployerPayment.DeliveryYear);
            if (assertCollectionPeriod)
            {
                Assert.AreEqual(9, actualEmployerPayment.CollectionPeriodMonth);
                Assert.AreEqual(2016, actualEmployerPayment.CollectionPeriodYear);
                Assert.AreEqual("1617-R02", actualEmployerPayment.CollectionPeriodName);
            }
            Assert.AreEqual(expectedEmployerPayment.TransactionType, actualEmployerPayment.TransactionType);
            Assert.AreEqual(-expectedEmployerPayment.Amount, actualEmployerPayment.Amount);
        }
    }
}
