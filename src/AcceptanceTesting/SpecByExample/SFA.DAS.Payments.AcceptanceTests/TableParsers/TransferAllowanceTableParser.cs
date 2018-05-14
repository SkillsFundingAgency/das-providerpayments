using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public static class TransferAllowanceTableParser
    {
        internal static List<EmployerAccountPeriodValue> ParseTransferAllowanceTable(Table employerTransferAllowancesTable, int employerAccountId)
        {
            return SimpleEmployerAccountPeriodValueTableParser.ParseTable(employerTransferAllowancesTable, employerAccountId,"Transfer Allowances");
        }
    }
}