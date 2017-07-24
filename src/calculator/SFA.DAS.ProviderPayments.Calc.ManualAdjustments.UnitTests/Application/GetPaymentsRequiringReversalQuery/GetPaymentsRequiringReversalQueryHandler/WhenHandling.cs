using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.GetPaymentsRequiringReversalQuery;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.Application.GetPaymentsRequiringReversalQuery.GetPaymentsRequiringReversalQueryHandler
{
    public class WhenHandling
    {
        private Guid[] _requiredPaymentIdsToReverse = 
        {
            Guid.Parse("5cebe7f2-dbee-42d3-a5ed-89158a3a9134"),
            Guid.Parse("2ee35f65-17a8-461a-b7b0-51773860474f")
        };

        private Mock<IManualAdjustmentRepository> _manualAdjustmentRepository;
        private ManualAdjustments.Application.GetPaymentsRequiringReversalQuery.GetPaymentsRequiringReversalQueryHandler _handler;
        private GetPaymentsRequiringReversalQueryRequest _request;

        [SetUp]
        public void Arrange()
        {
            _manualAdjustmentRepository = new Mock<IManualAdjustmentRepository>();
            _manualAdjustmentRepository.Setup(r => r.GetRequiredPaymentIdsToReverse())
                .Returns(_requiredPaymentIdsToReverse);

            _handler = new ManualAdjustments.Application.GetPaymentsRequiringReversalQuery.GetPaymentsRequiringReversalQueryHandler(_manualAdjustmentRepository.Object);

            _request = new GetPaymentsRequiringReversalQueryRequest();
        }

        [Test]
        public void ThenItShouldReturnAValidResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }

        [Test]
        public void ThenItShouldReturnPaymentIdsTheNeedReversing()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(2, actual.Items.Length);
            Assert.AreEqual(_requiredPaymentIdsToReverse[0].ToString(), actual.Items[0]);
            Assert.AreEqual(_requiredPaymentIdsToReverse[1].ToString(), actual.Items[1]);
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfRepositoryThrowsException()
        {
            // Arrange
            var innerException = new Exception("An error just happened");
            _manualAdjustmentRepository.Setup(r => r.GetRequiredPaymentIdsToReverse())
                .Throws(innerException);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.AreSame(innerException, actual.Exception);
        }
    }
}
