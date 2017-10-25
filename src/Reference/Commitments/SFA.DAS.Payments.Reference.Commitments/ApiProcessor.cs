using System;
using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Commitments.Application;
using SFA.DAS.Payments.Reference.Commitments.Application.AddErrorCommand;
using SFA.DAS.Payments.Reference.Commitments.Application.AddOrUpdateCommitmentCommand;
using SFA.DAS.Payments.Reference.Commitments.Application.GetLastSeenEventIdQuery;
using SFA.DAS.Payments.Reference.Commitments.Application.GetNextBatchOfCommitmentEventsQuery;
using SFA.DAS.Payments.Reference.Commitments.Application.SetLastSeenEventIdCommand;
using EventsPaymentStatus = SFA.DAS.Events.Api.Types.PaymentStatus;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Reference.Commitments
{
    public class ApiProcessor
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ApiProcessor(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        protected ApiProcessor()
        {
            // For mocking
        }

        public virtual void Process()
        {
            _logger.Info("Started Commitments Api Processor.");

            var currentEventId = 0L;
            var lastSeenEventId = 0L;
            try
            {
                Events.Api.Types.ApprenticeshipEventView[] events;
                while ((events = GetNextBatchOfCommitments()).Length > 0)
                {
                    foreach (var @event in events)
                    {
                        currentEventId = @event.Id;
                        var paymentStatus = GetPaymentStatusOrThrow(@event.PaymentStatus);

                        if (paymentStatus == PaymentStatus.PendingApproval || paymentStatus == PaymentStatus.Deleted)
                        {
                            lastSeenEventId = @event.Id;
                            continue;
                        }

                        if (@event.PriceHistory != null && ((List<Events.Api.Types.PriceHistory>)@event.PriceHistory).Count > 0)
                        {
                            var request = ConvertEventToCommand(@event, paymentStatus);
                            _mediator.Send(request);
                        }
                        else
                        {
                            _logger.Warn($"PriceHistory for {@event.ApprenticeshipId} is null or empty so commitment will not be created.");
                        }
                        lastSeenEventId = @event.Id;
                    }

                    _mediator.Send(new SetLastSeenEventIdCommandRequest { LastSeenEventId = lastSeenEventId });
                }
            }
            catch (Exception ex)
            {
                var wrappedError = new Exception($"Error processing commitment event stream at {currentEventId} - {ex.Message}", ex);

                _logger.Error(wrappedError);
                _mediator.Send(new AddErrorCommandRequest { Error = wrappedError });
            }
            finally
            {
                _mediator.Send(new SetLastSeenEventIdCommandRequest { LastSeenEventId = lastSeenEventId });
            }
            _logger.Info("Finished Commitments Api Processor.");
        }

        private Events.Api.Types.ApprenticeshipEventView[] GetNextBatchOfCommitments()
        {
            var lastSeenEventId = _mediator.Send(new GetLastSeenEventIdQueryRequest()).EventId;
            if (lastSeenEventId > 0)
            {
                lastSeenEventId++;
            }
            var response = _mediator.Send(new GetNextBatchOfCommitmentEventsQueryRequest { LastSeenEventId = lastSeenEventId });
            if (!response.IsValid)
            {
                throw response.Exception;
            }
            return response.Items ?? new Events.Api.Types.ApprenticeshipEventView[0];
        }

        private AddOrUpdateCommitmentCommandRequest ConvertEventToCommand(Events.Api.Types.ApprenticeshipEventView @event, PaymentStatus paymentStatus)
        {

            if (!@event.TrainingTotalCost.HasValue)
            {
                _logger.Error($"TrainingTotalCost for {@event.ApprenticeshipId} is null so so default value will be used.");
            }
            if (!@event.TrainingStartDate.HasValue)
            {
                _logger.Error($"TrainingStartDate for {@event.ApprenticeshipId} is null so so default value will be used.");
            }
            if (!@event.TrainingEndDate.HasValue)
            {
                _logger.Error($"TrainingEndDate for {@event.ApprenticeshipId} is null so default value will be used.");
            }


            var request = new AddOrUpdateCommitmentCommandRequest
            {
                CommitmentId = @event.ApprenticeshipId,
                Ukprn = long.Parse(@event.ProviderId),
                Uln = GetUlnOrDefault(@event.LearnerId),
                AccountId = long.Parse(@event.EmployerAccountId),
                StartDate = @event.TrainingStartDate.HasValue ? @event.TrainingStartDate.Value : DateTime.MinValue,
                EndDate = @event.TrainingEndDate.HasValue ? @event.TrainingEndDate.Value : DateTime.MinValue,
                VersionId = @event.Id,
                Priority = @event.PaymentOrder,
                PaymentStatus = paymentStatus,
                LegalEntityName = @event.LegalEntityName
            };


            ((List<Events.Api.Types.PriceHistory>)@event.PriceHistory).
                ForEach(x =>
                request.PriceEpisodes.Add(new PriceEpisode
                {
                    AgreedPrice = x.TotalCost,
                    EffectiveFromDate = x.EffectiveFrom,
                    EffectiveToDate = x.EffectiveTo
                }
                )
            );

            if (@event.TrainingType == Events.Api.Types.TrainingTypes.Standard)
            {
                request.StandardCode = long.Parse(@event.TrainingId);
            }
            else if (@event.TrainingType == Events.Api.Types.TrainingTypes.Framework)
            {
                var parts = @event.TrainingId.Split('-');
                request.FrameworkCode = int.Parse(parts[0]);
                request.ProgrammeType = int.Parse(parts[1]);
                request.PathwayCode = int.Parse(parts[2]);
            }

            return request;
        }

        private long GetUlnOrDefault(string learnerId)
        {
            long uln;

            return long.TryParse(learnerId, out uln)
                ? uln
                : 0L;
        }

        private PaymentStatus GetPaymentStatusOrThrow(EventsPaymentStatus paymentStatus)
        {
            PaymentStatus status;

            switch (paymentStatus)
            {
                case EventsPaymentStatus.Active:
                    status = PaymentStatus.Active;
                    break;
                case EventsPaymentStatus.Withdrawn:
                    status = PaymentStatus.Withdrawn;
                    break;
                case EventsPaymentStatus.Completed:
                    status = PaymentStatus.Completed;
                    break;
                case EventsPaymentStatus.Deleted:
                    status = PaymentStatus.Deleted;
                    break;
                case EventsPaymentStatus.Paused:
                    status = PaymentStatus.Paused;
                    break;
                case EventsPaymentStatus.PendingApproval:
                    status = PaymentStatus.PendingApproval;
                    break;
                default:
                    throw new ArgumentException($"Invalid payment status of {paymentStatus} found.");

            }

            return status;
        }

        private bool IsValidCommitmentData(Events.Api.Types.ApprenticeshipEventView @event)
        {
            var result = true;
            if (!@event.TrainingTotalCost.HasValue)
            {
                _logger.Error($"TrainingTotalCost for {@event.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }
            if (!@event.TrainingStartDate.HasValue)
            {
                _logger.Error($"TrainingStartDate for {@event.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }
            if (!@event.TrainingEndDate.HasValue)
            {
                _logger.Error($"TrainingEndDate for {@event.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }

            if (@event.PriceHistory == null || ((List<Events.Api.Types.PriceHistory>)@event.PriceHistory).Count == 0)
            {
                _logger.Error($"PriceHistory for {@event.ApprenticeshipId} is null or empty so commitment will not be created.");
                result = false;
            }

            return result;
        }



    }
}
