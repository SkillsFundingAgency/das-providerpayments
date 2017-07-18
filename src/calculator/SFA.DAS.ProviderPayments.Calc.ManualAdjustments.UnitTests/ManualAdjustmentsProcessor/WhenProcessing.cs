using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.GetPaymentsRequiringReversalQuery;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.SetAdjustmentAsReversedCommand;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.ManualAdjustmentsProcessor
{
    public class WhenProcessing
    {
        private string[] _requiredPaymentIdsToReverse = new[]
        {
            "5cebe7f2-dbee-42d3-a5ed-89158a3a9134",
            "2ee35f65-17a8-461a-b7b0-51773860474f"
        };
        private string[] _requiredPaymentIdsForReversal = new[]
        {
            "37f3a454-5e13-462f-b542-643717d95b48",
            "37c64a07-ede0-4d48-b982-0ce71a986e52"
        };

        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private ManualAdjustments.ManualAdjustmentsProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsRequiringReversalQueryRequest>()))
                .Returns(new GetPaymentsRequiringReversalQueryResponse
                {
                    IsValid = true,
                    Items = _requiredPaymentIdsToReverse
                });
            _mediator.Setup(m => m.Send(It.Is<ReversePaymentCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[0])))
                .Returns(new ReversePaymentCommandResponse
                {
                    IsValid = true,
                    RequiredPaymentIdForReversal = _requiredPaymentIdsForReversal[0]
                });
            _mediator.Setup(m => m.Send(It.Is<ReversePaymentCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[1])))
                .Returns(new ReversePaymentCommandResponse
                {
                    IsValid = true,
                    RequiredPaymentIdForReversal = _requiredPaymentIdsForReversal[1]
                });
            _mediator.Setup(m => m.Send(It.IsAny<SetAdjustmentAsReversedCommandRequest>()))
                .Returns(new SetAdjustmentAsReversedCommandResponse
                {
                    IsValid = true
                });

            _processor = new ManualAdjustments.ManualAdjustmentsProcessor(_logger.Object, _mediator.Object, "1617");
        }

        [Test]
        public void ThenItShouldReverseEachPaymentThatHasAnAdjustment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<ReversePaymentCommandRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<ReversePaymentCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[0])), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<ReversePaymentCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[1])), Times.Once);
        }

        [Test]
        public void ThenItShouldUpdateEachAdjustmentWithTheRequiredPaymentIdForTheReversal()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<SetAdjustmentAsReversedCommandRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<SetAdjustmentAsReversedCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[0] && r.RequiredPaymentIdForReversal == _requiredPaymentIdsForReversal[0])), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<SetAdjustmentAsReversedCommandRequest>(r => r.RequiredPaymentIdToReverse == _requiredPaymentIdsToReverse[1] && r.RequiredPaymentIdForReversal == _requiredPaymentIdsForReversal[1])), Times.Once);
        }

        [Test]
        public void ThenItShouldThrowExceptionIfGetPaymentsRequiringReversalQueryIsInvalid()
        {
            // Arrange
            var innerException = new Exception("Some error occured");
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentsRequiringReversalQueryRequest>()))
                .Returns(new GetPaymentsRequiringReversalQueryResponse
                {
                    IsValid = false,
                    Exception = innerException
                });

            // Act + Assert
            var actual = Assert.Throws<Exception>(() => _processor.Process());
            Assert.AreSame(innerException, actual);
        }

        [Test]
        public void ThenItShouldThrowExceptionIfReversePaymentCommandIsInvalid()
        {
            // Arrange
            var innerException = new Exception("Some error occured");
            _mediator.Setup(m => m.Send(It.IsAny<ReversePaymentCommandRequest>()))
                .Returns(new ReversePaymentCommandResponse
                {
                    IsValid = false,
                    Exception = innerException
                });

            // Act + Assert
            var actual = Assert.Throws<Exception>(() => _processor.Process());
            Assert.AreSame(innerException, actual);
        }

        [Test]
        public void ThenItShouldThrowExceptionIfSetAdjustmentAsReversedCommandIsInvalid()
        {
            // Arrange
            var innerException = new Exception("Some error occured");
            _mediator.Setup(m => m.Send(It.IsAny<SetAdjustmentAsReversedCommandRequest>()))
                .Returns(new SetAdjustmentAsReversedCommandResponse
                {
                    IsValid = false,
                    Exception = innerException
                });

            // Act + Assert
            var actual = Assert.Throws<Exception>(() => _processor.Process());
            Assert.AreSame(innerException, actual);
        }

    }
}
