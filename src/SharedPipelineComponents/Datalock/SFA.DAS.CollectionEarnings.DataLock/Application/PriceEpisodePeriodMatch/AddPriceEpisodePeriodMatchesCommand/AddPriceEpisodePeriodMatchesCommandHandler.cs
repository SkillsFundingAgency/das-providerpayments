using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand
{
    public class AddPriceEpisodePeriodMatchesCommandHandler : IRequestHandler<AddPriceEpisodePeriodMatchesCommandRequest, Unit>
    {
        private readonly IPriceEpisodePeriodMatchRepository _priceEpisodePeriodMatchRepository;

        public AddPriceEpisodePeriodMatchesCommandHandler(IPriceEpisodePeriodMatchRepository priceEpisodePeriodMatchRepository)
        {
            _priceEpisodePeriodMatchRepository = priceEpisodePeriodMatchRepository;
        }

        public Unit Handle(AddPriceEpisodePeriodMatchesCommandRequest message)
        {
            var priceEpisodePeriodMatchEntities = message.PriceEpisodePeriodMatches
                .Select(
                    m => new PriceEpisodePeriodMatchEntity
                    {
                        Ukprn = m.Ukprn,
                        PriceEpisodeIdentifier = m.PriceEpisodeIdentifier,
                        LearnRefNumber = m.LearnerReferenceNumber,
                        AimSeqNumber = m.AimSequenceNumber,
                        CommitmentId = m.CommitmentId,
                        VersionId = m.CommitmentVersionId,
                        Period = m.Period,
                        Payable = m.Payable,
                        TransactionType= m.TransactionType,
                        TransactionTypesFlag = m.TransactionTypesFlag
                    })
                .ToArray();

            _priceEpisodePeriodMatchRepository.AddPriceEpisodePeriodMatches(priceEpisodePeriodMatchEntities);

            return Unit.Value;
        }
    }
}