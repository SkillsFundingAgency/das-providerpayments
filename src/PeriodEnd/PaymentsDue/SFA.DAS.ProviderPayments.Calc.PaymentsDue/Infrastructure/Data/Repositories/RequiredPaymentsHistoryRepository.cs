using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IRequiredPaymentsHistoryRepository
    {
        List<RequiredPaymentEntity> GetAllForProvider(long ukprn);
    }

    public class RequiredPaymentsHistoryRepository : DcfsRepository, IRequiredPaymentsHistoryRepository
    {
        public RequiredPaymentsHistoryRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RequiredPaymentEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT 
                [Id]
              ,[CommitmentId]
              ,[CommitmentVersionId]
              ,[AccountId]
              ,[AccountVersionId]
              ,[Uln]
              ,[LearnRefNumber]
              ,[AimSeqNumber]
              ,[Ukprn]
              ,[IlrSubmissionDateTime]
              ,[PriceEpisodeIdentifier]
              ,COALESCE([StandardCode], 0) [StandardCode]
              ,COALESCE([ProgrammeType], 0) [ProgrammeType]
              ,COALESCE([FrameworkCode], 0) [FrameworkCode]
              ,COALESCE([PathwayCode], 0) [PathwayCode]
              ,[ApprenticeshipContractType]
              ,[DeliveryMonth]
              ,[DeliveryYear]
              ,[CollectionPeriodName]
              ,[CollectionPeriodMonth]
              ,[CollectionPeriodYear]
              ,[TransactionType]
              ,[AmountDue]
              ,[SfaContributionPercentage]
              ,[FundingLineType]
              ,[UseLevyBalance]
              ,[LearnAimRef]
              ,[LearningStartDate]
            FROM Reference.RequiredPaymentsHistory
            WHERE Ukprn = @ukprn";

            var result = Query<RequiredPaymentEntity>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}
