using System;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Events.Api.Client;

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
                return new GetNextBatchOfCommitmentEventsQueryResponse
                {
                    IsValid = true,
                    Items = events.ToArray()
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
    }
}