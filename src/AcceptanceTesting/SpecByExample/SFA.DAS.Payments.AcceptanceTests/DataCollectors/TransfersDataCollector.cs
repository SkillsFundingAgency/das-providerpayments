using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.DataCollectors
{
    public static class TransfersDataCollector
    {
        private const string ReadTransfersFromDedsQuery =
            @"  SELECT
                                    [SendingAccountId],
                                    [ReceivingAccountId],
	                                [CollectionPeriodName],
	                                [Amount]
                                FROM
	                                [TransferPayments].[AccountTransfers]";

        public static List<TransferResult> CollectAllTransfers()
        {
            return ReadTransfersFromDeds();
        }

        private static List<TransferResult> ReadTransfersFromDeds()
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                return connection.Query<TransferResult>(ReadTransfersFromDedsQuery).ToList();
            }
        }
    }
}