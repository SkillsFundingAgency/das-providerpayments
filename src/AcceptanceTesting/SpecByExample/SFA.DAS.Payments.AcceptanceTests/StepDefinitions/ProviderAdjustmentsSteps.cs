using SFA.DAS.Payments.AcceptanceTests.Assertions;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
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
            var easValuesForPeriods = TableParser.Transpose(table);
            ProviderAdjustmentsContext.AddSubmission(easValuesForPeriods);

            ProviderAdjustmentsContext.RunMonthEnd();

            ProviderAdjustmentsContext.SetCollectionPeriod(_testCollectionPeriod);
        }

        [When("the following EAS form is submitted:")]
        public void WhenTheFollowingEasEntriesAreSubmitted(Table table)
        {
            var easValuesForPeriods = TableParser.Transpose(table);
            ProviderAdjustmentsContext.AddSubmission(easValuesForPeriods);
        }

        [Then("the EAS payments are:")]
        public void ThenTheFollowingAdjustmentsWillBeGenerated(Table table)
        {
            ProviderAdjustmentsContext.RunMonthEnd();

            ProviderAdjustmentsContext.GetPayments();

            var paymentListForPeriods = ProviderAdjustmentsContext.TransposeTable(table);
            ProviderPaymentsAssertions.AssertEasPayments(ProviderAdjustmentsContext, paymentListForPeriods);
        }
    }
}