using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependiencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class LearnerRefundProcessor : IProcessLearnerRefunds
    {
        public List<PaymentEntity> ProcessRefundsForLearner(
            List<RequiredPaymentEntity> refunds,
            List<HistoricalPaymentEntity> previousPayments)
        {
            var refundGroups = refunds.ToLookup(x => new RefundGroup(x));
            var volatilePreviousPayments = new List<HistoricalPaymentEntity>(previousPayments);

            var refundPayments = new List<PaymentEntity>();
            
            foreach (var refundGroup in refundGroups)
            {
                foreach (var refund in refundGroup)
                {
                    var previousPaymentGroup = volatilePreviousPayments.ToLookup(x => new RefundGroup(x));
                    var previousPaymentsForRefundGroup = new List<HistoricalPaymentEntity>();
                    if (previousPaymentGroup.Contains(refundGroup.Key))
                    {
                        previousPaymentsForRefundGroup = previousPaymentGroup[refundGroup.Key].ToList();
                    }

                    var newRefunds = ProcessRefund(refund, previousPaymentsForRefundGroup);
                    refundPayments.AddRange(newRefunds);
                    volatilePreviousPayments.AddRange(newRefunds.Select(x => new HistoricalPaymentEntity(x, refund)));
                }
            }

            return refundPayments;
        }

        private List<PaymentEntity> ProcessRefund(
            RequiredPaymentEntity refund, 
            List<HistoricalPaymentEntity> previousPayments)
        {
            var refundPayments = new List<PaymentEntity>();

            var amountToRefund = refund.AmountDue;
            var amountRefunded = 0m;
            var month = refund.DeliveryMonth;
            var year = refund.DeliveryYear;

            while (amountToRefund <= amountRefunded)
            {
                var stillToRefund = amountToRefund - amountRefunded;
                var paymentsForPeriod = previousPayments
                    .Where(x => x.DeliveryMonth == month && x.DeliveryYear == year)
                    .ToList();
                var newRefunds = ProcessRefundForPeriod(stillToRefund, year, month, paymentsForPeriod, refund);
                amountRefunded += newRefunds.Sum(x => x.Amount);
                refundPayments.AddRange(newRefunds);
            
                if (month == 8)
                {
                    break;
                }

                if (month == 1)
                {
                    month = 12;
                    year -= 1;
                }
                else
                {
                    month--;
                }
            }

            return refundPayments;
        }

        private List<PaymentEntity> ProcessRefundForPeriod(
            decimal amount,
            int deliveryYear,
            int deliveryMonth,
            List<HistoricalPaymentEntity> previoudPaymentsForPeriod,
            RequiredPaymentEntity refund)
        {
            var fundingSourcesToRefund = new List<FundingSource>
            {
                FundingSource.Levy,
                FundingSource.CoInvestedEmployer,
                FundingSource.CoInvestedSfa,
                FundingSource.FullyFundedSfa
            };

            var refundPayments = new List<PaymentEntity>();

            var total = previoudPaymentsForPeriod.Sum(x => x.Amount);
            if (total == 0)
            {
                return refundPayments;
            }

            var amountToRefund = Math.Min(total, amount);

            foreach (var fundingSource in fundingSourcesToRefund)
            {
                var totalPaidForFundingSource = TotalForFundingSource(previoudPaymentsForPeriod, fundingSource);
                if (totalPaidForFundingSource > 0)
                {
                    var refundAmountForFundingSource = amountToRefund * (totalPaidForFundingSource / total);
                    var payment = CreatePayment(refund, refundAmountForFundingSource, deliveryYear, deliveryMonth, fundingSource);
                    refundPayments.Add(payment);
                }
            }

            return refundPayments;
        }

        private decimal TotalForFundingSource(List<HistoricalPaymentEntity> payments, FundingSource fundingSource)
        {
            var total = payments.Where(x => x.FundingSource == fundingSource).Sum(x => x.Amount);
            if (total < 0)
            {
                throw new ApplicationException("Funding source total is negative");
            }

            return total;
        }

        private PaymentEntity CreatePayment(
            RequiredPaymentEntity refund, 
            decimal amount, 
            int deliveryYear,
            int deliveryMonth, 
            FundingSource fundingSource)
        {
            var payment = new PaymentEntity
            {
                DeliveryYear = deliveryYear,
                DeliveryMonth = deliveryMonth,
                Amount = amount,
                RequiredPaymentId = refund.Id,
                CollectionPeriodName = refund.CollectionPeriodName,
                CollectionPeriodMonth = refund.CollectionPeriodMonth,
                CollectionPeriodYear = refund.CollectionPeriodYear,
                FundingSource = fundingSource,
                TransactionType = refund.TransactionType,
            };

            return payment;
        }
    }
}
