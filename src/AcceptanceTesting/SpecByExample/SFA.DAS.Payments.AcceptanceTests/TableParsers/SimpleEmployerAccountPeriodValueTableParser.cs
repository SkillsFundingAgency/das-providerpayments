using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public static class SimpleEmployerAccountPeriodValueTableParser
    {
        internal static List<EmployerAccountPeriodValue> ParseTable(Table table, int employerAccountId, string tableName)
        {

            if (table.RowCount > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(table), $"{tableName} table can only contain a single row");
            }

            var periodValues = new List<EmployerAccountPeriodValue>();
            for (var c = 0; c < table.Header.Count; c++)
            {
                var periodName = table.Header.ElementAt(c);
                if (periodName == "...")
                {
                    continue;
                }
                if (!Validations.IsValidPeriodName(periodName))
                {
                    throw new ArgumentException($"'{periodName}' is not a valid period name format. Expected MM/YY");
                }

                int periodBalance;
                if (!int.TryParse(table.Rows[0][c], out periodBalance))
                {
                    throw new ArgumentException($"Balance '{table.Rows[0][c]}' is not a value balance");
                }

                periodValues.Add(new EmployerAccountPeriodValue
                {
                    PeriodName = PeriodArrearsHelper.GetEarningsMonthForPaymentsMadeIn(periodName),
                    Value = periodBalance,
                    EmployerAccountId = employerAccountId
                });
            }

            return periodValues;
        }
    }
}