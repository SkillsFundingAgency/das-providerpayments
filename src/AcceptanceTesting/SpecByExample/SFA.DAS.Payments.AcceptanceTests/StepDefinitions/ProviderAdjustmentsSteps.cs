using System.Linq;
using FluentAssertions;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ProviderAdjustmentsSteps
    {
        public ProviderAdjustmentsSteps(
            ProviderAdjustmentsContext providerAdjustmentsContext)
        {
            ProviderAdjustmentsContext = providerAdjustmentsContext;
        }

        public ProviderAdjustmentsContext ProviderAdjustmentsContext { get; }

        [Given("that the previous EAS entries for a provider are as follows:")]
        public void GivenThatThePreviousEasEntrierForAProviderAreAsFollows(Table table)
        {
            
        }

        [Given("the EAS collection period is (.*)")]
        public void GivenThatTheEasCollectionPeriodIs(string collectionPeriod)
        {
            ProviderAdjustmentsContext.SetCollectionPeriod(collectionPeriod);
        }

        [Given("the following EAS form is submitted in (.*):")]
        public void WhenTheFollowingHistoricFormIsSubmitted(string period, Table table)
        {
            
        }

        [When("the following EAS form is submitted:")]
        public void WhenTheFollowingEasEntriesAreSubmitted(Table table)
        {
            var periods = TableParser.Transpose(table);
            ProviderAdjustmentsContext.AddSubmissions(periods);
        }

        [Then("the EAS payments are:")]
        public void ThenTheFollowingAdjustmentsWillBeGenerated(Table table)
        {
            TestEnvironment.ProcessService.RunSummarisation(TestEnvironment.Variables);
            var periods = ProviderAdjustmentsContext.TransposeTable(table);
            foreach (var period in periods)
            {
                var paymentPeriod = new PeriodDefinition(period.Period);
                var earningsPeriod = paymentPeriod.TransformPaymentPeriodToEarningsPeriod();

                if (earningsPeriod == null)
                {
                    continue;
                }

                var payments = ProviderAdjustmentsRepository.GetEasPaymentsFor(
                    earningsPeriod.CollectionMonth, earningsPeriod.CollectionYear)
                    .Select(x => new
                    {
                        x.PaymentType,
                        x.PaymentTypeName,
                        x.Amount,
                    });

                foreach (var row in period.Rows)
                {
                    var payment = payments.FirstOrDefault(x => x.PaymentTypeName == row.Name);
                    if (row.Amount == 0)
                    {
                        payment.Should().BeNull($"the payment {row.Name} for period: {period.Period} should not exist");
                    }
                    else
                    {
                        payment.Should().NotBeNull($"the payment {row.Name} for period: {period.Period} should exist");
                        payment.Amount.Should().Be(row.Amount, $"the payment amount was for period {period.Period}");
                    }
                }
            }
        }
    }
}