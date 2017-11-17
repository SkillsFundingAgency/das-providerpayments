using System;
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
        private string _testCollectionPeriod;

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
            ProviderAdjustmentsContext.CleanEnvirnment();
            ProviderAdjustmentsContext.SetCollectionPeriod(collectionPeriod);
            _testCollectionPeriod = collectionPeriod;
        }

        [Given("the following EAS form is submitted in (.*):")]
        public void WhenTheFollowingHistoricFormIsSubmitted(string period, Table table)
        {
            ProviderAdjustmentsContext.SetCollectionPeriod(period);
            var submissionPeriods = TableParser.Transpose(table);
            ProviderAdjustmentsContext.AddSubmission(submissionPeriods);

            ProviderAdjustmentsContext.RunMonthEnd();

            ProviderAdjustmentsContext.SetCollectionPeriod(_testCollectionPeriod);
        }

        [When("the following EAS form is submitted:")]
        public void WhenTheFollowingEasEntriesAreSubmitted(Table table)
        {
            var periods = TableParser.Transpose(table);
            ProviderAdjustmentsContext.AddSubmission(periods);
        }

        [Then("the EAS payments are:")]
        public void ThenTheFollowingAdjustmentsWillBeGenerated(Table table)
        {
            ProviderAdjustmentsContext.RunMonthEnd();

            var periods = ProviderAdjustmentsContext.TransposeTable(table);
            foreach (var period in periods)
            {
                var paymentPeriod = new PeriodDefinition(period.Period);
                var earningsPeriod = paymentPeriod.TransformPaymentPeriodToEarningsPeriod();

                if (earningsPeriod == null)
                {
                    foreach (var row in period.Rows)
                    {
                        if (row.Amount != 0)
                        {
                            throw new ApplicationException(
                                $"The payment {row.Name} for period {period} is made before a payment is possible for this year");
                        }
                    }
                    continue;
                }

                var payments = ProviderAdjustmentsContext.PaymentsFor(
                    earningsPeriod.CollectionMonth, earningsPeriod.CollectionYear)
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
                        throw new ApplicationException($"expected: {row.Amount} found: {payment} for {row.Name} for period {period.Period}");
                    }
                }
            }
        }
    }
}