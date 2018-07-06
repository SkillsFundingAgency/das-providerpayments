using System.Data.SqlClient;
using Dapper;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.DataHelpers
{
    internal static class TestDataHelper
    {
        internal static void Execute(string command, object param = null, bool inTransient = true)
        {
            var connectionString = inTransient
                ? GlobalTestContext.Instance.TransientConnectionString
                : GlobalTestContext.Instance.DedsConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}