using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    internal static class TransfersTableParser
    {
        internal static void ParseTransfersTableIntoContext(TransfersBreakdown breakdown, Table transfers, int sendingEmployerAccountId)
        {
            if (transfers.Rows.Count < 1)
            {
                throw new ArgumentException("Transfers table must have at least 1 row");
            }

            var periodNames = EarningAndPaymentTableParser.ParseEarningAndPaymentsHeaders(transfers, "Recipient");
            ParseTransferRows(breakdown, transfers, periodNames, sendingEmployerAccountId);

        }

        private static void ParseTransferRows(TransfersBreakdown breakdown, Table transfers, string[] periodNames, int sendingEmployerAccountId)
        {
            breakdown.SendingEmployerAccountId = sendingEmployerAccountId;
            
            foreach (var row in transfers.Rows)
            {
                var transferLine = new TransferLine{ ReceivingEmployerAccountId = int.Parse(row[0].Substring(9)) };

                for (int i = 1; i < periodNames.Length; i++)
                {
                    transferLine.TransferAmounts.Add(new PeriodValue{ PeriodName = periodNames[i], Value = int.Parse(row[i]) });
                }
                breakdown.TransferLines.Add(transferLine);
            }
        }
    }
}