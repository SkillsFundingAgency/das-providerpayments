using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Domain;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Domain.Extensions;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Services
{
    public interface ICalculateProviderPayments
    {
        List<PaymentEntity> CalculatePayments(IEnumerable<AdjustmentEntity> previousPayments,
            IEnumerable<AdjustmentEntity> earnings);
    }

    class ProviderPaymentsCalculator : ICalculateProviderPayments
    {
        public List<PaymentEntity> CalculatePayments(
            IEnumerable<AdjustmentEntity> previousPayments, 
            IEnumerable<AdjustmentEntity> earnings)
        {
            var previousPaymentsAsAList = previousPayments.ToList();
            var alreadyProcessedGroups = new HashSet<ProviderPaymentsGroup>();
            var processedSubmissions = new HashSet<Guid>(previousPaymentsAsAList.Select(x => x.SubmissionId));

            var payments = new List<PaymentEntity>();

            var groupedPreviousPayments = previousPaymentsAsAList.ToLookup(x => new ProviderPaymentsGroup(x));
            var groupedEarnings = earnings
                .Where(x => !processedSubmissions.Contains(x.SubmissionId))
                .ToLookup(x => new ProviderPaymentsGroup(x));

            foreach (var earningGroup in groupedEarnings)
            {
                alreadyProcessedGroups.Add(earningGroup.Key);
                var paymentAmount = earningGroup.Sum(x => x.Amount) -
                                    groupedPreviousPayments[earningGroup.Key].Sum(x => x.Amount);

                if (paymentAmount != 0)
                {
                    payments.Add(earningGroup.Key.MakePayment(paymentAmount));
                }
            }

            foreach (var previousPaymentGroup in groupedPreviousPayments)
            {
                if (alreadyProcessedGroups.Contains(previousPaymentGroup.Key))
                {
                    continue;
                }

                var paymentAmount = -1 * previousPaymentGroup.Sum(x => x.Amount);
                if (paymentAmount != 0)
                {
                    payments.Add(previousPaymentGroup.Key.MakePayment(paymentAmount));
                }
            }

            return payments;
        }
    }
}
