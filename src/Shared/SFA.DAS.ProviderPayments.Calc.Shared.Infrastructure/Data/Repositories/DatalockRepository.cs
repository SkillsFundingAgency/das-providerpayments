using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public interface IDatalockRepository
    {
        List<DatalockOutputEntity> GetDatalockOutputForProvider(long ukprn);
        List<DatalockValidationError> GetValidationErrorsForProvider(long ukprn);

        void WriteValidationErrors(IEnumerable<DatalockValidationError> entities);
        void WriteValidationErrorsByPeriod(IEnumerable<DatalockValidationErrorByPeriod> entities);
        void WritePriceEpisodeMatches(IEnumerable<PriceEpisodeMatchEntity> entities);
        void WritePriceEpisodePeriodMatches(IEnumerable<PriceEpisodePeriodMatchEntity> entities);
        void WriteDatalockOutput(IEnumerable<DatalockOutputEntity> entities);
    }

    public class DatalockRepository: DcfsRepository, IDatalockRepository
    {
        public DatalockRepository(string connectionString) : base(connectionString)
        {
        }

        private const string PriceEpisodeMatchDestination = "DataLock.PriceEpisodeMatch";
        private const string PriceEpisodePeriodMatchDestination = "DataLock.PriceEpisodePeriodMatch";
        private const string ValidationErrorDestination = "DataLock.ValidationError";
        private const string ValidationErrorByPeriodDestination = "DataLock.ValidationErrorByPeriod";
        private const string DatalockOutputDestination = "DataLock.Output";

        public List<DatalockOutputEntity> GetDatalockOutputForProvider(long ukprn)
        {
            const string sql = @"
            SELECT          
				PM.Ukprn,
	            PM.PriceEpisodeIdentifier,
	            PM.LearnRefNumber,
	            PM.AimSeqNumber,
	            PM.CommitmentId,
	            PM.VersionId,
	            PM.[Period],
	            CASE WHEN M.IsSuccess = 1 AND PM.Payable = 1 THEN 1 ELSE 0 END AS Payable,
	            PM.TransactionType,
	            PM.TransactionTypesFlag
            FROM [DataLock].[PriceEpisodePeriodMatch] PM
            JOIN [DataLock].[PriceEpisodeMatch] M 
	            ON M.PriceEpisodeIdentifier = PM.PriceEpisodeIdentifier 
	            AND M.UkPrn = PM.UkPrn
                AND M.LearnRefNumber = PM.LearnRefNumber
                AND M.CommitmentId = PM.CommitmentId
            WHERE PM.Ukprn = @ukprn
			";

            var result = Query<DatalockOutputEntity>(sql, new { ukprn })
                .ToList();

            return result;
        }

        public List<DatalockValidationError> GetValidationErrorsForProvider(long ukprn)
        {
            const string sql = @"
                SELECT 
                    Ukprn, 
                    LearnRefNumber, 
                    AimSeqNumber,
                    RuleId,
                    PriceEpisodeIdentifier
                FROM Datalock.ValidationError
                WHERE Ukprn = @ukprn
                ";

            var result = Query<DatalockValidationError>(sql, new {ukprn})
                .ToList();

            return result;
        }

        public void WriteValidationErrors(IEnumerable<DatalockValidationError> entities)
        {
            ExecuteBatch(entities, ValidationErrorDestination);
        }

        public void WriteValidationErrorsByPeriod(IEnumerable<DatalockValidationErrorByPeriod> entities)
        {
            ExecuteBatch(entities, ValidationErrorByPeriodDestination);
        }

        public void WritePriceEpisodeMatches(IEnumerable<PriceEpisodeMatchEntity> entities)
        {
            ExecuteBatch(entities, PriceEpisodeMatchDestination);
        }

        public void WritePriceEpisodePeriodMatches(IEnumerable<PriceEpisodePeriodMatchEntity> entities)
        {
            ExecuteBatch(entities, PriceEpisodePeriodMatchDestination);
        }

        public void WriteDatalockOutput(IEnumerable<DatalockOutputEntity> entities)
        {
            //ExecuteBatch(entities, DatalockOutputDestination);
        }
    }
}