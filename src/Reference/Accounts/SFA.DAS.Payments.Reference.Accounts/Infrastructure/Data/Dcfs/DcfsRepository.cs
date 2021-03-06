﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public abstract class DcfsRepository
    {
        private readonly string _connectionString;

        protected DcfsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IEnumerable<T> Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected T QuerySingle<T>(string command, object param = null)
        {
            var resultset = Query<T>(command, param);
            return resultset == null ? default(T) : resultset.SingleOrDefault();
        }

        protected void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
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
