using System;
using System.Collections.Generic;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class EarningValidationService 
    {
        private static readonly HashSet<TransactionType> PotentialLevyTransactionTypes = 
            new HashSet<TransactionType> { TransactionType.Learning, TransactionType.Completion, TransactionType.Balancing };
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public EarningValidationResult CreatePayableEarnings(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            CensusDateType cenususType = CensusDateType.All)
        {
            var earningValidationResult = new EarningValidationResult();
            
            var payables = GetFundingDueForNonZeroTransactionTypes(earnings, commitment, cenususType);

            earningValidationResult.AddPayableEarnings(payables);
            return earningValidationResult;
        }

        public EarningValidationResult CreateNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayables = GetNonPayableEarningsForNonZeroTransactionTypes(earnings, reason, paymentFailureReason, commitment);
            var earningValidationResult = new EarningValidationResult();
            earningValidationResult.AddNonPayableEarnings(nonPayables);
            return earningValidationResult;
        }

        public EarningValidationResult IgnorePeriod(int period)
        {
            var earningValidationResult = new EarningValidationResult();
            earningValidationResult.PeriodsToIgnore.Add(period);
            return earningValidationResult;
        }

        private List<FundingDue> GetFundingDueForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment,
            CensusDateType censusDateType)
        {
            var payableEarnings = new List<FundingDue>();
            foreach (var rawEarning in earnings)
            {
                payableEarnings.AddRange(GetPayableEarnings(rawEarning, commitment, censusDateType));
            }

            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();
            foreach (var rawEarning in earnings)
            {
                nonPayableEarnings.AddRange(GetNonPayableEarnings(rawEarning, reason, paymentFailureReason, commitment));
            }
            return nonPayableEarnings;
        }

        private List<FundingDue> GetPayableEarnings(
            RawEarning rawEarnings,
            IHoldCommitmentInformation commitmentInformation = null,
            CensusDateType censusDateType = CensusDateType.All)
        {
            var payableEarnings = ExpandEarning<FundingDue>(rawEarnings, commitmentInformation, censusDateType);
            
            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarnings(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            var nonPayableEarnings = ExpandEarning<NonPayableEarning>(rawEarnings, commitmentInformation);
            nonPayableEarnings.ForEach(x =>
            {
                x.PaymentFailureMessage = reason;
                x.PaymentFailureReason = paymentFailureReason;
            });
            
            return nonPayableEarnings;
        }

        private List<T> ExpandEarning<T>(
            RawEarning earning,
            IHoldCommitmentInformation commitment = null,
            CensusDateType censusDateType = CensusDateType.All)
            where T : FundingDue, new()
        {
            var expandedList = new List<T>();
            foreach (TransactionType transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int) transactionTypeValue;

                if (IgnoreTransactionTypeForCensusDateType(censusDateType, transactionTypeValue))
                {
                    continue;
                }

                var amountDue = (decimal) FundingDueAccessor[earning, $"TransactionType{transactionType:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var constructor = typeof(T).GetConstructor(new[] {typeof(RawEarning)});
                var funding = constructor?.Invoke(new object[] {earning}) as T;

                if (funding == null)
                {
                    throw new Exception($"Could not create {typeof(T).FullName} - failing process");
                }
                funding.TransactionType = transactionType;

                if (TransactionTypeIsSfaOnly(transactionTypeValue))
                {
                    funding.SfaContributionPercentage = 1;
                }

                // Doing this to prevent a huge switch statement
                funding.AmountDue = amountDue;
                commitment?.CopyCommitmentInformationTo(funding);

                expandedList.Add(funding);
            }

            return expandedList;
        }

        private static bool TransactionTypeIsSfaOnly(TransactionType transactionType) 
        {
            return !PotentialLevyTransactionTypes.Contains(transactionType);
        }

        private static bool IgnoreTransactionTypeForCensusDateType(CensusDateType censusDateType, TransactionType transactionType)
        {
            // TODO These all need to be enums
            if (censusDateType == CensusDateType.All)
            {
                return false;
            }

            if (censusDateType == CensusDateType.OnProgLearning && (
                    transactionType == TransactionType.Learning ||
                                        transactionType == TransactionType.OnProgramme16To18FrameworkUplift ||
                                        transactionType == TransactionType.FirstDisadvantagePayment ||
                                        transactionType == TransactionType.SecondDisadvantagePayment ||
                                        transactionType == TransactionType.OnProgrammeMathsAndEnglish ||
                                        transactionType == TransactionType.BalancingMathsAndEnglish ||
                                        transactionType == TransactionType.LearningSupport
                ))
            {
                return false;
            }

            if (censusDateType == CensusDateType.First16To18Incentive && 
                (transactionType == TransactionType.First16To18EmployerIncentive ||
                 transactionType == TransactionType.First16To18ProviderIncentive))

            {
                return false;
            }

            if (censusDateType == CensusDateType.Second16To18Incentive && 
                (transactionType == TransactionType.Second16To18EmployerIncentive ||
                 transactionType == TransactionType.Second16To18ProviderIncentive))
            {
                return false;
            }

            if (censusDateType == CensusDateType.CompletionPayments &&
                (transactionType == TransactionType.Balancing ||
                 transactionType == TransactionType.Completion ||
                 transactionType == TransactionType.Completion16To18FrameworkUplift ||
                 transactionType == TransactionType.Balancing16To18FrameworkUplift))
            {
                return false;
            }

            if (censusDateType == CensusDateType.LearnerIncentive && 
                //(transactionType == TransactionType.CareLeaverApprenticePayments))
            true)
            {
                return false;
            }

            return true;
        }
    }
}
