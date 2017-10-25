using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand
{
    public class AddPriceEpisodeMatchesCommandHandler : IRequestHandler<AddPriceEpisodeMatchesCommandRequest, Unit>
    {
        private readonly IPriceEpisodeMatchRepository _priceEpisodeMatchRepository;

        public AddPriceEpisodeMatchesCommandHandler(IPriceEpisodeMatchRepository priceEpisodeMatchRepository)
        {
            _priceEpisodeMatchRepository = priceEpisodeMatchRepository;
        }

        public Unit Handle(AddPriceEpisodeMatchesCommandRequest message)
        {
            var priceEpisodeMatchEntities = message.PriceEpisodeMatches
                .Select(
                    lc => new PriceEpisodeMatchEntity
                    {
                        Ukprn = lc.Ukprn,
                        LearnRefNumber = lc.LearnerReferenceNumber,
                        AimSeqNumber = lc.AimSequenceNumber,
                        CommitmentId = lc.CommitmentId,
                        PriceEpisodeIdentifier = lc.PriceEpisodeIdentifier,
                        IsSuccess=lc.IsSuccess
                    })
                .ToArray();

            _priceEpisodeMatchRepository.AddPriceEpisodeMatches(priceEpisodeMatchEntities);

            return Unit.Value;
        }
    }
}