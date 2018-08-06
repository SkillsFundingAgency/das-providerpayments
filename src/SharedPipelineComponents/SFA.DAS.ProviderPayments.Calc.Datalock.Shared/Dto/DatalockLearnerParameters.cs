using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Dto
{
    class DatalockLearnerParameters
    {
        public List<Commitment> Commitments { get; set; }
        public List<RawEarning> PriceEpisodes { get; set; }
    }
}
