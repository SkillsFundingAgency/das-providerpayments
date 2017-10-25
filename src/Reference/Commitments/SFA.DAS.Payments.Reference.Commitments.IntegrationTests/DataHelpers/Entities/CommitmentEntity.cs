﻿using System;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers.Entities
{
    public class CommitmentEntity
    {
        public long CommitmentId { get; set; }
        public int Priority { get; set; }
        public string VersionId { get; set; }

        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public long AccountId { get; set; }


        public long StandardCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }
        public int PathwayCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal AgreedCost { get; set; }

        public int PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
     
        public DateTime EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }

        public string LegalEntityName { get; set; }
    }
}
