using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryRequest : IRequest<RunDataLockValidationQueryResponse>
    {
        public IEnumerable<Commitment.Commitment> Commitments { get; set; }
        public IEnumerable<PriceEpisode.PriceEpisode> PriceEpisodes { get; set; }
        public IEnumerable<DasAccount.DasAccount> DasAccounts { get; set; }
    }
}