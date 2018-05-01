using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.DataCollectors;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions
{
    public static class TransfersAssertions
    {
        public static void ValidateTransfers(TransfersBreakdown breakdown)
        {
            foreach (var line in breakdown.TransferLines)
            {
                var results = TransfersDataCollector.CollectForEmployer(line.ReceivingEmployerAccountId);
                foreach (var periodValue in line.TransferAmounts)
                {
                    var currentResultAmount = results.Values.FirstOrDefault(x => x.CollectionPeriodName == periodValue.PeriodName) != null
                        ? results.Values.FirstOrDefault(x => x.CollectionPeriodName == periodValue.PeriodName).Amount : 0;
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