using System;
using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Reference.Commitments.Application.AddOrUpdateCommitmentCommand
{
    public class AddOrUpdateCommitmentCommandRequest : IRequest
    {
        public AddOrUpdateCommitmentCommandRequest()
        {
            PriceEpisodes = new List<PriceEpisode>();
        }
        public long CommitmentId { get; set; }
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long StandardCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }
        public int PathwayCode { get; set; }
        public long VersionId { get; set; }
        public int Priority { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PausedOnDate { get; set; }
        public DateTime? WithdrawnOnDate { get; set; }
        public string LegalEntityName { get; set; }
        public List<PriceEpisode> PriceEpisodes { get; set; }
        public long? TransferSendingEmployerAccountId { get; set; }
        public DateTime? TransferApprovalDate { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
    }
}
