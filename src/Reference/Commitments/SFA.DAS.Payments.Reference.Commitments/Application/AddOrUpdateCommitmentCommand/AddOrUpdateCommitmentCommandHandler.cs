using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Payments.Reference.Commitments.Application.AddOrUpdateCommitmentCommand
{
    public class AddOrUpdateCommitmentCommandHandler : IRequestHandler<AddOrUpdateCommitmentCommandRequest, Unit>
    {
        private readonly ICommitmentRepository _commitmentRepository;
        private readonly ILogger _logger;

        public AddOrUpdateCommitmentCommandHandler(ICommitmentRepository commitmentRepository, ILogger logger)
        {
            _commitmentRepository = commitmentRepository;
            _logger = logger;
        }

        public Unit Handle(AddOrUpdateCommitmentCommandRequest message)
        {
            var commitmentsList = new List<CommitmentEntity>();
            var counter = 1;
            foreach (var priceEpisode in message.PriceEpisodes)
            {
                var commitment = new CommitmentEntity
                {
                    CommitmentId = message.CommitmentId,
                    Uln = message.Uln,
                    Ukprn = message.Ukprn,
                    AccountId = message.AccountId,
                    StartDate = message.StartDate,
                    EndDate = message.EndDate,
                    StandardCode = message.StandardCode,
                    FrameworkCode = message.FrameworkCode,
                    ProgrammeType = message.ProgrammeType,
                    PathwayCode = message.PathwayCode,
                    VersionId = $"{message.VersionId}-{counter.ToString("000")}",
                    Priority = message.Priority,
                    PaymentStatus = (int)message.PaymentStatus,
                    PausedOnDate = message.PausedOnDate,
                    WithdrawnOnDate = message.WithdrawnOnDate,
                    PaymentStatusDescription = message.PaymentStatus.ToString(),
                    LegalEntityName = message.LegalEntityName,
                    /* properties from price episode*/
                    AgreedCost = priceEpisode.AgreedPrice,
                    EffectiveFromDate = priceEpisode.EffectiveFromDate,
                    EffectiveToDate = priceEpisode.EffectiveToDate,
                    TransferSendingEmployerAccountId = message.TransferSendingEmployerAccountId,
                    TransferApprovalDate = message.TransferApprovalDate
                };
                counter++;
                commitmentsList.Add(commitment);
            }

            //check to see if there was a material change to any price episode, if yes then delete all versions of the same commitment and repopulate
            if (ShouldReRepopulateAllVersions(commitmentsList))
            {
                //delete all versions of the commitment
                _commitmentRepository.Delete(message.CommitmentId);

                //Now we need to re-populate all commitment versions from the price episodes we have received
                foreach (var commitment in commitmentsList)
                {
                    _commitmentRepository.Insert(commitment);
                    _commitmentRepository.InsertHistory(commitment);
                }
            }

            return Unit.Value;
        }

        private bool ShouldReRepopulateAllVersions(List<CommitmentEntity> commitments)
        {
            var result = false;
            foreach (var commitment in commitments)
            {
                var sameCommitmentExists = _commitmentRepository.CommitmentExists(commitment);
                if (!sameCommitmentExists)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

    }
}