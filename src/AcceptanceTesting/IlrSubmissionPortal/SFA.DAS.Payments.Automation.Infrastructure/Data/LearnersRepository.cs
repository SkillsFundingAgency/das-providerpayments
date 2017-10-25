using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using SFA.DAS.Payments.Automation.Infrastructure.Entities;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public class LearnersRepository : ILearnersRepository
    {
        private const string GetAllUsedULnsQuery = "SELECT uln,Ukprn,ScenarioName,LearnrefNumber From Learners.ULNs Where Used = 1 Order by UKPRN,ULN";

        private readonly string _connectionString;

        public LearnersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<UsedUlnEntity> GetAllUsedUlns()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<UsedUlnEntity>(GetAllUsedULnsQuery).ToList();
            }
        }
    }
}
