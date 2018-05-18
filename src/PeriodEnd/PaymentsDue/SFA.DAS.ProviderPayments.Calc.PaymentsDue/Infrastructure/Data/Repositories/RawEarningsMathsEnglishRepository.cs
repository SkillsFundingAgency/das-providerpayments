﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class RawEarningsMathsEnglishRepository : DcfsRepository
    {
        public RawEarningsMathsEnglishRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RawEarningMathsEnglishEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Staging.RawEarningsMathsEnglish
            WHERE Ukprn = @ukprn";

            var result = Query<RawEarningMathsEnglishEntity>(sql, new {ukprn})
                .ToList();

            return result;
        }
    }
}