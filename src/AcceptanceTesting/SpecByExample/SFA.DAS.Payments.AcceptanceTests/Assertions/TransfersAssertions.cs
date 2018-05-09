using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.ExecutionManagers;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions
{
    public static class TransfersAssertions
    {
        public static void ValidateTransfers(TransfersBreakdown breakdown, List<TransferResult> transferResults)
        {
            foreach (var line in breakdown.TransferLines)
            {
                foreach (var periodValue in line.TransferAmounts)
                {
                    var currentResultAmount = transferResults.FirstOrDefault(
                                                  x => PeriodNameHelper.GetStringDateFromLongPeriod(x.CollectionPeriodName) == periodValue.PeriodName
                                                       && x.SendingAccountId == breakdown.SendingEmployerAccountId
                                                       && x.ReceivingAccountId == line.ReceivingEmployerAccountId
                                              )?.Amount ?? 0;
                    if (currentResultAmount != periodValue.Value)
                    {
                        throw new Exception($"Expected transfer from sending employer {breakdown.SendingEmployerAccountId}" +
                                            $" to receiving employer {line.ReceivingEmployerAccountId}" +
                                            $" for {periodValue.PeriodName} of {periodValue.Value}. " +
                                            $"Actual transfer for this period was {currentResultAmount}.");
                    }
                }
            }
        }
    }
}