using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand
{
    public class ProcessAddPeriodisedValuesCommandHandler : IRequestHandler<ProcessAddPeriodisedValuesCommandRequest, Unit>
    {
        private readonly IApprenticeshipPriceEpisodePeriodisedValuesRepository _repository;

        public ProcessAddPeriodisedValuesCommandHandler(IApprenticeshipPriceEpisodePeriodisedValuesRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(ProcessAddPeriodisedValuesCommandRequest message)
        {
            var entities = message.PeriodisedValues
                .Select(pe =>
                    new ApprenticeshipPriceEpisodePeriodisedValuesEntity
                    {
                        LearnRefNumber = pe.LearnerReferenceNumber,
                        PriceEpisodeIdentifier = pe.PriceEpisodeId,
                        AttributeName = pe.AttributeName.ToString(),
                        Period_1 = pe.Period1Amount,
                        Period_2 = pe.Period2Amount,
                        Period_3 = pe.Period3Amount,
                        Period_4 = pe.Period4Amount,
                        Period_5 = pe.Period5Amount,
                        Period_6 = pe.Period6Amount,
                        Period_7 = pe.Period7Amount,
                        Period_8 = pe.Period8Amount,
                        Period_9 = pe.Period9Amount,
                        Period_10 = pe.Period10Amount,
                        Period_11 = pe.Period11Amount,
                        Period_12 = pe.Period12Amount
                    })
                .ToArray();

            _repository.AddApprenticeshipPriceEpisodePeriodisedValues(entities);

            return Unit.Value;
        }
    }
}