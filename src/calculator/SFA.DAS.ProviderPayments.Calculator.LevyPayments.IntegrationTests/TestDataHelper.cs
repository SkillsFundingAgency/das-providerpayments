using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests
{
    internal static class TestDataHelper
    {
        internal static void AddAccount(string id, string name = null, decimal balance = 999999999)
        {
            if (name == null)
            {
                name = id;
            }

            Execute("INSERT INTO Reference.DasAccounts (AccountId, AccountName, LevyBalance) VALUES (@id, @name, @balance)", new { id, name, balance });
        }
        internal static void AddCommitment(string id, 
                                           string accountId, 
                                           long uln = 0l, 
                                           long ukprn = 0l, 
                                           DateTime startDate = default(DateTime), 
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null)
        {
            var minStartDate = new DateTime(2017, 4, 1);

            if (!standardCode.HasValue && !programmeType.HasValue)
            {
                standardCode = 123456;
            }
            if (startDate < minStartDate)
            {
                startDate = minStartDate;
            }
            if (endDate < startDate)
            {
                endDate = startDate.AddYears(1);
            }

            Execute("INSERT INTO Reference.DataLockCommitments " +
                    "(CommitmentId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode) " +
                    "VALUES " +
                    "(@id, @accountId, @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode)",
                    new { id, accountId, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode });
        }
        internal static void AddEarningForCommitment(string commitmentId, 
                                                     string learnerRefNumber = null, 
                                                     int aimSequenceNumber = 1, 
                                                     string niNumber = "XX12345X",
                                                     int numberOfPeriods = 12,
                                                     int currentPeriod = 1)
        {
            if (string.IsNullOrEmpty(learnerRefNumber))
            {
                learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            }

            Execute("INSERT INTO Rulebase.AE_LearningDelivery "
                  + "SELECT "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "Ukprn, "
                  + "Uln, "
                  + "@niNumber, "
                  + "StandardCode, "
                  + "ProgrammeType, "
                  + "FrameworkCode, "
                  + "PathwayCode, "
                  + "AgreedCost, "
                  + "StartDate, "
                  + "NULL, "
                  + "EndDate, "
                  + "NULL, "
                  + "@numberOfPeriods, "
                  + "@currentPeriod, "
                  + "(AgreedCost * 0.8) / @numberOfPeriods, "
                  + "(AgreedCost * 0.8) / @numberOfPeriods, "
                  + "AgreedCost * 0.2, "
                  + "AgreedCost * 0.2 "
                  + "FROM Reference.DataLockCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber, niNumber, numberOfPeriods, currentPeriod });
        }


        internal static PaymentEntity[] GetPaymentsForCommitment(string commitmentId)
        {
            return Query<PaymentEntity>("SELECT * FROM LevyPayments.Payments");
        }

        private static void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
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
        private static T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param)?.ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
