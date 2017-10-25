using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery
{
    public class GetProviderPriceEpisodesQueryHandler : IRequestHandler<GetProviderPriceEpisodesQueryRequest, GetProviderPriceEpisodesQueryResponse>
    {
        private readonly IPriceEpisodeRepository _priceEpisodeRepository;

        public GetProviderPriceEpisodesQueryHandler(IPriceEpisodeRepository priceEpisodeRepository)
        {
            _priceEpisodeRepository = priceEpisodeRepository;
        }

        public GetProviderPriceEpisodesQueryResponse Handle(GetProviderPriceEpisodesQueryRequest message)
        {
            try
            {
                var priceEpisodeEntities = _priceEpisodeRepository.GetProviderPriceEpisodes(message.Ukprn);

                return new GetProviderPriceEpisodesQueryResponse
                {
                    IsValid = true,
                    Items = priceEpisodeEntities?.Select(l =>
                    new PriceEpisode
                    {
                        Ukprn = l.Ukprn,
                        LearnerReferenceNumber = l.LearnRefNumber,
                        Uln = l.Uln,
                        NiNumber = l.NiNumber,
                        AimSequenceNumber = l.AimSeqNumber,
                        StandardCode = l.StandardCode,
                        ProgrammeType = l.ProgrammeType,
                        FrameworkCode = l.FrameworkCode,
                        PathwayCode = l.PathwayCode,
                        NegotiatedPrice = l.NegotiatedPrice,
                        StartDate = l.StartDate,
                        PriceEpisodeIdentifier = l.PriceEpisodeIdentifier,
                        EndDate = l.EndDate,
                        FirstAdditionalPaymentThresholdDate=l.FirstAdditionalPaymentThresholdDate,
                        SecondAdditionalPaymentThresholdDate=l.SecondAdditionalPaymentThresholdDate
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProviderPriceEpisodesQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}