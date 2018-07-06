using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
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
        private readonly string _academicYear;
        private readonly bool _hasMatchingPastPayments;
        private readonly bool _monthsHaveHigherPaymentsThanRefunds;
        private readonly bool _hasNegativeFundingSources;
        private readonly decimal _refundAmount;
        private readonly decimal _paymentAmount;
        private readonly int _numberOfRefunds;

        private static readonly List<FundingSource> FundingSources = new List<FundingSource>
        {
            FundingSource.CoInvestedEmployer,
            FundingSource.CoInvestedSfa,
            FundingSource.FullyFundedSfa,
            FundingSource.Levy
        };

        public CreateMatchingRefundsAndPaymentsAttribute(
            int refundAmount = -900, 
            int paymentAmount = 300, 
            bool hasNegativeFundingSources = false, 
            bool hasMatchingPastPayments = true, 
            bool monthsHaveHigherPaymentsThanRefunds = true,
            string academicYear = "1718",
            int numberOfRefunds = 3)
        {
            _refundAmount = refundAmount;
            _paymentAmount = paymentAmount;
            _hasNegativeFundingSources = hasNegativeFundingSources;
            _hasMatchingPastPayments = hasMatchingPastPayments;
            _monthsHaveHigherPaymentsThanRefunds = monthsHaveHigherPaymentsThanRefunds;
            _academicYear = academicYear;
            _numberOfRefunds = numberOfRefunds;
        }

        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var results = new List<TestMethod>();
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var random = new Random();
            var refunds = new List<RequiredPaymentEntity>();
            var pastPayments = new List<HistoricalPaymentEntity>();

            for (var i = 0; i < _numberOfRefunds; i++)
            {
                var generatedRefund = RefundGenerator.Generate(
                    period: random.Next(11) + 1, 
                    refundAmount: _refundAmount,
                    paymentAmount: _paymentAmount, 
                    academicYear: _academicYear);

                refunds.Add(generatedRefund.Refunds.First());
                if (_hasMatchingPastPayments)
                {
                    var pastPaymentsForRefund = generatedRefund.AssociatedPayments;
                    
                    if (_monthsHaveHigherPaymentsThanRefunds)
                    {
                        var amount = pastPaymentsForRefund.Sum(x => x.Amount);
                        generatedRefund.Refunds.First().AmountDue = -1 * amount;
                    }

                    while (_hasNegativeFundingSources && !pastPaymentsForRefund.Any(x => x.Amount < 0))
                    {
                        foreach (var fundingSource in FundingSources)
                        {
                            if (random.Next(2) == 1)
                            {
                                pastPaymentsForRefund.Where(x => x.FundingSource == fundingSource).ToList()
                                    .ForEach(MakePaymentNegative);
                            }
                        }
                    }

                    pastPayments.AddRange(pastPaymentsForRefund);
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

            var learnerRefundProcessor = fixture.Build<LearnerRefundProcessor>().Create();
            var parameters = new TestCaseParameters(new object[] { refunds, pastPayments, learnerRefundProcessor });

            results.Add(new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters));
            return results;
        }

        private void MakePaymentNegative(HistoricalPaymentEntity payment)
        {
            payment.Amount = -payment.Amount;
        }
    }
}
