using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    class ProviderEarnings
    {
        private Dictionary<long, List<RawEarning>> EarningsByLearner { get; } = new Dictionary<long, List<RawEarning>>();

        public ProviderEarnings(IEnumerable<RawEarning> earnings)
        {
            if (earnings == null)
            {
                throw new ArgumentNullException(nameof(earnings));
            }

            var sortedEarnings = earnings.ToLookup(x => x.Uln);

            foreach (var grouping in sortedEarnings)
            {
                EarningsByLearner.Add(grouping.Key, grouping.ToList());
            }
        }

        public List<RawEarning> EarningsForLearner(long uln)
        {
            if (EarningsByLearner.ContainsKey(uln))
            {
                return EarningsByLearner[uln];
            }
            throw new LearnerNotFoundException();
        }

        public IEnumerable<long> AllUlns()
        {
            return EarningsByLearner.Keys;
        }
    }
}
