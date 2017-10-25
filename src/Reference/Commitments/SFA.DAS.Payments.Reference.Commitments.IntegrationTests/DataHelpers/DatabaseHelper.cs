using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers
{
    internal static class DatabaseHelper
    {
        internal static T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param).ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        internal static T QuerySingle<T>(string command, object param = null)
        {
            var resultset = Query<T>(command, param);
            return resultset == null ? default(T) : resultset.SingleOrDefault();
        }
        internal static void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
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
