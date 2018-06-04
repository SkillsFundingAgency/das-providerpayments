using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class ProviderProcessor : IProviderProcessor
    {
        private readonly IProviderLearnersBuilder _providerLearnersBuilder;
        private readonly ILearnerEarningDataLockMatcher _earningDataLockMatcher;

        public ProviderProcessor(IProviderLearnersBuilder providerLearnersBuilder,
            ILearnerEarningDataLockMatcher earningDataLockMatcher)
        {
            _providerLearnersBuilder = providerLearnersBuilder;
            _earningDataLockMatcher = earningDataLockMatcher;
        }

        public void Process(ProviderEntity provider)
        {
            var providerLearners = _providerLearnersBuilder.Build(provider.Ukprn);

            foreach (var learner in providerLearners)
            {
                _earningDataLockMatcher.Match(learner.Value);
                /*learner.Value.MatchEarningsAndDataLocks();//todo: will save to payable and nonpayable lists on the learner
                learner.Value.MatchMathsEnglishEarningsAndDataLocks();//todo: will save to payable and nonpayable lists on the learner

                learner.Value.IssuePayments();//todo: will group by course, sum the group and record in required pmts table*/
            }
        }
    }
}