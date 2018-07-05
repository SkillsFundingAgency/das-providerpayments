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
                    try
                    {
                        if (previousPaymentGroup.Contains(refundGroup.Key))
                        {
                            previousPaymentsForRefundGroup = previousPaymentGroup[refundGroup.Key].ToList();
                        }

                        var newRefunds = ProcessRefund(refund, previousPaymentsForRefundGroup);
                        refundPayments.AddRange(newRefunds);
                        volatilePreviousPayments.AddRange(newRefunds.Select(x => new HistoricalPaymentEntity(x, refund)));
                    }
                    catch(ApplicationException)
                    { 
                        // A funding source is negative, so ignoring this refund
                    }
                }
            }

            return refundPayments;
        }

        private List<PaymentEntity> ProcessRefund(
            RequiredPaymentEntity refund, 
            List<HistoricalPaymentEntity> previousPayments)
        {
            var refundPayments = new List<PaymentEntity>();

            var amountToRefund = Math.Round(refund.AmountDue, 5);
            var amountRefunded = 0m;
            var month = refund.DeliveryMonth;
            var year = refund.DeliveryYear;

            while (amountToRefund < amountRefunded)
            {
                var stillToRefund = amountToRefund - amountRefunded;
                var paymentsForPeriod = previousPayments
                    .Where(x => x.DeliveryMonth == month && x.DeliveryYear == year)
                    .ToList();
                var newRefunds = ProcessRefundForPeriod(stillToRefund, year, month, paymentsForPeriod, refund);
                amountRefunded +=  Math.Round(newRefunds.Sum(x => x.Amount), 5);
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

        private static readonly List<FundingSource> FundingSourcesToRefund = new List<FundingSource>
                                        {
                                            FundingSource.Levy,
                                            FundingSource.CoInvestedEmployer,
                                            FundingSource.CoInvestedSfa,
                                            FundingSource.FullyFundedSfa
                                        };

        private List<PaymentEntity> ProcessRefundForPeriod(
            decimal amount,
            int deliveryYear,
            int deliveryMonth,
            List<HistoricalPaymentEntity> previoudPaymentsForPeriod,
            RequiredPaymentEntity refund)
        {
            var refundPayments = new List<PaymentEntity>();

            var total = Math.Round(previoudPaymentsForPeriod.Sum(x => x.Amount), 5);
            if (total == 0)
            {
                return refundPayments;
            }

            var amountToRefund = Math.Round(Math.Max(total * -1, amount), 5);
            
            foreach (var fundingSource in FundingSourcesToRefund)
            {
                var totalPaidForFundingSource = TotalForFundingSource(previoudPaymentsForPeriod, fundingSource);
                if (totalPaidForFundingSource > 0)
                {
                    var refundAmountForFundingSource = Math.Round(amountToRefund * totalPaidForFundingSource / total, 5);
                    var payment = CreatePayment(refund, refundAmountForFundingSource, deliveryYear, deliveryMonth, fundingSource);
                    refundPayments.Add(payment);
                }
            }

            return refundPayments;
        }

        private decimal TotalForFundingSource(List<HistoricalPaymentEntity> payments, FundingSource fundingSource)
        {
            var total = Math.Round(payments.Where(x => x.FundingSource == fundingSource).Sum(x => x.Amount), 5);
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
