using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class PaymentsDueCalculationService : ICalculatePaymentsDue
    {
        
        // Input
        private IReadOnlyList<int> PeriodsToIgnore { get; set; }
        
        // Output
        public List<RequiredPaymentEntity> RequiredPayments { get; private set; }

        public List<RequiredPaymentEntity> Calculate(List<FundingDue> earnings,
            List<int> periodsToIgnore,
            List<RequiredPaymentEntity> pastPayments)
        {
            RequiredPayments = new List<RequiredPaymentEntity>();
            PeriodsToIgnore = periodsToIgnore;

            var processedGroups = new HashSet<PaymentGroup>();

            var groupedEarnings = earnings
                .GroupBy(x => new PaymentGroup(x))
                .ToDictionary(x => x.Key, x => x.ToList());

            var groupedPastPayments = pastPayments
                .GroupBy(x => new PaymentGroup(x))
                .ToDictionary(x => x.Key, x => x.ToList());

            // Payments for earnings
            foreach (var key in groupedEarnings.Keys)
            {
                processedGroups.Add(key);
                if (ShouldIgnoreEarnings(key))
                {
                    continue;
                }

                var earningsForGroup = groupedEarnings[key];
                var pastPaymentsForGroup = new List<RequiredPaymentEntity>();

                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPaymentsForGroup = groupedPastPayments[key];
                }

                var payment = earningsForGroup.Sum(x => x.AmountDue) -
                              pastPaymentsForGroup.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(earningsForGroup.First(), payment);
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
                
                var pastPaymentsForGroup = groupedPastPayments[key];

                var payment = -pastPaymentsForGroup.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    AddRequiredPayment(pastPaymentsForGroup.First(), payment);
                }
            }

            return new List<RequiredPaymentEntity>(RequiredPayments);
        }

        private bool ShouldIgnoreEarnings(PaymentGroup earningInformation)
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