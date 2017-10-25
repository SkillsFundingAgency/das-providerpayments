using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Application.Entities
{
    public class CommitmentEntity
    {
        public long CommitmentId { get; set; }
        public int VersionId { get; set; }
        public string EmployerKey { get; set; }
        public string LearnerKey { get; set; }
        public int Priority { get; set; }
        public string ProviderKey { get; set; }
        public int AgreedPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int Status { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public long Uln { get; set; }



    }
}
