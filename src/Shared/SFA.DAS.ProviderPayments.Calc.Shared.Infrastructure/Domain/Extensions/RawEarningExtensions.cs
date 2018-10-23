using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Domain.Extensions
{
    public static class RawEarningExtensions
    {
        public static bool HasNonZeroTransactions(this RawEarning earning)
        {
            return earning.HasOnProgLearningTransactions() ||
                   earning.HasFirst16To18IncentiveTransactions() ||
                   earning.HasSecond16To18IncentiveTransactions() ||
                   earning.HasCompletionTransactions() ||
                   earning.HasLearnerIncentiveTransactions();
        }

        public static bool HasOnProgLearningTransactions(this RawEarning earning)
        {
            return earning.TransactionType01 != 0 ||
                   earning.TransactionType08 != 0 ||
                   earning.TransactionType11 != 0 ||
                   earning.TransactionType12 != 0 ||
                   earning.TransactionType13 != 0 ||
                   earning.TransactionType14 != 0 ||
                   earning.TransactionType15 != 0;
        }

        public static bool HasFirst16To18IncentiveTransactions(this RawEarning earning)
        {
            return earning.TransactionType04 != 0 ||
                   earning.TransactionType05 != 0;
        }

        public static bool HasSecond16To18IncentiveTransactions(this RawEarning earning)
        {
            return earning.TransactionType06 != 0 ||
                   earning.TransactionType07 != 0;
        }

        public static bool HasCompletionTransactions(this RawEarning earning)
        {
            return earning.TransactionType02 != 0 ||
                   earning.TransactionType03 != 0 ||
                   earning.TransactionType09 != 0 ||
                   earning.TransactionType10 != 0;
        }

        public static bool HasLearnerIncentiveTransactions(this RawEarning earning)
        {
            return false;
        }


        public static bool HasNonZeroTransactionsForCensusDateType(this RawEarning earning, CensusDateType censusDateType)
        {
            switch (censusDateType)
            {
                case CensusDateType.All:
                    return earning.HasNonZeroTransactions();
                case CensusDateType.OnProgLearning:
                    return earning.HasOnProgLearningTransactions();
                case CensusDateType.CompletionPayments:
                    return earning.HasCompletionTransactions();
                case CensusDateType.First16To18Incentive:
                    return earning.HasFirst16To18IncentiveTransactions();
                case CensusDateType.Second16To18Incentive:
                    return earning.HasSecond16To18IncentiveTransactions();
                case CensusDateType.LearnerIncentive:
                    return earning.HasLearnerIncentiveTransactions();
            }

            return false;
        }
    }
}
