using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels.ProviderAdjustments
{
    public class EasSubmission
    {
        public void AddValue(EasSubmissionValues value)
        {
            value.SubmissionId = SubmissionId;
            Values.Add(value);
        }

        public Guid SubmissionId { get; set; } = Guid.NewGuid();
        public long Ukprn { get; set; }
        public int CollectionPeriod { get; set; }
        public string ProviderName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool DeclarationChecked { get; set; }
        public bool NilReturn { get; set; }
        public string UpdatedBy { get; set; }

        public List<EasSubmissionValues> Values { get; set; } = new List<EasSubmissionValues>();
    }
}
