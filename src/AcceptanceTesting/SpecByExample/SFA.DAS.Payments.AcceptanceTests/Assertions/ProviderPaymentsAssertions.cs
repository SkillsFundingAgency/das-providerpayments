using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions
{
    static class ProviderPaymentsAssertions
    {
        public static void AssertEasPayments(ProviderAdjustmentsContext context, List<GenericPeriodBasedRow> paymentListForPeriods)
        {
            foreach (var paymentListForPeriod in paymentListForPeriods)
            {
                var paymentPeriod = new PeriodDefinition(paymentListForPeriod.Period);
                var earningsPeriod = paymentPeriod.TransformPaymentPeriodToEarningsPeriod();

                if (earningsPeriod == null)
                {
                    AssertThatThereAreNoPaymentsForPeriod(paymentListForPeriod);
                    continue;
                }

                AssertThatPaymentsAreCorrectForPeriod(context, earningsPeriod, paymentListForPeriod);
            }
        }

        static void AssertThatThereAreNoPaymentsForPeriod(GenericPeriodBasedRow period)
        {
            foreach (var row in period.Rows)
            {
                if (row.Amount != 0)
                {
                    throw new ApplicationException(
                        $"The payment {row.Name} for period {period.Period} is made before a payment is possible for this year");
                }
            }
        }

        static void AssertThatPaymentsAreCorrectForPeriod(ProviderAdjustmentsContext context, 
            PeriodDefinition earningsPeriod, GenericPeriodBasedRow period)
        {
            var payments = context.PaymentsFor(earningsPeriod)
                .Select(x => new
                {
                    x.PaymentType,
                    x.PaymentTypeName,
                    x.Amount,
                }).ToList();

            foreach (var row in period.Rows)
            {
                var payment = payments.Where(x => x.PaymentTypeName == row.Name).Sum(x => x.Amount);
                if (row.Amount != payment)
                {
                    throw new ApplicationException(
                        $"expected: {row.Amount} found: {payment} for {row.Name} for period {period.Period}");
                }
            }
        }
    }
}
