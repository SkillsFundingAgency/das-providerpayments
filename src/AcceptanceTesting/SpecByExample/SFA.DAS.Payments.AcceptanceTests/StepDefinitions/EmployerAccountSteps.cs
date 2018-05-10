using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;
using SFA.DAS.Payments.AcceptanceTests.TableParsers;
using SFA.DAS.Payments.AcceptanceTests.Assertions;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class EmployerAccountSteps
    {
        public EmployerAccountSteps(EmployerAccountContext employerAccountContext,
                                    EarningsAndPaymentsContext earningsAndPaymentsContext,
                                    SubmissionContext multipleSubmissionsContext,
                                    PeriodContext periodContext)
        {
            EmployerAccountContext = employerAccountContext;
            EarningsAndPaymentsContext = earningsAndPaymentsContext;
            MultipleSubmissionsContext = multipleSubmissionsContext;
            PeriodContext = periodContext;
        }
        public EmployerAccountContext EmployerAccountContext { get; }

        public EarningsAndPaymentsContext EarningsAndPaymentsContext { get; }

        public SubmissionContext MultipleSubmissionsContext { get; set; }

        public PeriodContext PeriodContext { get; set; }

        [Given("levy balance > agreed price for all months")]
        public void GivenUnnamedEmployersLevyBalanceIsMoreThanPrice()
        {
            GivenNamedEmployersLevyBalanceIsMoreThanPrice(Defaults.EmployerAccountId.ToString());
        }

        [Given("the employer (.*) has a levy balance > agreed price for all months")]
        public void GivenNamedEmployersLevyBalanceIsMoreThanPrice(string employerNumber)
        {
            int id;
            if (!int.TryParse(employerNumber, out id))
            {
                throw new ArgumentException($"Employer number '{employerNumber}' is not a valid number");
            }

            AddOrUpdateEmployerAccount(id, int.MaxValue);
        }

        [Given("levy balance = 0 for all months")]
        public void GivenLevyBalanceIsZero()
        {
            AddOrUpdateEmployerAccount(Defaults.EmployerAccountId, 0m);
        }

        [Given("the employer (.*) has a levy balance = 0 for all months")]
        public void GivenLevyBalanceIsZeroNamedEmployer(int employerId)
        {
            AddOrUpdateEmployerAccount(employerId, 0m);
        }

        [Given("the employer's levy balance is:")]
        public void GivenUnnamedEmployersLevyBalanceIsDifferentPerMonth(Table employerBalancesTable)
        {
            GivenNamedEmployersLevyBalanceIsDifferentPerMonth(Defaults.EmployerAccountId.ToString(), employerBalancesTable);
        }

        [Given("the employer (.*) has a levy balance of:")]
        public void GivenNamedEmployersLevyBalanceIsDifferentPerMonth(string employerNumber, Table employerBalancesTable)
        {
            int id;
            if (!int.TryParse(employerNumber, out id))
            {
                throw new ArgumentException($"Employer number '{employerNumber}' is not a valid number");
            }
            var periodBalances = LevyBalanceTableParser.ParseLevyAccountBalanceTable(employerBalancesTable,id);
            AddOrUpdateEmployerAccount(id, 0m, periodBalances);
        }

        [Given("the employer (.*) has a transfer allowance > agreed price for all months")]
        public void GivenNamedEmployersTransferAllowanceIsMoreThanPrice(int employerId)
        {
            UpdateTransferAllowance(employerId, int.MaxValue);
        }

        [Given("the employer (.*) has a transfer allowance = 0 for all months")]
        public void GivenNamedEmployersTransferAllowanceIsZeroForAllMonths(int employerId)
        {
            UpdateTransferAllowance(employerId, 0m);
        }

        [Given(@"the employer is not a levy payer")]
        public void GivenTheEmployerIsNotALevyPayer()
        {
            AddOrUpdateEmployerAccount(Defaults.EmployerAccountId,0m, isLevyPayer:false);
        }


        [Given("the learner changes employers")]
        public void GivenTheLearnerChangesEmployers(Table employmentDates)
        {
            foreach (var row in employmentDates.Rows)
            {
                if (!row[0].StartsWith("employer"))
                {
                    continue;
                }

                var employerAccountId = int.Parse(row[0].Substring("employer ".Length));
                var isDasEMployer = row[1].Equals("DAS", StringComparison.CurrentCultureIgnoreCase);

                var account = EmployerAccountContext.EmployerAccounts.SingleOrDefault(a => a.Id == employerAccountId);
                if (account == null)
                {
                    account = AddOrUpdateEmployerAccount(employerAccountId, 0, null, isDasEMployer);
                }
                account.IsDasEmployer = isDasEMployer;
            }
        }

       

        [Then(@"the net effect on employer's levy balance after each period end is:")]
        public void ThenTheEmployerSLevyBalanceIs(Table table)
        {
           
            var periodBalances = LevyBalanceTableParser.ParseLevyAccountBalanceTable(table,Defaults.EmployerAccountId);

            var breakdown = new  EarningsAndPaymentsBreakdown
            {
                EmployerLevyTransactions = periodBalances
            };

            EarningsAndPaymentsContext.OverallEarningsAndPayments.Add(breakdown);
            PaymentsAndEarningsAssertions.AssertPaymentsAndEarningsResults(EarningsAndPaymentsContext, PeriodContext, EmployerAccountContext);
        }


        private EmployerAccountReferenceData AddOrUpdateEmployerAccount(int id, 
                                                                        decimal balance, 
                                                                        List<EmployerAccountPeriodValue> periodBalances = null, 
                                                                        bool isDasEmployer = true,
                                                                        bool isLevyPayer = true)
        {
            var account = EmployerAccountContext.EmployerAccounts.SingleOrDefault(a => a.Id == id);
            if (account == null)
            {
                account = new EmployerAccountReferenceData
                {
                    Id = id
                };
                EmployerAccountContext.EmployerAccounts.Add(account);
            }

            account.Balance = balance;
            account.PeriodBalances = periodBalances ?? new List<EmployerAccountPeriodValue>();
            account.IsDasEmployer = isDasEmployer;
            account.IsLevyPayer = isLevyPayer;

            EmployerAccountManager.AddOrUpdateAccount(account);

            return account;
        }

        private void UpdateTransferAllowance(int employerId, decimal transferAllowance)
        {
            EmployerAccountManager.UpdateTransferBalance(employerId, transferAllowance);
        }
    }
}
