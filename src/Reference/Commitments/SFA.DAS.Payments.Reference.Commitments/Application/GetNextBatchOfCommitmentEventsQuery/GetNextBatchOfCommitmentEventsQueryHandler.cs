using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Payments.Reference.Commitments.Application.GetNextBatchOfCommitmentEventsQuery
{
    public class GetNextBatchOfCommitmentEventsQueryHandler : IRequestHandler<GetNextBatchOfCommitmentEventsQueryRequest, GetNextBatchOfCommitmentEventsQueryResponse>
    {
        private readonly IEventsApi _eventsApiClient;

        private readonly ILogger _logger;

        public GetNextBatchOfCommitmentEventsQueryHandler(IEventsApi eventsApiClient, ILogger logger)
        {
            _eventsApiClient = eventsApiClient;
            _logger = logger;
        }

        public GetNextBatchOfCommitmentEventsQueryResponse Handle(GetNextBatchOfCommitmentEventsQueryRequest message)
        {
            try
            {
                var events = _eventsApiClient.GetApprenticeshipEventsById(message.LastSeenEventId).Result;

                var eventsToReturn = CleanInvalidEventPriceHistory(events);

                return new GetNextBatchOfCommitmentEventsQueryResponse
                {
                    IsValid = true,
                    Items = eventsToReturn.ToArray()
                };
            }
            catch (AggregateException ex)
            {
                return new GetNextBatchOfCommitmentEventsQueryResponse
                {
                    IsValid = false,
                    Exception = ex.InnerExceptions.First()
                };
            }
        }

        private static IEnumerable<ApprenticeshipEventView> CleanInvalidEventPriceHistory(List<ApprenticeshipEventView> events)
        {
            if (events == null || !events.Any())
            {
                return events;
            }

            events.ForEach(e =>
            {
                if (e.PriceHistory != null && e.PriceHistory.Any())
                {
                    var priceHistoryList = e.PriceHistory.ToList();
                    e.PriceHistory = priceHistoryList.Where(h => EffectiveFromIsBeforeEffectiveTo(h.EffectiveFrom, h.EffectiveTo)).ToList();
                }
            });

            return events.Where(e => EffectiveFromIsBeforeEffectiveTo(e.EffectiveFrom, e.EffectiveTo));
        }

        private static bool EffectiveFromIsBeforeEffectiveTo(DateTime? effectiveFrom, DateTime? effectiveTo)
        {
            return !effectiveFrom.HasValue || ( !effectiveTo.HasValue || effectiveFrom.Value <= effectiveTo.Value);
        }
    }
}