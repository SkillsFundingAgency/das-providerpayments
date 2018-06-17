using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class Learner : ILearner
    {
        public Learner(
            List<FundingDue> earnings,
            List<int> periodsToIgnore,
            List<RequiredPaymentEntity> pastPayments)
        {
            PastPayments = pastPayments;
            Earnings = earnings;
            PeriodsToIgnore = periodsToIgnore;
        }

        // Input
        public IReadOnlyList<FundingDue> Earnings { get; }
        public IReadOnlyList<int> PeriodsToIgnore { get; }
        public IReadOnlyList<RequiredPaymentEntity> PastPayments { get; }

        // Output
        public List<RequiredPaymentEntity> RequiredPayments { get; } = new List<RequiredPaymentEntity>();
        
        public List<RequiredPaymentEntity> CalculatePaymentsDue()
        {
            var processedGroups = new HashSet<MatchSetForPayments>();

            var groupedEarnings = Earnings.GroupBy(x => new MatchSetForPayments
                (
                    x.StandardCode,
                    x.FrameworkCode,
                    x.ProgrammeType,
                    x.PathwayCode,
                    x.ApprenticeshipContractType,
                    x.TransactionType,
                    x.SfaContributionPercentage,
                    x.LearnAimRef,
                    x.FundingLineType,
                    x.DeliveryYear,
                    x.DeliveryMonth,
                    x.AccountId)
            ).ToDictionary(x => x.Key, x => x.ToList());

            var groupedPastPayments = PastPayments.GroupBy(x => new MatchSetForPayments
            (
                x.StandardCode,
                x.FrameworkCode,
                x.ProgrammeType,
                x.PathwayCode,
                x.ApprenticeshipContractType,
                x.TransactionType,
                x.SfaContributionPercentage,
                x.LearnAimRef,
                x.FundingLineType,
                x.DeliveryYear,
                x.DeliveryMonth,
                x.AccountId)).ToDictionary(x => x.Key, x => x.ToList());

            // Payments for earnings
            foreach (var key in groupedEarnings.Keys)
            {
                processedGroups.Add(key);
                if (ShouldIgnoreEarnings(key))
                {
                    continue;
                }

                var earnings = groupedEarnings[key];
                var pastPayments = new List<RequiredPaymentEntity>();

                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPayments = groupedPastPayments[key];
                }

                var payment = earnings.Sum(x => x.AmountDue) -
                              pastPayments.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(earnings.First(), payment);
                }
            }

            // Refunds for past payments that don't have corresponding earnings
            foreach (var key in groupedPastPayments.Keys)
            {
                if (processedGroups.Contains(key))
                {
                    // Already processed
                    continue;
                }

                if (ShouldIgnoreEarnings(key))
                {
                    continue;
                }
                
                var pastPayments = groupedPastPayments[key];

                var payment = -pastPayments.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(pastPayments.First(), payment);
                }
            }

            return RequiredPayments;
        }

        private bool ShouldIgnoreEarnings(MatchSetForPayments earningInformation)
        {
            var period = PeriodFromDeliveryMonth(earningInformation.DeliveryMonth);
            if (PeriodsToIgnore.Contains(period))
            {
                return true;
            }

            return false;
        }

        private void AddRequiredPayment(RequiredPaymentEntity requiredPayment, decimal amount)
        {
            var payment = new RequiredPaymentEntity(requiredPayment);
            payment.AmountDue = amount;
            RequiredPayments.Add(payment);
        }

        private int PeriodFromDeliveryMonth(int deliveryMonth)
        {
            if (deliveryMonth < 8)
            {
                return deliveryMonth + 5;
            }

            return deliveryMonth - 7;
        }
    }
}