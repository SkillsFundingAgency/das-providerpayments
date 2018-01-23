﻿using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Assertions;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class DataLockSteps
    {
        public DataLockSteps(DataLockContext dataLockContext, CommitmentsContext commitmentsContext, MultipleSubmissionsContext multipleSubmissionsContext, EmployerAccountContext employerAccountContext, LookupContext lookupContext)
        {
            DataLockContext = dataLockContext;
            CommitmentsContext = commitmentsContext;
            MultipleSubmissionsContext = multipleSubmissionsContext;
            EmployerAccountContext = employerAccountContext;
            LookupContext = lookupContext;
        }

        public DataLockContext DataLockContext { get; }
        public CommitmentsContext CommitmentsContext { get; }
        public MultipleSubmissionsContext MultipleSubmissionsContext { get; }
        public EmployerAccountContext EmployerAccountContext { get; }
        public LookupContext LookupContext { get; }

        [Then(@"the following data lock event is returned:")]
        public void ThenTheFollowingDataLockEventIsReturned(Table table)
        {
            EnsureSubmissionsHaveHappened();

            DataLockEventsTableParser.ParseDataLockEventsIntoContext(DataLockContext, table, LookupContext);

            DataLockAssertions.AssertDataLockOutput(DataLockContext, MultipleSubmissionsContext.SubmissionResults.ToArray());
        }

        [Then("no data lock event is returned")]
        public void ThenNoDataLockEventIsReturned()
        {
            EnsureSubmissionsHaveHappened();

            DataLockContext.ExpectsNoDataLockEvents = true;

            DataLockAssertions.AssertDataLockOutput(DataLockContext, MultipleSubmissionsContext.SubmissionResults.ToArray());
        }

        [Then(@"the data lock event has the following errors:")]
        public void ThenTheDataLockEventHasTheFollowingErrors(Table table)
        {
            EnsureSubmissionsHaveHappened();

            DataLockEventErrorsTableParser.ParseDataLockEventErrorsIntoContext(DataLockContext, table, LookupContext);

            DataLockAssertions.AssertDataLockOutput(DataLockContext, MultipleSubmissionsContext.SubmissionResults.ToArray());
        }

        [Then(@"the data lock event has the following periods")]
        public void ThenTheDataLockEventHasTheFollowingPeriods(Table table)
        {
            EnsureSubmissionsHaveHappened();

            DataLockEventPeriodTableParser.ParseDataLockEventPeriodsIntoContext(DataLockContext, table, LookupContext);

            DataLockAssertions.AssertDataLockOutput(DataLockContext, MultipleSubmissionsContext.SubmissionResults.ToArray());
        }

        [Then(@"the data lock event used the following commitments")]
        public void ThenTheDataLockEventUsedTheFollowingCommitments(Table table)
        {
            EnsureSubmissionsHaveHappened();

            DataLockEventCommitmentsTableParser.ParseDataLockEventCommitmentsIntoContext(DataLockContext, table, LookupContext);

            DataLockAssertions.AssertDataLockOutput(DataLockContext, MultipleSubmissionsContext.SubmissionResults.ToArray());
        }


        private void EnsureSubmissionsHaveHappened()
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                if (!submission.HaveSubmissionsBeenDone)
                {
                    var periodsToSubmitTo = new[]
                    {
                        CommitmentsContext.Commitments.Max(x => x.EffectiveFrom).ToString("MM/yy")
                    };
                    MultipleSubmissionsContext.SubmissionResults.AddRange(SubmissionManager.SubmitIlrAndRunMonthEndAndCollateResults(
                        submission.IlrLearnerDetails,
                        submission.FirstSubmissionDate,
                        LookupContext,
                        EmployerAccountContext.EmployerAccounts,
                        submission.ContractTypes,
                        submission.EmploymentStatus,
                        submission.LearningSupportStatus,
                        periodsToSubmitTo));
                    submission.HaveSubmissionsBeenDone = true;
                }
            }
        }

    }
}