using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ParallelFramework;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class CommitmentSteps
    {
        public CommitmentSteps(CommitmentsContext commitmentsContext, LookupContext lookupContext, ParallelContext parallelContext)
        {
            CommitmentsContext = commitmentsContext;
            LookupContext = lookupContext;
            ParallelContext = parallelContext;
        }
        public CommitmentsContext CommitmentsContext { get; }
        public LookupContext LookupContext { get; }
        public ParallelContext ParallelContext { get; }

        [Given("the following commitments exist:")]
        public void GivenCommitmentsExistForLearners(Table commitments)
        {
            CommitmentsTableParser.ParseCommitmentsIntoContext(CommitmentsContext, commitments, LookupContext);
            CommitmentsContext.Commitments = ReferenceDataScenarioScopeHelper.ScopeToScenario(CommitmentsContext.Commitments,ParallelContext.GetCurrentScenario().Id);
            foreach (var commitment in CommitmentsContext.Commitments)
            {
                CommitmentManager.AddCommitment(commitment);
            }
        }

        [Given("the following commitments exist on (.*):")] // do we really care about the date?
        public void GivenCommitmentsExistForLearnersAtSpecificDate(string specDate, Table commitments)
        {
            GivenCommitmentsExistForLearners(commitments);
        }
    }
}
