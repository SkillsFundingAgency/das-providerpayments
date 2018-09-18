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
        List<PaymentEntity> CalculatePayments(List<AdjustmentEntity> previousPayments,
            List<AdjustmentEntity> earnings);
    }

    public class ProviderPaymentsCalculator : ICalculateProviderPayments
    {
        public List<PaymentEntity> CalculatePayments(
            List<AdjustmentEntity> previousPayments, 
            List<AdjustmentEntity> earnings)
        {
            var alreadyProcessedGroups = new HashSet<ProviderPaymentsGroup>();
            var processedSubmissions = new HashSet<Guid>(
                previousPayments.Select(x => x.SubmissionId)
                    .Intersect(earnings.Select(y => y.SubmissionId)));

            var payments = new List<PaymentEntity>();

            var groupedPreviousPayments = previousPayments.ToLookup(x => new ProviderPaymentsGroup(x));
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
                if (alreadyProcessedGroups.Contains(previousPaymentGroup.Key) ||
                    processedSubmissions.Contains(previousPaymentGroup.Key.SubmissionId))
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
