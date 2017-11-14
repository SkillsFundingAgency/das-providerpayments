using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
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
