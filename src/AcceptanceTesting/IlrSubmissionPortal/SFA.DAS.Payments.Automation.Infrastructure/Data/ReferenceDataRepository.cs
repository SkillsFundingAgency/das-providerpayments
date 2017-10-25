using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private const string GetNextUlnProc = "Learners.GetNextUln @LearnRefNumber, @ScenarioName, @Ukprn";

        private readonly string _connectionString;

        public ReferenceDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public long GetNextUkprn()
        {
            throw new NotImplementedException();
        }

        public long GetNextUln(string scenarioName, string learnRefNumber,long ukprn)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<long>(GetNextUlnProc,new {learnRefNumber,scenarioName,ukprn}).SingleOrDefault();
            }
        }

        

    }
}
