using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.RemovePriceEpisodePeriodMatchesCommand
{
    public class RemovePriceEpisodePeriodMatchesCommandHandler : IRequestHandler<RemovePriceEpisodePeriodMatchesCommandRequest, Unit>
    {
        private readonly IPriceEpisodePeriodMatchRepository _priceEpisodePeriodMatchRepository;

        public RemovePriceEpisodePeriodMatchesCommandHandler(IPriceEpisodePeriodMatchRepository priceEpisodePeriodMatchRepository)
        {
            _priceEpisodePeriodMatchRepository = priceEpisodePeriodMatchRepository;
        }

        public Unit Handle(RemovePriceEpisodePeriodMatchesCommandRequest message)
        {
            _priceEpisodePeriodMatchRepository.RemoveExtraPriceEpisodePeriodMatches();

            return Unit.Value;
        }

        
    }
}