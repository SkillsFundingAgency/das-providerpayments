using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
   public interface IMatcher
    {
        MatchResult Match(IReadOnlyList<CommitmentEntity> commitments, RawEarning earning);
    }
}
