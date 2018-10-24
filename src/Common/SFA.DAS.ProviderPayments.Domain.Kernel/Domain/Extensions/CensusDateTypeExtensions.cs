using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class CensusDateTypeExtensions
    {
        public static string DependentPropertyName(this CensusDateType source)
        {
            switch (source)
            {
                case CensusDateType.All:
                    return null;
                case CensusDateType.OnProgLearning:
                    return null;
                case CensusDateType.First16To18Incentive:
                    return nameof(RawEarning.FirstIncentiveCensusDate);
                case CensusDateType.Second16To18Incentive:
                    return nameof(RawEarning.SecondIncentiveCensusDate);
                case CensusDateType.CompletionPayments:
                    return nameof(RawEarning.EndDate);
                case CensusDateType.LearnerIncentive:
                    return "changeme when it's merged";
            }

            return "error";
        }

        public static List<TransactionType> ValidTransactionTypes(this CensusDateType source)
        {
            switch (source)
            {
                case CensusDateType.All:
                    return Enum.GetValues(typeof(TransactionType)).OfType<TransactionType>().ToList();
                case CensusDateType.OnProgLearning:
                    return new List<TransactionType>
                    {
                        TransactionType.Learning,
                        TransactionType.OnProgramme16To18FrameworkUplift,
                        TransactionType.FirstDisadvantagePayment,
                        TransactionType.SecondDisadvantagePayment,
                        TransactionType.OnProgrammeMathsAndEnglish,
                        TransactionType.BalancingMathsAndEnglish,
                        TransactionType.LearningSupport,
                    };
                case CensusDateType.First16To18Incentive:
                    return new List<TransactionType>
                    {
                        TransactionType.First16To18EmployerIncentive,
                        TransactionType.First16To18ProviderIncentive,
                    };
                case CensusDateType.Second16To18Incentive:
                    return new List<TransactionType>
                    {
                        TransactionType.Second16To18EmployerIncentive,
                        TransactionType.Second16To18ProviderIncentive,
                    };
                case CensusDateType.CompletionPayments:
                    return new List<TransactionType>
                    {
                        TransactionType.Completion,
                        TransactionType.Balancing,
                        TransactionType.Completion16To18FrameworkUplift,
                        TransactionType.Balancing16To18FrameworkUplift,
                    };
                case CensusDateType.LearnerIncentive:
                    return new List<TransactionType>
                    {

                    };
            }

            return new List<TransactionType>();
        }
    }
}
