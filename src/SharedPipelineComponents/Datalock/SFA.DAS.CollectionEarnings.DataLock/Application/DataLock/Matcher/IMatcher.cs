using System;
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
   public interface IMatcher
    {
        MatchResult Match(IReadOnlyList<Commitment> commitments, RawEarning earning, DateTime censusDate);
    }
}
