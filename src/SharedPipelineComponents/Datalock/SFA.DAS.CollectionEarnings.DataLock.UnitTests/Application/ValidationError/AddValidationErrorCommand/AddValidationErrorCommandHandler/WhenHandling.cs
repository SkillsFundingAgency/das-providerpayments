using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler
{
    public class WhenHandling
    {
        private static readonly CollectionEarnings.DataLock.Application.ValidationError.ValidationError ValidationError
            = new CollectionEarnings.DataLock.Application.ValidationError.ValidationError
            {
                Ukprn = 10007459,
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 1,
                RuleId = DataLockErrorCodes.MismatchingUkprn,
                PriceEpisodeIdentifier = "20-25-01/08/2016"
            };

        private Mock<IValidationErrorRepository> _validationErrorRepository;

        private AddValidationErrorCommandRequest _request;
        private CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new Mock<IValidationErrorRepository>();

            _request = new AddValidationErrorCommandRequest
            {
                ValidationError = ValidationError
            };

            _handler = new CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler(_validationErrorRepository.Object);
        }

        [Test]
        public void ThenSuccessfullForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenItShouldWriteTheValidationErrorToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _validationErrorRepository.Verify(r => r.AddValidationError(It.Is<ValidationErrorEntity>(ve => ValidationErrorsMatch(ve, ValidationError))));
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationError(It.IsAny<ValidationErrorEntity>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool ValidationErrorsMatch(ValidationErrorEntity entity, CollectionEarnings.DataLock.Application.ValidationError.ValidationError validationError)
        {
            return entity.Ukprn == validationError.Ukprn &&
                   entity.LearnRefNumber == validationError.LearnerReferenceNumber &&
                   entity.AimSeqNumber == validationError.AimSequenceNumber &&
                   entity.RuleId == validationError.RuleId &&
                   entity.PriceEpisodeIdentifier == validationError.PriceEpisodeIdentifier;
        }
    }
}