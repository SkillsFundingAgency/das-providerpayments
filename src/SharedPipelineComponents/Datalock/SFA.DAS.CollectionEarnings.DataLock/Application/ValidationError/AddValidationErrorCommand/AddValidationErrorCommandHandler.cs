using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandHandler : IRequestHandler<AddValidationErrorCommandRequest, Unit>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public Unit Handle(AddValidationErrorCommandRequest message)
        {
            var validationErrorEntity = new ValidationErrorEntity
            {
                Ukprn = message.ValidationError.Ukprn,
                LearnRefNumber = message.ValidationError.LearnerReferenceNumber,
                AimSeqNumber = message.ValidationError.AimSequenceNumber,
                RuleId = message.ValidationError.RuleId,
                PriceEpisodeIdentifier = message.ValidationError.PriceEpisodeIdentifier
            };

            _validationErrorRepository.AddValidationError(validationErrorEntity);

            return Unit.Value;
        }
    }
}