using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.SetAdjustmentAsReversedCommand;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.Application.SetAdjustmentAsReversedCommand.SetAdjustmentAsReversedCommandHandler
{
    public class WhenHandling
    {

        private Mock<IManualAdjustmentRepository> _manualAdjustmentRepository;
        private ManualAdjustments.Application.SetAdjustmentAsReversedCommand.SetAdjustmentAsReversedCommandHandler _handler;
        private SetAdjustmentAsReversedCommandRequest _request;

        [SetUp]
        public void Arrange()
        {
            _manualAdjustmentRepository = new Mock<IManualAdjustmentRepository>();

            _handler = new ManualAdjustments.Application.SetAdjustmentAsReversedCommand.SetAdjustmentAsReversedCommandHandler(_manualAdjustmentRepository.Object);

            _request = new SetAdjustmentAsReversedCommandRequest
            {
                RequiredPaymentIdToReverse = "5cebe7f2-dbee-42d3-a5ed-89158a3a9134",
                RequiredPaymentIdForReversal = "37f3a454-5e13-462f-b542-643717d95b48"
            };
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
        public void ThenItShouldReturnInvalidResponseIfRepositoryThrowsException()
        {
            // Arrange
            var innerException = new Exception("An error just happened");
            _manualAdjustmentRepository.Setup(r => r.SetRequiredPaymentIdAsReversed(It.IsAny<string>(), It.IsAny<string>()))
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
