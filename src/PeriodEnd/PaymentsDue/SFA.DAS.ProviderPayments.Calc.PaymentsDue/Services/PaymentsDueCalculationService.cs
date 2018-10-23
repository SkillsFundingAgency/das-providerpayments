using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class PaymentsDueCalculationService : ICalculatePaymentsDue
    {
        public List<RequiredPayment> Calculate(
            List<FundingDue> earnings,
            HashSet<int> periodsToIgnore,
            List<RequiredPayment> pastPayments)
        {
            var requiredPayments = new List<RequiredPayment>();
            
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
                if (ShouldIgnoreEarnings(key, periodsToIgnore))
                {
                    continue;
                }

                var earningsForGroup = groupedEarnings[key];
                var pastPaymentsForGroup = new List<RequiredPayment>();

                if (groupedPastPayments.ContainsKey(key))
                {
                    pastPaymentsForGroup = groupedPastPayments[key];
                }

                var payment = earningsForGroup.Sum(x => x.AmountDue) -
                              pastPaymentsForGroup.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    requiredPayments.Add(CreateRequiredPayment(earningsForGroup.First(), payment));
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

                if (ShouldIgnoreEarnings(key, periodsToIgnore))
                {
                    continue;
                }
                
                var pastPaymentsForGroup = groupedPastPayments[key];

                var payment = -pastPaymentsForGroup.Sum(x => x.AmountDue);

                if (payment != 0)
                {
                    requiredPayments.Add(CreateRequiredPayment(pastPaymentsForGroup.First(), payment));
                }
            }

            return requiredPayments;
        }

        private bool ShouldIgnoreEarnings(PaymentGroup earningInformation, HashSet<int> periodsToIgnore)
        {
            var period = PeriodFromDeliveryMonth(earningInformation.DeliveryMonth);
            if (periodsToIgnore.Contains(period))
            {
                return true;
            }

            return false;
        }

        private RequiredPayment CreateRequiredPayment(RequiredPayment requiredPayment, decimal amount)
        {
            var payment = new RequiredPayment(requiredPayment);
            payment.AmountDue = amount;
            return payment;
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