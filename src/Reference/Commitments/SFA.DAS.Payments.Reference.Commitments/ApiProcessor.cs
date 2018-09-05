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
                    foreach (var apprenticeshipEvent in events)
                    {
                        currentEventId = apprenticeshipEvent.Id;
                        var paymentStatus = GetPaymentStatusOrThrow(apprenticeshipEvent.PaymentStatus);

                        if (paymentStatus == PaymentStatus.PendingApproval || paymentStatus == PaymentStatus.Deleted)
                        {
                            lastSeenEventId = apprenticeshipEvent.Id;
                            continue;
                        }

                        if (apprenticeshipEvent.PriceHistory != null && ((List<Events.Api.Types.PriceHistory>)apprenticeshipEvent.PriceHistory).Count > 0)
                        {
                            var request = ConvertEventToCommand(apprenticeshipEvent, paymentStatus);
                            _mediator.Send(request);
                        }
                        else
                        {
                            _logger.Warn($"PriceHistory for {apprenticeshipEvent.ApprenticeshipId} is null or empty so commitment will not be created.");
                        }
                        lastSeenEventId = apprenticeshipEvent.Id;
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

        private AddOrUpdateCommitmentCommandRequest ConvertEventToCommand(Events.Api.Types.ApprenticeshipEventView apprenticeshipEvent, PaymentStatus paymentStatus)
        {

            if (!apprenticeshipEvent.TrainingTotalCost.HasValue)
            {
                _logger.Error($"TrainingTotalCost for {apprenticeshipEvent.ApprenticeshipId} is null so so default value will be used.");
            }
            if (!apprenticeshipEvent.TrainingStartDate.HasValue)
            {
                _logger.Error($"TrainingStartDate for {apprenticeshipEvent.ApprenticeshipId} is null so so default value will be used.");
            }
            if (!apprenticeshipEvent.TrainingEndDate.HasValue)
            {
                _logger.Error($"TrainingEndDate for {apprenticeshipEvent.ApprenticeshipId} is null so default value will be used.");
            }

            var request = new AddOrUpdateCommitmentCommandRequest
            {
                CommitmentId = apprenticeshipEvent.ApprenticeshipId,
                Ukprn = long.Parse(apprenticeshipEvent.ProviderId),
                Uln = GetUlnOrDefault(apprenticeshipEvent.LearnerId),
                AccountId = long.Parse(apprenticeshipEvent.EmployerAccountId),
                StartDate = apprenticeshipEvent.TrainingStartDate ?? DateTime.MinValue,
                EndDate = apprenticeshipEvent.TrainingEndDate ?? DateTime.MinValue,
                VersionId = apprenticeshipEvent.Id,
                Priority = apprenticeshipEvent.PaymentOrder,
                PaymentStatus = paymentStatus,
                PausedOnDate = apprenticeshipEvent.PausedOnDate,
                WithdrawnOnDate = apprenticeshipEvent.StoppedOnDate,
                LegalEntityName = apprenticeshipEvent.LegalEntityName,
                TransferSendingEmployerAccountId = apprenticeshipEvent.TransferSenderId,
                TransferApprovalDate = apprenticeshipEvent.TransferApprovalActionedOn,
                AccountLegalEntityPublicHashedId = apprenticeshipEvent.AccountLegalEntityPublicHashedId
            };
            
            ((List<Events.Api.Types.PriceHistory>)apprenticeshipEvent.PriceHistory).
                ForEach(x =>
                request.PriceEpisodes.Add(new PriceEpisode
                {
                    AgreedPrice = x.TotalCost,
                    EffectiveFromDate = x.EffectiveFrom,
                    EffectiveToDate = x.EffectiveTo
                }
                )
            );

            if (apprenticeshipEvent.TrainingType == Events.Api.Types.TrainingTypes.Standard)
            {
                request.StandardCode = long.Parse(apprenticeshipEvent.TrainingId);
            }
            else if (apprenticeshipEvent.TrainingType == Events.Api.Types.TrainingTypes.Framework)
            {
                var parts = apprenticeshipEvent.TrainingId.Split('-');
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

        private bool IsValidCommitmentData(Events.Api.Types.ApprenticeshipEventView apprenticeshipEvent)
        {
            var result = true;
            if (!apprenticeshipEvent.TrainingTotalCost.HasValue)
            {
                _logger.Error($"TrainingTotalCost for {apprenticeshipEvent.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }
            if (!apprenticeshipEvent.TrainingStartDate.HasValue)
            {
                _logger.Error($"TrainingStartDate for {apprenticeshipEvent.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }
            if (!apprenticeshipEvent.TrainingEndDate.HasValue)
            {
                _logger.Error($"TrainingEndDate for {apprenticeshipEvent.ApprenticeshipId} is null so commitment will not be created.");
                result = false;
            }

            if (apprenticeshipEvent.PriceHistory == null || ((List<Events.Api.Types.PriceHistory>)apprenticeshipEvent.PriceHistory).Count == 0)
            {
                _logger.Error($"PriceHistory for {apprenticeshipEvent.ApprenticeshipId} is null or empty so commitment will not be created.");
                result = false;
            }

            return result;
        }
    }
}
