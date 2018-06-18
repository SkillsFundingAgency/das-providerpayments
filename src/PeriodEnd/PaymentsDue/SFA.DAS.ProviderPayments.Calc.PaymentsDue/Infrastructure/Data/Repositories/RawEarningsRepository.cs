using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IRawEarningsRepository
    {
        List<RawEarning> GetAllForProvider(long ukprn, DateTime firstDayOfNextAcademicYear);
    }

    public class RawEarningsRepository : DcfsRepository, IRawEarningsRepository
    {
        public RawEarningsRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RawEarning> GetAllForProvider(long ukprn, DateTime firstDayOfNextAcademicYear)
        {
            const string sql = @"
            SELECT *
            FROM Staging.RawEarnings
            WHERE Ukprn = @ukprn
            AND CONVERT(date, SUBSTRING(PriceEpisodeIdentifier, LEN(PriceEpisodeIdentifier) - 9, 10), 103) < @firstDayOfNextAcademicYear
            AND CONVERT(date, SUBSTRING(PriceEpisodeIdentifier, LEN(PriceEpisodeIdentifier) - 9, 10), 103) >= @firstDayOfThisAcademicYear
";

            var firstDayOfThisAcademicYear = firstDayOfNextAcademicYear.AddYears(-1);
            var result = Query<RawEarning>(sql, new {ukprn, firstDayOfNextAcademicYear, firstDayOfThisAcademicYear})
                .ToList();

            return result;
        }
    }
}