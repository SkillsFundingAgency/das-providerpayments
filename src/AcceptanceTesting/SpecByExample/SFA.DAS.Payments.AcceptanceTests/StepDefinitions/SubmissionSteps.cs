using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class SubmissionSteps
    {
        public SubmissionSteps(CommitmentsContext commitmentsContext, LookupContext lookupContext, MultipleSubmissionsContext multipleSubmissionsContext)
        {
            CommitmentsContext = commitmentsContext;
            LookupContext = lookupContext;
            MultipleSubmissionsContext = multipleSubmissionsContext;
        }
        public CommitmentsContext CommitmentsContext { get; }
        public LookupContext LookupContext { get; }
        public MultipleSubmissionsContext MultipleSubmissionsContext { get; set; }

        [When("an ILR file is submitted with the following data:")]
        [When(@"an ILR file is submitted every month with the following data:")] //Duplicate?
        public void WhenAnIlrFileIsSubmitted(Table ilrDetails)
        {
            var submissionContext = new Submission();
            IlrTableParser.ParseIlrTableIntoContext(submissionContext, ilrDetails);
            MultipleSubmissionsContext.Submissions.Add(submissionContext);
        }

        [When("an ILR file is submitted for period (.*) with the following data:")]
        public void WhenAnIlrFileIsSubmittedForPeriod(string period, Table ilrDetails)
        {
            var submissionContext = new Submission() {SubmissionPeriod = period};
            IlrTableParser.ParseIlrTableIntoContext(submissionContext, ilrDetails);
            MultipleSubmissionsContext.Submissions.Add(submissionContext);
        }

        [When("the providers submit the following ILR files:")] //Duplicate?
        public void WhenMultipleIlrFilesAreSubmitted(Table ilrDetails)
        {
            var submissionContext = new Submission();
            IlrTableParser.ParseIlrTableIntoContext(submissionContext, ilrDetails);
            MultipleSubmissionsContext.Submissions.Add(submissionContext);
        }

        [When(@"an ILR file is submitted on (.*) with the following data:")] // what is the purpose of the dates?
        public void WhenIlrSubmittedOnSpecificDate(string specSumissionDate, Table ilrDetails)
        {
            var submissionContext = new Submission();
            IlrTableParser.ParseIlrTableIntoContext(submissionContext, ilrDetails);
            MultipleSubmissionsContext.Submissions.Add(submissionContext);
        }

        [When("an ILR file is submitted for the first time on (.*) with the following data:")]
        public void WhenIlrFirstSubmittedOnSpecificDate(string specSumissionDate, Table ilrDetails)
        {
            var submissionContext = new Submission();
            IlrTableParser.ParseIlrTableIntoContext(submissionContext, ilrDetails);
            

            DateTime firstSubmissionDate;
            if (!DateTime.TryParse(specSumissionDate, out firstSubmissionDate))
            {
                throw new ArgumentException($"{specSumissionDate} is not a valid date");
            }
            
            submissionContext.FirstSubmissionDate = firstSubmissionDate;
            MultipleSubmissionsContext.Submissions.Add(submissionContext);
        }

        [Obsolete]
        [When("the Contract type in the ILR is:")] //constrains spec to single ILR
        public void WhenTheContractTypeInTheIlrIs(Table contractTypes)
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                ContractTypeTableParser.ParseContractTypesIntoContext(submission, contractTypes);
            }
        }

        [Obsolete]
        [When("the employment status in the ILR is:")] //constrains spec to single ILR
        public void WhenTheEmploymentStatusInTheIlrIs(Table employmentStatus)
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                EmploymentStatusTableParser.ParseEmploymentStatusIntoContext(submission, employmentStatus);
            }
        }

        [Obsolete]
        [When(@"the learning support status of the ILR is:")] //constrains spec to single ILR
        public void WhenTheLearningSupportStatusOfTheIlrIs(Table learningSupportStatus)
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                LearningSupportTableParser.ParseLearningSupportIntoContext(submission, learningSupportStatus);
            }
        }
    }
}
