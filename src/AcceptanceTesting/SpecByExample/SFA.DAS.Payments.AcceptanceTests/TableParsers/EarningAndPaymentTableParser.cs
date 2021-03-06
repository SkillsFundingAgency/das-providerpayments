﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    internal static class EarningAndPaymentTableParser
    {
        internal static void ParseEarningsAndPaymentsTableIntoContext(EarningsAndPaymentsBreakdown breakdown, Table earningAndPayments)
        {
            if (earningAndPayments.Rows.Count < 1)
            {
                throw new ArgumentException("Earnings and payments table must have at least 1 row");
            }

            var periodNames = ParseEarningAndPaymentsHeaders(earningAndPayments, "Type");
            ParseEarningAndPaymentsRows(breakdown, earningAndPayments, periodNames);
        }


        internal static string[] ParseEarningAndPaymentsHeaders(Table earningAndPayments, string expectedFirstColumn)
        {
            var headers = earningAndPayments.Header.ToArray();
            if (headers[0] != expectedFirstColumn)
            {
                throw new ArgumentException($"Earnings and payments table must have {expectedFirstColumn} as first column");
            }

            var periods = new string[headers.Length];
            for (var c = 1; c < headers.Length; c++)
            {
                var periodName = headers[c];
                if (periodName == "...")
                {
                    continue;
                }
                if (!Validations.IsValidPeriodName(periodName))
                {
                    throw new ArgumentException($"'{periodName}' is not a valid period name format. Expected MM/YY");
                }

                periods[c] = periodName;
            }
            return periods;
        }
        private static void ParseEarningAndPaymentsRows(EarningsAndPaymentsBreakdown breakdown, Table earningAndPayments, string[] periodNames)
        {
            breakdown.PeriodDates = periodNames
                .Skip(1)
                .Select(name => PeriodNameHelper.GetDateFromStringDate(name).GetValueOrDefault())
                .ToList();

            foreach (var row in earningAndPayments.Rows)
            {
                Match match;
                if (row[0].Equals("Provider Earned Total", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.ProviderEarnedTotal);
                }
                else if (row[0].Equals("Provider Earned from SFA", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.ProviderEarnedFromSfa);
                }
                else if (row[0].Equals("Provider Earned from Employer", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseEmployerRow(Defaults.EmployerAccountId.ToString(), row, periodNames, breakdown.ProviderEarnedFromEmployers);
                }
                else if ((match = Regex.Match(row[0], "Provider Earned from Employer ([0-9]{1,})", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.ProviderEarnedFromEmployers);
                }
                else if (row[0].Equals("Provider Paid by SFA", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.ProviderPaidBySfa);
                }
                else if ((match = Regex.Match(row[0], "Provider Paid by SFA for ULN ([0-9]{1,9}$)", RegexOptions.IgnoreCase)).Success)
                {
                    ParseUlnRow(match.Groups[1].Value, row, periodNames, breakdown.ProviderPaidBySfaForUln);
                }
                else if (row[0].Equals("Payment due from Employer", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseEmployerRow(Defaults.EmployerAccountId.ToString(), row, periodNames, breakdown.PaymentDueFromEmployers);
                }
                else if ((match = Regex.Match(row[0], "Payment due from Employer ([0-9]{1,})", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.PaymentDueFromEmployers);
                }
                else if (row[0].Equals("Levy account debited", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseEmployerRow(Defaults.EmployerAccountId.ToString(), row, periodNames, breakdown.EmployersLevyAccountDebited);
                }
                else if ((match = Regex.Match(row[0], "employer ([0-9]{1,}) Levy account debited$", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.EmployersLevyAccountDebited);
                }
                else if ((match = Regex.Match(row[0], "employer ([0-9]{1,}) Levy account debited for ULN ([0-9]{1,9})$", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerUlnRow(match.Groups[1].Value, match.Groups[2].Value, row, periodNames, breakdown.EmployersLevyAccountDebitedForUln);
                }
                else if ((match = Regex.Match(row[0], "employer ([0-9]{1,}) Levy account debited via transfer", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.EmployersLevyAccountDebitedViaTransfer);
                }
                else if ((match = Regex.Match(row[0], "employer ([0-9]{1,}) Levy account debited for ULN ([0-9]{1,9}) via transfer$", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerUlnRow(match.Groups[1].Value, match.Groups[2].Value, row, periodNames, breakdown.EmployersLevyAccountDebitedForUlnViaTransfer);
                }
                else if ((match = Regex.Match(row[0], "employer ([0-9]{1,}) Levy account credited$", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.EmployersLevyAccountCredited);
                }
                else if (row[0].Equals("SFA Levy employer budget", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.SfaLevyBudget);
                }
                else if (row[0].Equals("SFA Levy co-funding budget", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.SfaLevyCoFundBudget);
                }
                else if (row[0].Equals("SFA non-Levy co-funding budget", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.SfaNonLevyCoFundBudget);
                }
                else if (row[0].Equals("SFA Levy additional payments budget", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.SfaLevyAdditionalPayments);
                }
                else if (row[0].Equals("SFA non-Levy additional payments budget", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.SfaNonLevyAdditionalPayments);
                }
                else if (row[0].Equals("Refund taken by SFA", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseNonEmployerRow(row, periodNames, breakdown.RefundTakenBySfa);
                }
                else if (row[0].Equals("Levy account credited", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseEmployerRow(Defaults.EmployerAccountId.ToString(), row, periodNames, breakdown.EmployersLevyAccountCredited);
                }
                else if (row[0].Equals("Refund due to employer", StringComparison.InvariantCultureIgnoreCase))
                {
                    ParseEmployerRow(Defaults.EmployerAccountId.ToString(), row, periodNames, breakdown.RefundDueToEmployer);
                }
                else if ((match = Regex.Match(row[0], "Refund due to employer ([0-9]{1,})", RegexOptions.IgnoreCase)).Success)
                {
                    ParseEmployerRow(match.Groups[1].Value, row, periodNames, breakdown.RefundDueToEmployer);
                }
                else
                {
                    throw new ArgumentException($"Unexpected earning and payments row type of '{row[0]}'");
                }
            }
        }
        private static void ParseNonEmployerRow(TableRow row, string[] periodNames, List<PeriodValue> contextList)
        {
            ParseRowValues(row, periodNames, contextList, (periodName, value) => new PeriodValue
            {
                PeriodName = periodName,
                Value = value
            });
        }
        private static void ParseEmployerRow(string rowAccountId, TableRow row, string[] periodNames, List<EmployerAccountPeriodValue> contextList)
        {
            int employerAccountId;
            if (!int.TryParse(rowAccountId, out employerAccountId))
            {
                throw new ArgumentException($"Employer id '{rowAccountId}' is not valid (Parsing row {row[0]})");
            }

            ParseRowValues(row, periodNames, contextList, (periodName, value) => new EmployerAccountPeriodValue
            {
                EmployerAccountId = employerAccountId,
                PeriodName = periodName,
                Value = value
            });
        }

        private static void ParseEmployerUlnRow(string rowAccountId, string rowUlnId, TableRow row, string[] periodNames, List<EmployerAccountUlnPeriodValue> contextList)
        {
            int employerAccountId;
            if (!int.TryParse(rowAccountId, out employerAccountId))
            {
                throw new ArgumentException($"Employer id '{rowAccountId}' is not valid (Parsing row {row[0]})");
            }

            long uln;
            if (!long.TryParse(rowUlnId, out uln))
            {
                throw new ArgumentException($"Uln '{rowUlnId}' is not valid (Parsing row {row[0]})");
            }
            ParseRowValues(row, periodNames, contextList, (periodName, value) => new EmployerAccountUlnPeriodValue
            {
                EmployerAccountId = employerAccountId,
                Uln = uln,
                PeriodName = periodName,
                Value = value
            });
        }

        private static void ParseUlnRow(string rowUlnId, TableRow row, string[] periodNames, List<UlnPeriodValue> contextList)
        {
            long uln;
            if (!long.TryParse(rowUlnId, out uln))
            {
                throw new ArgumentException($"Uln '{rowUlnId}' is not valid (Parsing row {row[0]})");
            }

            ParseRowValues(row, periodNames, contextList, (periodName, value) => new UlnPeriodValue
            {
                Uln = uln,
                PeriodName = periodName,
                Value = value
            });
        }

        private static void ParseRowValues<T>(TableRow row, string[] periodNames, List<T> contextList, Func<string, decimal, T> valueCreator)
        {
            for (var i = 1; i < periodNames.Length; i++)
            {
                var periodName = periodNames[i];
                if (string.IsNullOrEmpty(periodName))
                {
                    continue;
                }

                decimal value;
                if (!decimal.TryParse(row[i], out value))
                {
                    throw new ArgumentException($"Value '{row[i]}' is not a valid enter in the earning and payments table for {row[0]} in period {periodName}");
                }

                contextList.Add(valueCreator(periodName, value));
            }
        }
    }
}
