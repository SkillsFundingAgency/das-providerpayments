using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Exceptions;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services
{
    public class LearnerRefundProcessor : IProcessLearnerRefunds
    {
        private readonly ILogger _logger;

        private static readonly List<FundingSource> FundingSourcesToRefund = new List<FundingSource>
        {
            FundingSource.Levy,
            FundingSource.CoInvestedEmployer,
            FundingSource.CoInvestedSfa,
            FundingSource.FullyFundedSfa
        };

        public LearnerRefundProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public List<Refund> ProcessRefundsForLearner(
            List<RequiredPaymentEntity> refunds,
            List<HistoricalPayment> previousPayments)
        {
            var refundGroups = refunds.ToLookup(x => new RefundGroupIdentifier(x));
            var volatilePreviousPayments = new List<HistoricalPayment>(previousPayments);

            var refundPayments = new List<Refund>();
            
            foreach (var refundGroup in refundGroups)
            {
                foreach (var refund in refundGroup)
                {
                    var previousPaymentGroup = volatilePreviousPayments.ToLookup(x => new RefundGroupIdentifier(x));
                    var previousPaymentsForRefundGroup = new List<HistoricalPayment>();
                    try
                    {
                        if (previousPaymentGroup.Contains(refundGroup.Key))
                        {
                            previousPaymentsForRefundGroup = previousPaymentGroup[refundGroup.Key].ToList();
                        }

                        var newRefunds = ProcessRefund(refund, previousPaymentsForRefundGroup);
                        refundPayments.AddRange(newRefunds);
                        volatilePreviousPayments.AddRange(newRefunds.Select(x => new HistoricalPayment(x, refund)));

                        if (NewRefundsAreLessThanRequested(newRefunds, refund))
                        {
                            _logger.Warn($"Refund not fully made. " +
                                         $"UKPRN: {refund.Ukprn} " +
                                         $"LearnRefNumber: {refund.LearnRefNumber} " +
                                         $"Refund Amount: {refund.AmountDue} " +
                                         $"Actual Amount: {newRefunds.Sum(x => x.Amount)} " +
                                         $"Delivery Month: {refund.DeliveryMonth}");
                        }
                    }
                    catch(NetNegativePaymentsException)
                    { 
                        // A funding source is negative, so ignoring this refund
                        _logger.Error($"Refund for UKPRN: {refund.Ukprn} " +
                                      $"LearnRefNumber: {refund.LearnRefNumber} " +
                                      $"Amount: {refund.AmountDue} " +
                                      $"DeliveryMonth: {refund.DeliveryMonth} " +
                                      $"not made due to a net negative funding source");
                    }
                }
            }

            return refundPayments;
        }

        private static bool NewRefundsAreLessThanRequested(IEnumerable<Refund> newRefunds, RequiredPaymentEntity refund)
        {
            return newRefunds.Sum(x => x.Amount) < refund.AmountDue;
        }

        private List<Refund> ProcessRefund(
            RequiredPaymentEntity refund, 
            List<HistoricalPayment> previousPayments)
        {
            var refundPayments = new List<Refund>();

            var amountToRefund = refund.AmountDue;
            var amountRefunded = 0m;
            var month = refund.DeliveryMonth;
            var year = refund.DeliveryYear;

            while (RefundIsStillRequired(amountToRefund, amountRefunded))
            {
                var stillToRefund = amountToRefund - amountRefunded;
                
                var newRefunds = ProcessRefundForPeriod(stillToRefund, year, month, previousPayments, refund);
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

        private static bool RefundIsStillRequired(decimal amountToRefund, decimal amountRefunded)
        {
            return amountToRefund < amountRefunded;
        }

        private List<Refund> ProcessRefundForPeriod(
            decimal requestedRefundAmountForPeriod,
            int deliveryYear,
            int deliveryMonth,
            List<HistoricalPayment> previousPayments,
            RequiredPaymentEntity refund)
        {
            var refundPayments = new List<Refund>();

            var previousPaymentsForPeriod = previousPayments
                .Where(x => x.DeliveryMonth == deliveryMonth && x.DeliveryYear == deliveryYear)
                .ToList();

            var total = previousPaymentsForPeriod.Sum(x => x.Amount);
            if (total == 0)
            {
                return refundPayments;
            }

            var amountToRefund = Math.Max(total * -1, requestedRefundAmountForPeriod);
            
            foreach (var fundingSource in FundingSourcesToRefund)
            {
                var totalPaidForFundingSource = TotalForFundingSource(previousPaymentsForPeriod, fundingSource);
                if (totalPaidForFundingSource > 0)
                {
                    var refundAmountForFundingSource = amountToRefund * totalPaidForFundingSource / total;
                    var payment = CreatePayment(refund, refundAmountForFundingSource, deliveryYear, deliveryMonth, fundingSource);
                    refundPayments.Add(payment);
                }
            }

            return refundPayments;
        }

        private decimal TotalForFundingSource(List<HistoricalPayment> payments, FundingSource fundingSource)
        {
            var total = payments.Where(x => x.FundingSource == fundingSource).Sum(x => x.Amount);
            if (total < 0)
            {
                throw new NetNegativePaymentsException("Funding source total is negative");
            }

            return total;
        }

        private Refund CreatePayment(
            RequiredPaymentEntity refund, 
            decimal amount, 
            int deliveryYear,
            int deliveryMonth, 
            FundingSource fundingSource)
        {
            var payment = new Refund
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
                AccountId = refund.AccountId,
            };

            return payment;
        }
    }
}
