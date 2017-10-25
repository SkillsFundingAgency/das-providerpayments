using NUnit.Framework;
using SFA.DAS.payments.Automation.Assertions;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Assertions.UnitTests
{

    public class WhenValidatingAssertions
    {

        [SetUp]
        public void Arrange()
        {


        }

        [Test]
        public void AssertLevyPaymentResults_NoErrors()
        {

            var expectations = new List<ProviderEarningsAndPayments>();
            var actuals = new List<PaymentResult>();

            var periodearningAndPayments = new List<PeriodEarningAndPayments>();
            periodearningAndPayments.Add(new PeriodEarningAndPayments {
                Period="09/17",
                LevyAccountDebited=300,
                ProviderPaidBySfa=800,
            });


            expectations.Add(
                new ProviderEarningsAndPayments
                {
                    EarningAndPaymentsByPeriod = periodearningAndPayments,
                    ProviderKey="test"
                }
                );

            actuals.Add(new PaymentResult
            {
                Amount = 300,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithEmployer,
                CalculationPeriod = "08/17",
                DeliveryPeriod = "08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.Levy,
            });
            actuals.Add(new PaymentResult
            {
                Amount = 500,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithEmployer,
                CalculationPeriod = "08/17",
                DeliveryPeriod="08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.CoInvestedSfa,
            });

            var results = PaymentAssertions.AssertPayments(expectations,actuals,"Test scenario");

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
                

        }


        [Test]
        public void AssertCofundingPaymentResults_NoErrors()
        {

            var expectations = new List<ProviderEarningsAndPayments>();
            var actuals = new List<PaymentResult>();

            var periodearningAndPayments = new List<PeriodEarningAndPayments>();
            periodearningAndPayments.Add(new PeriodEarningAndPayments
            {
                Period = "09/17",
                ProviderPaidBySfa = 800,
            });


            expectations.Add(
                new ProviderEarningsAndPayments
                {
                    EarningAndPaymentsByPeriod = periodearningAndPayments,
                    ProviderKey = "test"
                }
                );

            actuals.Add(new PaymentResult
            {
                Amount = 800,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithSfa,
                CalculationPeriod = "08/17",
                DeliveryPeriod = "08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.CoInvestedSfa,
            });
          
            var results = PaymentAssertions.AssertPayments(expectations, actuals, "Test scenario");

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);


        }

        [Test]
        public void AssertAdditionalPaymentResults_WithSuccess()
        {

            var expectations = new List<ProviderEarningsAndPayments>();
            var actuals = new List<PaymentResult>();

            var periodearningAndPayments = new List<PeriodEarningAndPayments>();
            periodearningAndPayments.Add(new PeriodEarningAndPayments
            {
                Period = "09/17",
                ProviderPaidBySfa = 900,
                
            });
            periodearningAndPayments.Add(new PeriodEarningAndPayments
            {
                Period = "08/17",
                SfaNonLevyAdditionalPaymentsBudget = 100,
                ProviderEarnedTotal=900,
                SfaNonLevyCoFundingBudget=800

            });


            expectations.Add(
                new ProviderEarningsAndPayments
                {
                    EarningAndPaymentsByPeriod = periodearningAndPayments,
                    ProviderKey = "test"
                }
                );

            actuals.Add(new PaymentResult
            {
                Amount = 800,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithSfa,
                CalculationPeriod = "08/17",
                DeliveryPeriod = "08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.CoInvestedSfa,
            });

            actuals.Add(new PaymentResult
            {
                Amount = 100,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithSfa,
                CalculationPeriod = "08/17",
                DeliveryPeriod = "08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.FullyFundedSfa,
            });

            var results = PaymentAssertions.AssertPayments(expectations, actuals, "Test scenario");

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);


        }

        [Test]
        public void AssertAdditionalPaymentResults_WithErrors()
        {

            var expectations = new List<ProviderEarningsAndPayments>();
            var actuals = new List<PaymentResult>();

            var periodearningAndPayments = new List<PeriodEarningAndPayments>();
            periodearningAndPayments.Add(new PeriodEarningAndPayments
            {
                Period = "09/17",
                ProviderPaidBySfa = 900,

            });
            periodearningAndPayments.Add(new PeriodEarningAndPayments
            {
                Period = "08/17",
                SfaNonLevyAdditionalPaymentsBudget = 100,
                ProviderEarnedTotal = 1000,
                SfaNonLevyCoFundingBudget = 800

            });


            expectations.Add(
                new ProviderEarningsAndPayments
                {
                    EarningAndPaymentsByPeriod = periodearningAndPayments,
                    ProviderKey = "test"
                }
                );

            actuals.Add(new PaymentResult
            {
                Amount = 800,
                ContractType = Infrastructure.PaymentResults.ContractType.ContractWithSfa,
                CalculationPeriod = "08/17",
                DeliveryPeriod = "08/17",
                CollectionPeriodMonth = 8,
                CollectionPeriodYear = 2017,
                DeliveryMonth = 08,
                DeliveryYear = 2017,
                FundingSource = FundingSource.CoInvestedSfa,
            });

          

            var results = PaymentAssertions.AssertPayments(expectations, actuals, "Test scenario");

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);


        }

    }
}
