﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories
{
    public interface IHistoricalPaymentsRepository
    {
        List<HistoricalPaymentEntity> GetAllForProvider(long ukprn);
    }

    public class HistoricalPaymentsRepository : DcfsRepository, IHistoricalPaymentsRepository
    {
        public HistoricalPaymentsRepository(string transientConnectionString) 
            : base(transientConnectionString) { }

        public List<HistoricalPaymentEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn
            ";

            var result = Query<HistoricalPaymentEntity>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}