using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler
{
    public class WhenHandling
    {
        private static readonly CollectionEarnings.DataLock.Application.ValidationError.ValidationError[] ValidationErrors =
            {
                new CollectionEarnings.DataLock.Application.ValidationError.ValidationError
                {
                    Ukprn = 10007459,
                    LearnerReferenceNumber = "Lrn001",
                    AimSequenceNumber = 1,
                    RuleId = DataLockErrorCodes.MismatchingUkprn,
                    PriceEpisodeIdentifier = "20-25-01/08/2016"
                },
                new CollectionEarnings.DataLock.Application.ValidationError.ValidationError
                {
                    Ukprn = 10007459,
                    LearnerReferenceNumber = "Lrn002",
                    AimSequenceNumber = 1,
                    RuleId = DataLockErrorCodes.MismatchingUkprn,
                    PriceEpisodeIdentifier = "20-25-09/08/2016"
                }
            };

        private Mock<IValidationErrorRepository> _validationErrorRepository;

        private AddValidationErrorsCommandRequest _request;
        private CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new Mock<IValidationErrorRepository>();

            _request = new AddValidationErrorsCommandRequest
            {
                ValidationErrors = ValidationErrors
            };

            _handler = new CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler(_validationErrorRepository.Object);
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
        public void ThenItShouldWriteTheValidationErrorsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _validationErrorRepository.Verify(r => r.AddValidationErrors(It.Is<ValidationErrorEntity[]>(ve => ValidationErrorsBatchesMatch(ve, ValidationErrors))));
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationErrors(It.IsAny<ValidationErrorEntity[]>()))
                .Throws(new Exception("Exception while writing validation errors."));

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool ValidationErrorsBatchesMatch(ValidationErrorEntity[] entities, CollectionEarnings.DataLock.Application.ValidationError.ValidationError[] validationErrors)
        {
            if (entities.Length != validationErrors.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!ValidationErrorsMatch(entities[x], validationErrors[x]))
                {
                    return false;
                }
            }

            return true;
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