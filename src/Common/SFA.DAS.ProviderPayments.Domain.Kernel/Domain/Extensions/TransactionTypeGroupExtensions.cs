using System;
using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class TransactionTypeGroupExtensions
    {
        public static string DependentPropertyName(this TransactionTypeGroup source)
        {
            switch (source)
            {
                case TransactionTypeGroup.OnProgLearning:
                    return null;
                case TransactionTypeGroup.NinetyDayIncentives:
                    return nameof(RawEarning.FirstIncentiveCensusDate);
                case TransactionTypeGroup.ThreeSixtyFiveDayIncentives:
                    return nameof(RawEarning.SecondIncentiveCensusDate);
                case TransactionTypeGroup.CompletionPayments:
                    return nameof(RawEarning.EndDate);
                case TransactionTypeGroup.SixtyDayIncentives:
                    return nameof(RawEarning.LearnerAdditionalPaymentsDate);
            }

            throw new Exception($"Please include the raw earning dependent property name for Transaction Type Group: {source}");
        }

        public static List<TransactionType> ValidTransactionTypes(this TransactionTypeGroup source)
        {
            switch (source)
            {
                case TransactionTypeGroup.OnProgLearning:
                    return new List<TransactionType>
                    {
                        TransactionType.Learning,
                        TransactionType.OnProgramme16To18FrameworkUplift,
                        TransactionType.OnProgrammeMathsAndEnglish,
                        TransactionType.BalancingMathsAndEnglish,
                        TransactionType.LearningSupport,
                    };
                case TransactionTypeGroup.NinetyDayIncentives:
                    return new List<TransactionType>
                    {
                        TransactionType.First16To18EmployerIncentive,
                        TransactionType.First16To18ProviderIncentive,
                        TransactionType.FirstDisadvantagePayment,
                    };
                case TransactionTypeGroup.ThreeSixtyFiveDayIncentives:
                    return new List<TransactionType>
                    {
                        TransactionType.Second16To18EmployerIncentive,
                        TransactionType.Second16To18ProviderIncentive,
                        TransactionType.SecondDisadvantagePayment,
                    };
                case TransactionTypeGroup.CompletionPayments:
                    return new List<TransactionType>
                    {
                        TransactionType.Completion,
                        TransactionType.Balancing,
                        TransactionType.Completion16To18FrameworkUplift,
                        TransactionType.Balancing16To18FrameworkUplift,
                    };
                case TransactionTypeGroup.SixtyDayIncentives:
                    return new List<TransactionType>
                    {
                        TransactionType.CareLeaverApprenticePayments,
                    };
            }

            return new List<TransactionType>();
        }
    }
}
