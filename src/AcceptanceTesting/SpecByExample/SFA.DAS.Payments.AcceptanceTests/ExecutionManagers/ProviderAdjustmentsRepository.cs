using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FastMember;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels.ProviderAdjustments;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    public class ProviderAdjustmentsRepository
    {
        public static void SavePreviousEasPayments(List<EasPayment> payment)
        {
            
        }

        public static void SaveSubmittedEas(EasSubmission submission)
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.Open();
                using (var sqlBulkCopy = new SqlBulkCopy(connection){DestinationTableName = "[dbo].[EAS_Submission_Values]"})
                using (var reader = ObjectReader.Create(submission.Values, "SubmissionId", "CollectionPeriod", "PaymentId", 
                    "PaymentValue"))
                {
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SubmissionId", "Submission_Id"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CollectionPeriod", "CollectionPeriod"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PaymentId", "Payment_Id"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PaymentValue", "PaymentValue"));

                    sqlBulkCopy.WriteToServer(reader);
                }

                using (var sqlBulkCopy = new SqlBulkCopy(connection) { DestinationTableName = "[dbo].[EAS_Submission]" })
                using (var reader = ObjectReader.Create(new List<EasSubmission>{submission}, "SubmissionId", "Ukprn", 
                    "CollectionPeriod", "ProviderName", "UpdatedOn", "DeclarationChecked", "NilReturn", "UpdatedBy"))
                {
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SubmissionId", "Submission_Id"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Ukprn", "UKPRN"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CollectionPeriod", "CollectionPeriod"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProviderName", "ProviderName"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UpdatedOn", "UpdatedOn"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DeclarationChecked", "DeclarationChecked"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NilReturn", "NilReturn"));
                    sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UpdatedBy", "UpdatedBy"));
                    
                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }

        public static List<EasPayment> GetEasPaymentsFor(int month, int year)
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                var collectionPeriods = connection
                    .Query<EasPayment>("SELECT * FROM [ProviderAdjustments].[Payments] " +
                                        "WHERE CollectionPeriodMonth = @month AND CollectionPeriodYear = @year",
                                        new {month, year});
                return collectionPeriods.ToList();
            }
        }

        public static List<EasPayment> GetEasPayments()
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                var collectionPeriods = connection
                    .Query<EasPayment>("SELECT * FROM [ProviderAdjustments].[Payments]");
                return collectionPeriods.ToList();
            }
        }

        public static List<CollectionPeriodMapping> GetPeriodMappings()
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                var collectionPeriods = connection.Query<CollectionPeriodMapping>("SELECT * FROM dbo.Collection_Period_Mapping");
                return collectionPeriods.ToList();
            }
        }

        public static List<PaymentType> GetPaymentTypes()
        {
            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                var collectionPeriods = connection.Query<PaymentType>("SELECT * FROM dbo.Payment_Types WHERE FM36 = 1");
                return collectionPeriods.ToList();
            }
        }
    }
}
