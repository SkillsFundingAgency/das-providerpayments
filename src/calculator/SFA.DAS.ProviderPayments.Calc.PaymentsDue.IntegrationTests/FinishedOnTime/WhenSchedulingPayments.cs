using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenSchedulingPayments
    {
        private readonly IExternalTask _task = new PaymentsDueTask();

        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        public void ThenNoPaymentIsScheduledForACollectionPeriodOutsideTheLearningPeriod()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 1, 10);
            var endDate = startDate.AddYears(1);
            TestDataHelper.AddCommitment(commitmentId, ukprn, startDate: startDate, endDate: endDate);

            TestDataHelper.AddEarningForCommitment(commitmentId, startDate: startDate, endDate: endDate);

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
            var scheduledPayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(scheduledPayments);
            Assert.AreEqual(0, scheduledPayments.Length);
        }

        [Test]
        public void ThenALearningPaymentIsScheduledForTheEarningsWhenTheLearningIsNotComplete()
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
            var scheduledPayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(scheduledPayments);
            Assert.AreEqual(1, scheduledPayments.Length);

            Assert.AreEqual((int)TransactionType.Learning, scheduledPayments[0].TransactionType);
            Assert.AreEqual(1000.00m, scheduledPayments[0].AmountDue);
        }

        [Test]
        public void ThenACompletionPaymentIsScheduledForTheEarningsWhenTheLearningIsComplete()
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
            var scheduledPayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(scheduledPayments);
            Assert.AreEqual(1, scheduledPayments.Length);

            Assert.AreEqual((int)TransactionType.Completion, scheduledPayments[0].TransactionType);
            Assert.AreEqual(3000.00m, scheduledPayments[0].AmountDue);
        }

        [Test]
        public void ThenALearningPaymentAndACompletionPaymentAreScheduledForTheEarningsWhenTheLearningIsCompletedOnACensusDate()
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
            var scheduledPayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.IsNotNull(scheduledPayments);
            Assert.AreEqual(2, scheduledPayments.Length);

            Assert.AreEqual(1, scheduledPayments.Count(p => (int)TransactionType.Learning == p.TransactionType && 1000.00m == p.AmountDue));
            Assert.AreEqual(1, scheduledPayments.Count(p => (int)TransactionType.Completion == p.TransactionType && 3000.00m == p.AmountDue));
        }
    }
}