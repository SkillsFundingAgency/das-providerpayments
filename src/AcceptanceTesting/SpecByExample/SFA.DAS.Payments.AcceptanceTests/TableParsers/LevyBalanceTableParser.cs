using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;
using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public static class LevyBalanceTableParser
    {
        internal static List<EmployerAccountPeriodValue> ParseLevyAccountBalanceTable(Table employerBalancesTable, int employerAccountId)
        {
            return SimpleEmployerAccountPeriodValueTableParser.ParseTable(employerBalancesTable, employerAccountId, "Balances");
        }
    }
}
