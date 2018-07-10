using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.TestHelpers
{
    class RefundGenerator
    {
        public class RefundGeneratorResult
        {
            public List<RequiredPaymentEntity> Refunds { get; set; }
            public List<HistoricalPaymentEntity> AssociatedPayments { get; set; }
        }

        public static RefundGeneratorResult Generate(
            int period = 1, 
            decimal refundAmount = -1000,
            decimal paymentAmount = 100,
            int numberOfPayments = 3,
            string academicYear = "1819",
            int numberOfRefunds = 1,
            string fundingLineType = "flt1")
        {
            var initialYearForAcademicYear = int.Parse(academicYear.Substring(0, 2)) + 2000;
            var fixture = new Fixture();

            List<RequiredPaymentEntity> refunds;
            var random = new Random();

            if (numberOfRefunds > 1)
            {
                var transactionType = fixture.Create<TransactionType>();
                var act = random.Next(2) + 1;

                refunds = fixture.Build<RequiredPaymentEntity>()
                    .With(x => x.AmountDue, refundAmount)
                    .With(x => x.AccountId, fixture.Create<Generator<long>>().First())
                    .With(x => x.ApprenticeshipContractType, act)
                    .With(x => x.TransactionType, transactionType)
                    .With(x => x.FundingLineType, fundingLineType)
                    .CreateMany(numberOfRefunds)
                    .ToList();
            }
            else
            {
                refunds = refunds = fixture.Build<RequiredPaymentEntity>()
                    .With(x => x.AmountDue, refundAmount)
                    .With(x => x.FundingLineType, fundingLineType)
                    .CreateMany(numberOfRefunds)
                    .ToList();
            }

            var pastPayments = new List<HistoricalPaymentEntity>();

            foreach (var refund in refunds)
            {
                refund.DeliveryMonth = DeliveryMonthFromPeriod(period);
                refund.DeliveryYear = DeliveryYearFromPeriod(period, initialYearForAcademicYear);

                var pastPaymentsForRefund = fixture.Build<HistoricalPaymentEntity>()
                    .With(x => x.AccountId, refund.AccountId)
                    .With(x => x.ApprenticeshipContractType, refund.ApprenticeshipContractType)
                    .With(x => x.TransactionType, refund.TransactionType)
                    .With(x => x.DeliveryMonth, refund.DeliveryMonth)
                    .With(x => x.DeliveryYear, refund.DeliveryYear)
                    .Without(x => x.FundingSource)
                    .With(x => x.Amount, paymentAmount)
                    .With(x => x.FundingLineType, fundingLineType)
                    .CreateMany(numberOfPayments)
                    .ToList();

                foreach (var historicalPaymentEntity in pastPaymentsForRefund)
                {
                    do
                    {
                        historicalPaymentEntity.FundingSource = fixture.Create<FundingSource>();
                    } while (historicalPaymentEntity.FundingSource == FundingSource.Transfer);
                }

                pastPayments.AddRange(pastPaymentsForRefund);
            }

            return new RefundGeneratorResult
            {
                Refunds = refunds,
                AssociatedPayments = pastPayments,
            };
        }

        private static int DeliveryMonthFromPeriod(int period)
        {
            if (period < 6)
            {
                return period + 7;
            }

            return period - 5;
        }

        private static int DeliveryYearFromPeriod(int period, int academicYear)
        {
            if (period < 6)
            {
                return academicYear;
            }

            return academicYear + 1;
        }
    }
}
