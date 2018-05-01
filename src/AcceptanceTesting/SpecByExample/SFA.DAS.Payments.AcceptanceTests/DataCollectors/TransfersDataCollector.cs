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
	                                [CollectionPeriodName],
	                                [Amount]
                                FROM
	                                [TransferPayments].[AccountTransfers]
                                WHERE
                                    [ReceivingAccountId] = @receivingEmployerAccountId";

        public static TransferResults CollectForEmployer(int receivingEmployerAccountId)
        {
            return new TransferResults
            {
                ReceivingEmployerAccountId = receivingEmployerAccountId,
                Values = ReadTransfersFromDeds(receivingEmployerAccountId)
            };
        }

        private static List<TransferPeriodValueEntity> ReadTransfersFromDeds(int receivingEmployerAccountId)
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                return connection.Query<TransferPeriodValueEntity>(ReadTransfersFromDedsQuery, new { receivingEmployerAccountId }).ToList();
            }
        }
    }
}