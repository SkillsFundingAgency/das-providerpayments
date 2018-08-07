using System.Collections.Generic;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryRequest : IRequest<RunDataLockValidationQueryResponse>
    {
        public IEnumerable<CommitmentEntity> Commitments { get; set; }
        public List<RawEarning> PriceEpisodes { get; set; }
        public IEnumerable<DasAccount.DasAccount> DasAccounts { get; set; }

        public IEnumerable<IncentiveEarnings> IncentiveEarnings { get; set; }
    }
}