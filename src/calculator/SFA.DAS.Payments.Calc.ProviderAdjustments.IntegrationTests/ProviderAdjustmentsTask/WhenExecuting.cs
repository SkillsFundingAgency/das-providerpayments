using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Tools;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.ProviderAdjustmentsTask
{
    public class WhenExecuting
    {
        private ProviderAdjustments.ProviderAdjustmentsTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _context = new IntegrationTaskContext();
            _task = new ProviderAdjustments.ProviderAdjustmentsTask();
        }

        [Test]
        public void ThenItShouldWriteFullPaymentsWhenNoPreviousAdjustmentsArePresent()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProviderAdjustmentsSubmission(ukprn);

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var payments = TestDataHelper.GetPaymentsForProvider(ukprn);

            Assert.IsNotNull(payments);
            Assert.AreEqual(12, payments.Count(p => p.Ukprn == ukprn && p.Amount == 1000.00m));
        }

        [Test]
        public void ThenItShouldWriteAdjustedPaymentsWhenPreviousAdjustmentsArePresent()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProviderAdjustmentsSubmission(ukprn, amount: 125.00m, currentPeriod: 6);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn, amount: 25.00m, currentPeriod: 5);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn, amount: 50.00m, currentPeriod: 4);

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var payments = TestDataHelper.GetPaymentsForProvider(ukprn);

            Assert.IsNotNull(payments);
            Assert.AreEqual(6, payments.Count(p => p.Ukprn == ukprn));
            Assert.AreEqual(1, payments.Count(p => p.Amount == 125.00m));
            Assert.AreEqual(1, payments.Count(p => p.Amount == 100.00m));
            Assert.AreEqual(4, payments.Count(p => p.Amount == 50.00m));
        }

        [Test]
        public void ThenItShouldNotWriteAnyPaymentsWhenCurrentAdjustmentsAreNotPresent()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProvider(ukprn);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn, amount: 25.00m, currentPeriod: 5);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn, amount: 50.00m, currentPeriod: 4);

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var payments = TestDataHelper.GetPaymentsForProvider(ukprn);

            Assert.IsNotNull(payments);
            Assert.AreEqual(0, payments.Length);
        }

        [Test]
        public void ThenItShouldWriteFullAndAdjustedPaymentsWhenMultipleProvidersArePresent()
        {
            // Arrange
            var ukprn1 = 10007459;
            var ukprn2 = 10007460;
            TestDataHelper.AddProviderAdjustmentsSubmission(ukprn1, amount: 125.00m, currentPeriod: 6);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn1, amount: 25.00m, currentPeriod: 5);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn1, amount: 50.00m, currentPeriod: 4);

            TestDataHelper.AddProviderAdjustmentsSubmission(ukprn2, amount: 225.00m);

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var payments1 = TestDataHelper.GetPaymentsForProvider(ukprn1);
            var payments2 = TestDataHelper.GetPaymentsForProvider(ukprn2);

            Assert.IsNotNull(payments1);
            Assert.AreEqual(6, payments1.Count(p => p.Ukprn == ukprn1));
            Assert.AreEqual(1, payments1.Count(p => p.Amount == 125.00m));
            Assert.AreEqual(1, payments1.Count(p => p.Amount == 100.00m));
            Assert.AreEqual(4, payments1.Count(p => p.Amount == 50.00m));

            Assert.IsNotNull(payments2);
            Assert.AreEqual(12, payments2.Count(p => p.Ukprn == ukprn2 && p.Amount == 225.00m));
        }
    }
}