using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsEarningRepository : DcfsRepository, IEarningRepository
    {
        private const string SelectQuery = @"SELECT    CommitmentId, 
                                                       CommitmentVersionId, 
                                                       AccountId, 
                                                       AccountVersionId, 
                                                       Ukprn, 
                                                       Uln, 
                                                       LearnRefNumber [LearnerRefNumber], 
                                                       AimSeqNumber [AimSequenceNumber], 
                                                       Period, 
                                                       StandardCode, 
                                                       ProgrammeType, 
                                                       FrameworkCode, 
                                                       PathwayCode, 
                                                       ApprenticeshipContractType, 
                                                       PriceEpisodeIdentifier, 
                                                       PriceEpisodeFundLineType, 
                                                       PriceEpisodeSfaContribPct, 
                                                       PriceEpisodeLevyNonPayInd, 
                                                       PriceEpisodeEndDate, 
                                                       TransactionType, 
                                                       EarningAmount [Amount],
                                                       IsSuccess,
                                                       Payable,
                                                       LearnAimRef,
                                                       LearningStartDate
                                             FROM PaymentsDue.vw_ApprenticeshipEarning
                                             WHERE Ukprn = @ukprn";

        public DcfsEarningRepository(string connectionString)
            : base(connectionString)
        {
        }

        public EarningEntity[] GetProviderEarnings(long ukprn)
        {
            return Query<EarningEntity>(SelectQuery, new { ukprn });
        }
    }
}