using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandHandler : IRequestHandler<AddValidationErrorsCommandRequest, Unit>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorsCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public Unit Handle(AddValidationErrorsCommandRequest message)
        {
            var validationErrorEntities = message.ValidationErrors
                .Select(
                    ve => new ValidationErrorEntity
                    {
                        Ukprn = ve.Ukprn,
                        LearnRefNumber = ve.LearnerReferenceNumber,
                        AimSeqNumber = ve.AimSequenceNumber,
                        RuleId = ve.RuleId,
                        PriceEpisodeIdentifier = ve.PriceEpisodeIdentifier
                    })
                .ToArray();

            _validationErrorRepository.AddValidationErrors(validationErrorEntities);

            return Unit.Value;
        }
    }
}