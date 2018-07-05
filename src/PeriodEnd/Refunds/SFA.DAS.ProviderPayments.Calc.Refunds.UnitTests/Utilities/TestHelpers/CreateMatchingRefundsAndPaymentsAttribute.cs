using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.TestHelpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CreateMatchingRefundsAndPaymentsAttribute : Attribute, ITestBuilder
    {
        private readonly int _academicYear;
        private readonly bool _hasMatchingPastPayments;
        private readonly bool _monthsHaveHigherPaymentsThanRefunds;
        private readonly bool _hasNegativeFundingSources;

        private static readonly List<FundingSource> FundingSources = new List<FundingSource>
        {
            FundingSource.CoInvestedEmployer,
            FundingSource.CoInvestedSfa,
            FundingSource.FullyFundedSfa,
            FundingSource.Levy
        };

        public CreateMatchingRefundsAndPaymentsAttribute(
            bool hasNegativeFundingSources = false, 
            bool hasMatchingPastPayments = true, 
            bool monthsHaveHigherPaymentsThanRefunds = true,
            string academicYear = "1718")
        {
            _hasNegativeFundingSources = hasNegativeFundingSources;
            _hasMatchingPastPayments = hasMatchingPastPayments;
            _monthsHaveHigherPaymentsThanRefunds = monthsHaveHigherPaymentsThanRefunds;
            _academicYear = int.Parse(academicYear.Substring(0, 2)) + 2000;
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var results = new List<TestMethod>();
            var fixture = new Fixture();

            var refunds = fixture.Build<RequiredPaymentEntity>()
                .CreateMany()
                .ToList();

            var random = new Random();

            foreach (var refund in refunds)
            {
                var period = random.Next(11) + 1; // 0 based
                refund.DeliveryMonth = DeliveryMonthFromPeriod(period);
                refund.DeliveryYear = DeliveryYearFromPeriod(period);
            }

            var pastPayments = new List<HistoricalPaymentEntity>();

            if (_hasMatchingPastPayments)
            {
                foreach (var refund in refunds)
                {
                    var pastPaymentsForRefund = fixture.Build<HistoricalPaymentEntity>()
                        .With(x => x.AccountId, refund.AccountId)
                        .With(x => x.ApprenticeshipContractType, refund.ApprenticeshipContractType)
                        .With(x => x.TransactionType, refund.TransactionType)
                        .With(x => x.DeliveryMonth, refund.DeliveryMonth)
                        .With(x => x.DeliveryYear, refund.DeliveryYear)
                        .Without(x => x.FundingSource)
                        .CreateMany()
                        .ToList();

                    foreach (var historicalPaymentEntity in pastPaymentsForRefund)
                    {
                        do
                        {
                            historicalPaymentEntity.FundingSource = fixture.Create<FundingSource>();
                        } while (historicalPaymentEntity.FundingSource == FundingSource.Transfer);
                    }

                    pastPayments.AddRange(pastPaymentsForRefund);

                    if (_monthsHaveHigherPaymentsThanRefunds)
                    {
                        var amount = pastPaymentsForRefund.Sum(x => x.Amount);
                        refund.AmountDue = -1 * amount;
                    }

                    while (_hasNegativeFundingSources && !pastPaymentsForRefund.Any(x => x.Amount < 0))
                    {
                        foreach (var fundingSource in FundingSources)
                        {
                            if (random.Next(2) == 2)
                            {
                                pastPaymentsForRefund.Where(x => x.FundingSource == fundingSource).ToList()
                                    .ForEach(MakePaymentNegative);
                            }
                        }
                    }
                }
            }

            var methodParameters = method.GetParameters();
            if (methodParameters.Length != 3 ||
                methodParameters[0].ParameterType != typeof(List<RequiredPaymentEntity>) ||
                methodParameters[1].ParameterType != typeof(List<HistoricalPaymentEntity>) ||
                methodParameters[2].ParameterType != typeof(LearnerRefundProcessor)
            )
            {
                throw new ApplicationException("Please ensure that you have 2 parameters, List<RequiredPaymentEntity> and List<HistoricPaymentEntity>");
            }

            var parameters = new TestCaseParameters(new object[] { refunds, pastPayments, new LearnerRefundProcessor() });

            results.Add(new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters));
            return results;
        }

        private int DeliveryMonthFromPeriod(int period)
        {
            if (period < 6)
            {
                return period + 7;
            }

            return period - 5;
        }

        private int DeliveryYearFromPeriod(int period)
        {
            if (period < 6)
            {
                return _academicYear;
            }

            return _academicYear + 1;
        }

        private void MakePaymentNegative(HistoricalPaymentEntity payment)
        {
            payment.Amount = -payment.Amount;
        }
    }
}
