using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Dto
{
    class DatalockLearnerParameters
    {
        public List<Commitment> Commitments { get; set; }
        public List<RawEarning> PriceEpisodes { get; set; }
    }
}
