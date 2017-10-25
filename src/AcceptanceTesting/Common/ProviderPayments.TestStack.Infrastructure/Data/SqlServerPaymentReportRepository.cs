using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerPaymentReportRepository : SqlServerRepository, IPaymentReportRepository
    {
        public SqlServerPaymentReportRepository()
            : base("TransientConnectionString")
        {
        }

        public async Task<IEnumerable<PaymentReportEntity>> All()
        {
            return await Query<PaymentReportEntity>(@"select *
                                                        from (
	                                                        select
			                                                        rp.[CommitmentId],
                                                                    rp.[AccountId],
			                                                        c.[Priority],
			                                                        rp.[Ukprn],
			                                                        rp.[Uln],
			                                                        cp.[DeliveryMonth],
			                                                        cp.[DeliveryYear],
			                                                        cp.[CollectionPeriodName],
			                                                        cp.[TransactionType],
			                                                        cp.[FundingSource],
			                                                        cp.[Amount],
			                                                        l.[LearnerName],
			                                                        pr.[ProviderName]
		                                                        from [CoInvestedPayments].[Payments] cp
			                                                        join [PaymentsDue].[RequiredPayments] rp on cp.[RequiredPaymentId] = rp.[Id]
			                                                        join [Reference].[DasCommitments] c on rp.[CommitmentId] = c.[CommitmentId]
			                                                        join [TestStack].[Learner] l on rp.[Uln] = l.[Uln]
                                                                    join [TestStack].[Provider] pr on rp.[Ukprn] = pr.[Ukprn]
	                                                        union
	                                                        select
			                                                        rp.[CommitmentId],
                                                                    rp.[AccountId],
			                                                        c.[Priority],
			                                                        rp.[Ukprn],
			                                                        rp.[Uln],
			                                                        lp.[DeliveryMonth],
			                                                        lp.[DeliveryYear],
			                                                        lp.[CollectionPeriodName],
			                                                        lp.[TransactionType],
			                                                        lp.[FundingSource],
			                                                        lp.[Amount],
			                                                        l.[LearnerName],
			                                                        pr.[ProviderName]
		                                                        from [LevyPayments].[Payments] lp
			                                                        join [PaymentsDue].[RequiredPayments] rp on lp.[RequiredPaymentId] = rp.[Id]
			                                                        join [Reference].[DasCommitments] c on rp.[CommitmentId] = c.[CommitmentId]
			                                                        join [TestStack].[Learner] l on rp.[Uln] = l.[Uln]
                                                                    join [TestStack].[Provider] pr on rp.[Ukprn] = pr.[Ukprn]
                                                        ) x
                                                        order by x.[Priority], x.[TransactionType], x.[FundingSource]");
        }
    }
}