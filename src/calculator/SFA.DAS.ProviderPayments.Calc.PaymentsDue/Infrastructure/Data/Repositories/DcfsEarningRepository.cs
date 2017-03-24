﻿using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsEarningRepository : DcfsRepository, IEarningRepository
    {
        private const string EarningSource = "PaymentsDue.vw_ApprenticeshipEarning";

        private const string EarningColumns = "CommitmentId, "
                                              + "CommitmentVersionId, "
                                              + "AccountId, "
                                              + "AccountVersionId, "
                                              + "Ukprn, "
                                              + "Uln, "
                                              + "LearnRefNumber [LearnerRefNumber], "
                                              + "AimSeqNumber [AimSequenceNumber], "
                                              + "Period, "
                                              + "StandardCode, "
                                              + "ProgrammeType, "
                                              + "FrameworkCode, "
                                              + "PathwayCode, "
                                              + "ApprenticeshipContractType, "
                                              + "PriceEpisodeIdentifier, "
                                              + "PriceEpisodeFundLineType, "
                                              + "PriceEpisodeSfaContribPct, "
                                              + "PriceEpisodeLevyNonPayInd, "
                                              + "PriceEpisodeEndDate, "
                                              + "TransactionType, "
                                              + "EarningAmount [Amount],"
                                              + "IsSuccess,"
                                              + "Payable";
        private const string SelectEarnings = "SELECT " + EarningColumns + " FROM " + EarningSource;
        private const string SelectProviderEarnings = SelectEarnings + " WHERE Ukprn = @Ukprn";

        public DcfsEarningRepository(string connectionString)
            : base(connectionString)
        {
        }

        public EarningEntity[] GetProviderEarnings(long ukprn)
        {
            return Query<EarningEntity>(SelectProviderEarnings, new { ukprn });
        }
    }
}