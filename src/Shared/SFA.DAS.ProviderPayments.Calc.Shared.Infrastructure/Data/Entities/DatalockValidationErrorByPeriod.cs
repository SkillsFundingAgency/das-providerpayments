﻿using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class DatalockValidationErrorByPeriod
    {
        public long Ukprn { get; set; }
        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        public int AimSeqNumber { get; set; }
        public string RuleId { get; set; }
        [StringLength(25)]
        public string PriceEpisodeIdentifier { get; set; }
        [Range(1, 12)]
        public int Period { get; set; }
    }
}
