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
            public RequiredPaymentEntity Refund { get; set; }
            public List<HistoricalPaymentEntity> AssociatedPayments { get; set; }
        }

        public static RefundGeneratorResult Generate(
            int period = 1, 
            decimal amount = -1000,
            decimal paymentAmount = 100,
            int numberOfPayments = 3,
            string academicYear = "1819")
        {
            var initialYearForAcademicYear = int.Parse(academicYear.Substring(0, 2)) + 2000;
            var fixture = new Fixture();

            var refund = fixture.Build<RequiredPaymentEntity>()
                .With(x => x.AmountDue, amount)
                .Create();

            var random = new Random();

            refund.DeliveryMonth = DeliveryMonthFromPeriod(period);
            refund.DeliveryYear = DeliveryYearFromPeriod(period, initialYearForAcademicYear);

            var pastPayments = new List<HistoricalPaymentEntity>();
            var pastPaymentsForRefund = fixture.Build<HistoricalPaymentEntity>()
                .With(x => x.AccountId, refund.AccountId)
                .With(x => x.ApprenticeshipContractType, refund.ApprenticeshipContractType)
                .With(x => x.TransactionType, refund.TransactionType)
                .With(x => x.DeliveryMonth, refund.DeliveryMonth)
                .With(x => x.DeliveryYear, refund.DeliveryYear)
                .Without(x => x.FundingSource)
                .With(x => x.Amount, paymentAmount)
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

            return new RefundGeneratorResult
            {
                Refund = refund,
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
