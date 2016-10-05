using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenMakingDuePayments
    {
        private readonly IExternalTask _task = new PaymentsDueTask();

        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        public void ThenItShouldMakePaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            TestDataHelper.AddCommitment(commitmentId, ukprn, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, currentPeriod: 5);


            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);
            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual(2, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2018, duePayments[0].DeliveryYear);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
            Assert.AreEqual(TransactionType.Learning, duePayments[0].TransactionType);
        }

        //[Test]
        public void ThenNoPaymentIsDueForACollectionPeriodOutsideTheLearningPeriod()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 1, 10);
            var endDate = startDate.AddYears(1);
            TestDataHelper.AddCommitment(commitmentId, ukprn, startDate: startDate, endDate: endDate);

            TestDataHelper.AddEarningForCommitment(commitmentId, startDate: startDate, endDate: endDate);

            _context = new ExternalContextStub();

            // Act
            _task.Execute(_context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(duePayments);
            Assert.AreEqual(0, duePayments.Length);
        }

        //[Test]
        public void ThenALearningPaymentIsDueForTheEarningsWhenTheLearningIsNotComplete()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, ukprn);

            TestDataHelper.AddEarningForCommitment(commitmentId);

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.ConnectionString},
                    {ContextPropertyKeys.LogLevel, "DEBUG"}
                }
            };

            // Act
            _task.Execute(_context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(duePayments);
            Assert.AreEqual(1, duePayments.Length);

            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);
            Assert.AreEqual(1000.00m, duePayments[0].AmountDue);
        }

        //[Test]
        public void ThenACompletionPaymentIsSDueForTheEarningsWhenTheLearningIsComplete()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2016, 8, 1);
            var endDate = startDate.AddYears(1);
            TestDataHelper.AddCommitment(commitmentId, ukprn, startDate: startDate, endDate: endDate);

            TestDataHelper.AddEarningForCommitment(commitmentId, startDate: startDate, endDate: endDate, actualEndDate: endDate);

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.ConnectionString},
                    {ContextPropertyKeys.LogLevel, "DEBUG"}
                }
            };

            // Act
            _task.Execute(_context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(duePayments);
            Assert.AreEqual(1, duePayments.Length);

            Assert.AreEqual((int)TransactionType.Completion, duePayments[0].TransactionType);
            Assert.AreEqual(3000.00m, duePayments[0].AmountDue);
        }

        //[Test]
        public void ThenALearningPaymentAndACompletionPaymentAreDueForTheEarningsWhenTheLearningIsCompletedOnACensusDate()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2016, 8, 1);
            var endDate = startDate.AddYears(1).LastDayOfMonth();
            TestDataHelper.AddCommitment(commitmentId, ukprn, startDate: startDate, endDate: endDate);

            TestDataHelper.AddEarningForCommitment(commitmentId, startDate: startDate, endDate: endDate, actualEndDate: endDate);

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.ConnectionString},
                    {ContextPropertyKeys.LogLevel, "DEBUG"}
                }
            };

            // Act
            _task.Execute(_context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(duePayments);
            Assert.AreEqual(2, duePayments.Length);

            Assert.AreEqual(1, duePayments.Count(p => (int)TransactionType.Learning == p.TransactionType && 1000.00m == p.AmountDue));
            Assert.AreEqual(1, duePayments.Count(p => (int)TransactionType.Completion == p.TransactionType && 3000.00m == p.AmountDue));
        }
    }
}