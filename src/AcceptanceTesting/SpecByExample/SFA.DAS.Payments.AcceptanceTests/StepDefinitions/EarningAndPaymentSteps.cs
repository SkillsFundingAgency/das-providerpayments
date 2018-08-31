using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Assertions;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using TechTalk.SpecFlow;
using System;
using SFA.DAS.Payments.AcceptanceTests.Refactoring.ExecutionManagers;
using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.DataCollectors;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class EarningAndPaymentSteps
    {
        public EarningAndPaymentSteps(EmployerAccountContext employerAccountContext,
                                      EarningsAndPaymentsContext earningsAndPaymentsContext,
                                      DataLockContext dataLockContext,
                                      SubmissionDataLockContext submissionDataLockContext,
                                      LookupContext lookupContext,
                                    CommitmentsContext commitmentsContext,
                                    SubmissionContext multipleSubmissionsContext,
                                    PeriodContext periodContext,
                                    TransfersContext transfersContext)
        {
            EmployerAccountContext = employerAccountContext;
            EarningsAndPaymentsContext = earningsAndPaymentsContext;
            DataLockContext = dataLockContext;
            SubmissionDataLockContext = submissionDataLockContext;
            LookupContext = lookupContext;
            CommitmentsContext = commitmentsContext;
            MultipleSubmissionsContext = multipleSubmissionsContext;
            PeriodContext = periodContext;
            TransfersContext = transfersContext;
        }
        public EmployerAccountContext EmployerAccountContext { get; }
        public DataLockContext DataLockContext { get; }
        public SubmissionDataLockContext SubmissionDataLockContext { get; }
        public EarningsAndPaymentsContext EarningsAndPaymentsContext { get; }
        public LookupContext LookupContext { get; }
        public CommitmentsContext CommitmentsContext { get; }
        public SubmissionContext MultipleSubmissionsContext { get; set; }
        public PeriodContext PeriodContext { get; set; }
        public TransfersContext TransfersContext { get; set; }



        [Then("the provider earnings and payments break down as follows:")]
        public void ThenProviderEarningAndPaymentsBreakDownTo(Table earningAndPayments)
        {
            ThenProviderEarningAndPaymentsBreakDownTo(Defaults.ProviderIdSuffix, earningAndPayments);
        }

        [Then("OBSOLETE - the provider earnings and payments break down as follows:"), Obsolete]
        public void ThenProviderEarningAndPaymentsBreakDownToObsolete(Table earningAndPayments)
        {
            ThenProviderEarningAndPaymentsBreakDownToObsolete(Defaults.ProviderIdSuffix, earningAndPayments);
        }

        [Then("OBSOLETE - the earnings and payments break down for provider (.*) is as follows:"), Obsolete]
        public void ThenProviderEarningAndPaymentsBreakDownToObsolete(string providerIdSuffix, Table earningAndPayments)
        {
            var providerBreakdown = EarningsAndPaymentsContext.OverallEarningsAndPayments.SingleOrDefault(x => x.ProviderId == "provider " + providerIdSuffix);
            if (providerBreakdown == null)
            {
                providerBreakdown = new EarningsAndPaymentsBreakdown { ProviderId = "provider " + providerIdSuffix };
                EarningsAndPaymentsContext.OverallEarningsAndPayments.Add(providerBreakdown);
            }

            EarningAndPaymentTableParser.ParseEarningsAndPaymentsTableIntoContext(providerBreakdown, earningAndPayments);

            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                if (!submission.HaveSubmissionsBeenDone)
                {
                    PeriodContext.PeriodResults.AddRange(SubmissionManager.SubmitIlrAndRunMonthEndAndCollateResults(
                        submission.IlrLearnerDetails, submission.FirstSubmissionDate,
                        LookupContext, EmployerAccountContext.EmployerAccounts, submission.ContractTypes,
                        submission.EmploymentStatus, submission.LearningSupportStatus, CommitmentsContext, 
                        lastAssertionPeriodDate: providerBreakdown.PeriodDates.Max()));
                    submission.HaveSubmissionsBeenDone = true;
                }
            }

            AssertResults();
        }

        [Then("the earnings and payments break down for provider (.*) is as follows:")]
        public void ThenProviderEarningAndPaymentsBreakDownTo(string providerIdSuffix, Table earningAndPayments)
        {
            var providerBreakdown = EarningsAndPaymentsContext.OverallEarningsAndPayments.SingleOrDefault(x => x.ProviderId == "provider " + providerIdSuffix);
            if (providerBreakdown == null)
            {
                providerBreakdown = new EarningsAndPaymentsBreakdown { ProviderId = "provider " + providerIdSuffix };
                EarningsAndPaymentsContext.OverallEarningsAndPayments.Add(providerBreakdown);
            }

            EarningAndPaymentTableParser.ParseEarningsAndPaymentsTableIntoContext(providerBreakdown, earningAndPayments);

            PeriodContext.PeriodResults.AddRange(SubmissionManager.SubmitMultipleIlrAndRunMonthEndAndCollateResults(MultipleSubmissionsContext, LookupContext,
                EmployerAccountContext.EmployerAccounts, providerBreakdown.PeriodDates.Max(), CommitmentsContext));

            PeriodContext.TransferResults = MultipleSubmissionsContext.TransferResults;

            AssertResults();
        }

        [Then("the transaction types for the payments are:")]
        public void ThenTheTransactionTypesForEarningsAre(Table earningBreakdown)
        {
            ThenTheTransactionTypesForNamedProviderEarningsAre(Defaults.ProviderIdSuffix, earningBreakdown);
        }

        [Then("the transaction types for the payments for provider (.*) are:")]
        public void ThenTheTransactionTypesForNamedProviderEarningsAre(string providerIdSuffix, Table transactionTypes)
        {
            TransactionTypeTableParser.ParseTransactionTypeTableIntoContext(EarningsAndPaymentsContext, $"provider {providerIdSuffix}", transactionTypes);

            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                if (!submission.HaveSubmissionsBeenDone)
                {
                    PeriodContext.PeriodResults.AddRange(SubmissionManager.SubmitIlrAndRunMonthEndAndCollateResults(
                        submission.IlrLearnerDetails, submission.FirstSubmissionDate,
                        LookupContext, EmployerAccountContext.EmployerAccounts, submission.ContractTypes,
                        submission.EmploymentStatus, submission.LearningSupportStatus, CommitmentsContext,
                        lastAssertionPeriodDate: EarningsAndPaymentsContext.PeriodDates.Max()));
                    submission.HaveSubmissionsBeenDone = true;
                }
            }

            AssertResults();
        }

        [Then(@"the provider earnings and payments break down for ULN (.*) as follows:")]
        public void ThenTheProviderEarningsAndPaymentsBreakDownForUlnAsFollows(string learnerId, Table earningAndPayments)
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                if (!submission.HaveSubmissionsBeenDone)
                {
                    PeriodContext.PeriodResults.AddRange(SubmissionManager.SubmitIlrAndRunMonthEndAndCollateResults(
                        submission.IlrLearnerDetails, submission.FirstSubmissionDate,
                        LookupContext, EmployerAccountContext.EmployerAccounts, submission.ContractTypes,
                        submission.EmploymentStatus, submission.LearningSupportStatus, CommitmentsContext));
                    submission.HaveSubmissionsBeenDone = true;
                }
            }

            var breakdown = new LearnerEarningsAndPaymentsBreakdown
            {
                ProviderId = Defaults.ProviderId, // This may not be true in every case, need to check specs
                LearnerReferenceNumber = learnerId
            };
            EarningsAndPaymentsContext.LearnerOverallEarningsAndPayments.Add(breakdown);
            EarningAndPaymentTableParser.ParseEarningsAndPaymentsTableIntoContext(breakdown, earningAndPayments);
            AssertResults();
        }

        [Then(@"the following transfers from employer (.*) exist for the given months of earnings activity:")]
        public void ThenTheFollowingTransfersFromEmployerExists(string sendingEmployerIdSuffix, Table transfers)
        {
            VerifyAllSubmissionsHaveBeenDone("All submissions must have been completed prior to the transfers assertion step.");
            TransfersTableParser.ParseTransfersTableIntoContext(TransfersContext.TransfersBreakdown, transfers, int.Parse(sendingEmployerIdSuffix));
            TransfersAssertions.ValidateTransfers(TransfersContext.TransfersBreakdown, PeriodContext.TransferResults);
        }

        private void AssertResults()
        {
            PaymentsAndEarningsAssertions.AssertPaymentsAndEarningsResults(EarningsAndPaymentsContext, PeriodContext, EmployerAccountContext);
            TransactionTypeAssertions.AssertPaymentsAndEarningsResults(EarningsAndPaymentsContext, PeriodContext, EmployerAccountContext);
            SubmissionDataLockAssertions.AssertPaymentsAndEarningsResults(SubmissionDataLockContext, PeriodContext);
        }


        [Given(@"following learning has been recorded for previous payments:")]
        public void GivenFollowingLearningHasBeenRecordedForPreviousSubmission(Table table)
        {
            IlrTableParser.ParseIlrTableIntoSubmission(MultipleSubmissionsContext.HistoricalLearningDetails, table);
        }

        [Given(@"the following (.*) earnings and payments have been made to the (.*) for (.*):")]
        public void GivenTheFollowingEarningsAndPaymentsHaveBeenMadeToTheProviderAForLearnerA(string aimType, string providerName, string learnerRefererenceNumber, Table table)
        {
            var paymentsAimType = (AimType)aimType.ToEnumByDescription(typeof(AimType));
            CreatePreviousEarningsAndPayments(providerName, learnerRefererenceNumber, table, paymentsAimType);
        }

        [Given(@"the following earnings and payments have been made to the (.*) for (.*):")]
        public void GivenTheFollowingEarningsAndPaymentsHaveBeenMadeToTheProviderAForLearnerA(string providerName, string learnerRefererenceNumber, Table table)
        {
            CreatePreviousEarningsAndPayments(providerName, learnerRefererenceNumber, table, AimType.Programme);
        }

        [Given(@"the following payments have been made to the (.*) for (.*):")]
        public void GivenTheFollowingPaymentsHaveBeenMadeToTheProviderAForLearnerA(string providerName, string learnerRefererenceNumber, Table table)
        {
            var context = new EarningsAndPaymentsContext();
            TransactionTypeTableParser.ParseTransactionTypeTableIntoContext(context, providerName, table);


            var learningDetails = MultipleSubmissionsContext.HistoricalLearningDetails.Single(x => x.LearnerReference.Equals(learnerRefererenceNumber, StringComparison.InvariantCultureIgnoreCase));

            long learnerUln;
            if (!string.IsNullOrEmpty(learningDetails.Uln))
            {
                learnerUln = long.Parse(learningDetails.Uln);
                LookupContext.AddUln(learnerRefererenceNumber, learnerUln);
            }
            else
            {
                learnerUln = LookupContext.AddOrGetUln(learnerRefererenceNumber);
            }


            var provider = LookupContext.AddOrGetUkprn(providerName);

            var commitment = CommitmentsContext.Commitments.FirstOrDefault(x => x.ProviderId == providerName && x.LearnerId == learnerRefererenceNumber);

            CreatePayment(context.ProviderEarnedForOnProgramme, provider, learnerUln, learnerRefererenceNumber, commitment, learningDetails,TransactionType.OnProgram, commitment==null ? FundingSource.CoInvestedSfa : FundingSource.Levy);
            CreatePayment(context.ProviderEarnedForLearningSupport, provider, learnerUln, learnerRefererenceNumber, commitment, learningDetails, TransactionType.LearningSupport, FundingSource.FullyFundedSfa);
            CreatePayment(context.ProviderEarnedForFrameworkUpliftOnProgramme, provider, learnerUln, learnerRefererenceNumber, commitment, learningDetails, TransactionType.OnProgramme16To18FrameworkUplift,FundingSource.FullyFundedSfa);

        }

       
        [Given(@"the following payments will be added for reversal:")]
        public void GivenTheFollowingPaymentsWillBeAddedForReversal(Table table)
        {
            var context = new EarningsAndPaymentsContext();
            TransactionTypeTableParser.ParseTransactionTypeTableIntoContext(context, "", table);
            
            foreach(var row in context.ProviderEarnedForOnProgramme)
            {
                var month = Int32.Parse(row.PeriodName.Split('/')[0]);
                var year = Int32.Parse($"20" + row.PeriodName.Split('/')[1]);
                PaymentsManager.AddRequiredPaymentForReversal(month, year, row.Value, TransactionType.OnProgram);
            }
        }


        private void CreatePayment(List<ProviderEarnedPeriodValue> paymentValues, long ukprn, 
                                    long uln, string learnRefNumber, 
                                    CommitmentReferenceData commitment, 
                                    IlrLearnerReferenceData learningDetails,
                                    TransactionType transactionType,
                                    FundingSource fundingSource)
        {

            foreach (var payment in paymentValues)
            {
                if (payment.Value > 0)
                {
                    var requiredPaymentId = Guid.NewGuid().ToString();
                    var month = int.Parse(payment.PeriodName.Substring(0, 2));
                    var year = int.Parse(payment.PeriodName.Substring(3, 2)) + 2000;
                    var date = new DateTime(year, month, 1);
                    var periodNumber = date.GetPeriodNumber();
                    
                    var yearPortion = int.Parse(payment.PeriodName.Substring(3, 2));
                    string academicYear;
                    if (month < 8)
                    {
                        academicYear = $"{(yearPortion - 1)}{yearPortion}";
                    }
                    else
                    {
                        academicYear = $"{yearPortion}{yearPortion + 1}";
                    }
                    var periodName = $"{academicYear}-R" + periodNumber.ToString("00");


                    PaymentsManager.SavePaymentDue(requiredPaymentId, ukprn, uln, commitment, learnRefNumber, periodName,
                                                    month, year, (int)transactionType, payment.Value, learningDetails);

                    PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year, (int)transactionType, fundingSource, payment.Value);

                }
            }
        }


        private void CreatePreviousEarningsAndPayments(string providerName, string learnerRefererenceNumber, Table table, AimType paymentsAimType)
        {

            var learnerBreakdown = new EarningsAndPaymentsBreakdown { ProviderId = providerName };
            EarningAndPaymentTableParser.ParseEarningsAndPaymentsTableIntoContext(learnerBreakdown, table);

            var learningDetails = MultipleSubmissionsContext.HistoricalLearningDetails.Single(x => x.AimType == paymentsAimType && x.LearnerReference.Equals(learnerRefererenceNumber, StringComparison.InvariantCultureIgnoreCase));

            long learnerUln;
            if (!string.IsNullOrEmpty(learningDetails.Uln))
            {
                learnerUln = long.Parse(learningDetails.Uln);
                LookupContext.AddUln(learnerRefererenceNumber, learnerUln);
            }
            else
            {
                learnerUln = LookupContext.AddOrGetUln(learnerRefererenceNumber);
            }


            var provider = LookupContext.AddOrGetUkprn(learnerBreakdown.ProviderId);

            var commitment = CommitmentsContext.Commitments.FirstOrDefault(x => x.ProviderId == learnerBreakdown.ProviderId && x.LearnerId == learnerRefererenceNumber);

            foreach (var earned in learnerBreakdown.ProviderEarnedTotal)
            {
                var requiredPaymentId = Guid.NewGuid().ToString();
                var month = int.Parse(earned.PeriodName.Substring(0, 2));
                int year = int.Parse(earned.PeriodName.Substring(3, 2)) + 2000;

                var date = new DateTime(year, month, 1);
                var periodNumber = date.GetPeriodNumber();

                string academicYear;
                var yearPortion = int.Parse(earned.PeriodName.Substring(3, 2));
                if (month < 8)
                {
                    academicYear = $"{(yearPortion - 1)}{yearPortion}";
                }
                else
                {
                    academicYear = $"{yearPortion}{yearPortion + 1}";
                }
                var periodName = $"{academicYear}-R" + periodNumber.ToString("00");

                if (earned.Value > 0)
                {
                    PaymentsManager.SavePaymentDue(requiredPaymentId, provider, learnerUln,
                                                        commitment, learnerRefererenceNumber, periodName,
                                                        month, year, learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish
                                                        , earned.Value, learningDetails);

                    var levyPayment = learnerBreakdown.SfaLevyBudget.Where(x => x.PeriodName == earned.PeriodName).SingleOrDefault();
                    if (levyPayment != null && levyPayment.Value > 0)
                    {
                        PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year,
                                                           learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish, FundingSource.Levy, levyPayment.Value);
                    }

                    var earnedFromEmployer = learnerBreakdown.ProviderEarnedFromEmployers.Where(x => x.PeriodName == earned.PeriodName).SingleOrDefault();
                    if (earnedFromEmployer != null && earnedFromEmployer.Value > 0)
                    {
                        PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year,
                                                           learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish, FundingSource.CoInvestedEmployer, earnedFromEmployer.Value);
                    }

                    var coInvestedBySfaLevy = learnerBreakdown.SfaLevyCoFundBudget.Where(x => x.PeriodName == earned.PeriodName).SingleOrDefault();
                    if (coInvestedBySfaLevy != null && coInvestedBySfaLevy.Value > 0)
                    {
                        PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year,
                                                           learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish, FundingSource.CoInvestedSfa, coInvestedBySfaLevy.Value);
                    }

                    var coInvestedBySfaNonLevy = learnerBreakdown.SfaNonLevyCoFundBudget.Where(x => x.PeriodName == earned.PeriodName).SingleOrDefault();
                    if (coInvestedBySfaNonLevy != null && coInvestedBySfaNonLevy.Value > 0)
                    {
                        PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year,
                                                          learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish, FundingSource.CoInvestedSfa, coInvestedBySfaNonLevy.Value);
                    }

                    var aditionalPayments = learnerBreakdown.SfaLevyAdditionalPayments.Where(x => x.PeriodName == earned.PeriodName).SingleOrDefault();
                    if (aditionalPayments != null && aditionalPayments.Value > 0)
                    {
                        PaymentsManager.SavePayment(requiredPaymentId, periodName, month, year,
                                                          learningDetails.AimType == AimType.Programme ? (int)TransactionType.OnProgram : (int)TransactionType.OnProgrammeMathsAndEnglish, FundingSource.FullyFundedSfa, aditionalPayments.Value);
                    }

                }
            }


        }

        private void VerifyAllSubmissionsHaveBeenDone(string message)
        {
            foreach (var submission in MultipleSubmissionsContext.Submissions)
            {
                if(!submission.HaveSubmissionsBeenDone)
                    throw new Exception($"Not all submissions have been run. {message}");
            }
        }
    }
}
