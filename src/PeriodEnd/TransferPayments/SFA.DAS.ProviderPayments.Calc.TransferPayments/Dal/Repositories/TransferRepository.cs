using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories
{
    internal class TransferRepository : DcfsRepository, IAmATransferRepository
    {
        [UsedImplicitly]
        public TransferRepository(string connectionString) : base(connectionString)
        {
        }

        public void SaveTransfers(List<TransferPaymentSet> transfers)
        {
            ExecuteBatch(transfers.SelectMany(x => x.TransferPayments).ToArray(), "TransferPayments.Payments");
            ExecuteBatch(transfers.SelectMany(x => x.AccountTransfers).ToArray(), "TransferPayments.AccountTransfers");
        }

        public IEnumerable<RequiredTransferPayment> RequiredTransferPayments()
        {
            const string command = @"SELECT RequiredPaymentId,  
                          AccountId,  
                          AccountVersionId,  
                          Uln,  
                          LearnRefNumber,  
                          AimSeqNumber,  
                          Ukprn,  
                          PriceEpisodeIdentifier,  
                          CommitmentId,  
                          CommitmentVersionId,  
                          StandardCode,  
                          ProgrammeType,  
                          FrameworkCode, 
                          PathwayCode,  
                          ApprenticeshipContractType,  
                          DeliveryMonth,  
                          DeliveryYear,  
                          TransactionType,  
                          AmountDue,  
                          SfaContributionPercentage,  
                          FundingLineType,  
                          UseLevyBalance,  
                          LearnAimRef,  
                          LearningStartDate,  
                          TransferSendingEmployerAccountId,  
                          TransferApprovalDate 
                          FROM  
                          TransferPayments.vw_RequiredTransferPayment;";

            var results = Query<RequiredTransferPayment>(command);
            return results;
        }
    }
}
