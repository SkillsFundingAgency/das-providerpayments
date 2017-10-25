using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand
{
    public class AddPriceEpisodePeriodCommandHandler: IRequestHandler<AddPriceEpisodePeriodCommandRequest, Unit>
    {
        private readonly IApprenticeshipPriceEpisodePeriodRepository _repository;

        public AddPriceEpisodePeriodCommandHandler(IApprenticeshipPriceEpisodePeriodRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(AddPriceEpisodePeriodCommandRequest message)
        {
            var periodEarnings = message.PeriodEarnings
                .Select(pe =>
                    new ApprenticeshipPriceEpisodePeriodEntity
                    {
                        LearnRefNumber = pe.LearnerReferenceNumber,
                        PriceEpisodeIdentifier = pe.PriceEpisodeId,
                        Period = pe.Period,
                        PriceEpisodeBalancePayment = pe.PriceEpisodeBalancePayment,
                        PriceEpisodeCompletionPayment = pe.PriceEpisodeCompletionPayment,
                        PriceEpisodeOnProgPayment = pe.PriceEpisodeOnProgPayment
                    })
                .ToArray();

            _repository.AddApprenticeshipPriceEpisodePeriod(periodEarnings);

            return Unit.Value;
        }
    }
}