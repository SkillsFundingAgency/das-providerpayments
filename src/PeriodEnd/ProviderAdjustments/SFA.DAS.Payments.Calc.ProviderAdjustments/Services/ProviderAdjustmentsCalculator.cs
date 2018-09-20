using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Domain;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Services
{
    public interface ICalculateProviderAdjustments
    {
        IEnumerable<PaymentEntity> CalculatePaymentsAndRefunds(List<AdjustmentEntity> previousPayments,
            List<AdjustmentEntity> earnings);
    }

    public class ProviderAdjustmentsCalculator : ICalculateProviderAdjustments
    {
        public IEnumerable<PaymentEntity> CalculatePaymentsAndRefunds(
            List<AdjustmentEntity> previousPayments, 
            List<AdjustmentEntity> earnings)
        {
            var processedSubmissions = new HashSet<Guid>(
                previousPayments.Select(x => x.SubmissionId)
                    .Intersect(earnings.Select(y => y.SubmissionId)));

            var groupedPreviousPayments = previousPayments.ToLookup(x => new ProviderPaymentsGroup(x));

            var groupedEarnings = EarningsGroupsThatHaveNotBeenProcessed(earnings, processedSubmissions);
            
            var payments = CalculatePaymentsAndRefunds(groupedEarnings, groupedPreviousPayments);
            var processedEarningsGroups = new HashSet<ProviderPaymentsGroup>(groupedEarnings.Select(x => x.Key));

            var refunds = CalculateRefunds(groupedPreviousPayments, processedEarningsGroups, processedSubmissions);

            return payments.Union(refunds);
        }

        private static IEnumerable<PaymentEntity> CalculatePaymentsAndRefunds(
            ILookup<ProviderPaymentsGroup, AdjustmentEntity> groupedEarnings,
            ILookup<ProviderPaymentsGroup, AdjustmentEntity> groupedPreviousPayments)
        {
            foreach (var earningGroup in groupedEarnings)
            {
                var paymentAmount = earningGroup.Sum(x => x.Amount) -
                                    groupedPreviousPayments[earningGroup.Key].Sum(x => x.Amount);

                if (paymentAmount != 0)
                {
                    yield return CreatePayment(earningGroup.Key, paymentAmount);
                }
            }
        }

        private static IEnumerable<PaymentEntity> CalculateRefunds(
            ILookup<ProviderPaymentsGroup, AdjustmentEntity> groupedPreviousPayments, 
            HashSet<ProviderPaymentsGroup> alreadyProcessedGroups,
            HashSet<Guid> processedSubmissions)
        {
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
                    yield return CreatePayment(previousPaymentGroup.Key, paymentAmount);
                }
            }
        }

        private static PaymentEntity CreatePayment(ProviderPaymentsGroup source, decimal amount)
        {
            var payment = new PaymentEntity
            {
                Amount = amount,
                PaymentType = source.PaymentType,
                Ukprn = source.Ukprn,
                PaymentTypeName = source.PaymentTypeName,
                SubmissionCollectionPeriod = source.Period,
                SubmissionId = source.SubmissionId,
            };
            return payment;
        }

        private static ILookup<ProviderPaymentsGroup, AdjustmentEntity> EarningsGroupsThatHaveNotBeenProcessed(List<AdjustmentEntity> earnings, HashSet<Guid> processedSubmissions)
        {
            return earnings
                .Where(x => !processedSubmissions.Contains(x.SubmissionId))
                .ToLookup(x => new ProviderPaymentsGroup(x));
        }
    }
}
