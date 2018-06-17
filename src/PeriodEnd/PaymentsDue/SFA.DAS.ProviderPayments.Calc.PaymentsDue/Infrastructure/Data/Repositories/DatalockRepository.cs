using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IDatalockRepository
    {
        List<DatalockOutput> GetDatalockOutputForProvider(long ukprn, DateTime firstDayOfNextAcademicYear);
        List<DatalockValidationError> GetValidationErrorsForProvider(long ukprn);
    }

    public class DatalockRepository: DcfsRepository, IDatalockRepository
    {
        public DatalockRepository(string connectionString) : base(connectionString)
        {
        }

        public List<DatalockOutput> GetDatalockOutputForProvider(long ukprn, DateTime firstDayOfNextAcademicYear)
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
            WHERE PM.Ukprn = @ukprn
			AND CONVERT(date, SUBSTRING(PM.PriceEpisodeIdentifier, LEN(PM.PriceEpisodeIdentifier) - 9, 10), 103) < @firstDayOfNextAcademicYear";

            var result = Query<DatalockOutput>(sql, new { ukprn, firstDayOfNextAcademicYear })
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
    }
}