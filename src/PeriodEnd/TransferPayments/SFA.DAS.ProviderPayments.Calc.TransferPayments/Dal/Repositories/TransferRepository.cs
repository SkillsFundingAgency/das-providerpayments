using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories
{
    class TransferRepository : DcfsRepository, IAmATransferRepository
    {
        public TransferRepository(string connectionString) : base(connectionString)
        {
        }

        public void SaveTransfers(List<TransferPaymentSet> transfers)
        {
            ExecuteBatch(transfers.SelectMany(x => x.TransferPayments).ToArray(), "RequiredTransferPayments.Payments");
            ExecuteBatch(transfers.SelectMany(x => x.AccountTransfers).ToArray(), "TransferPayments.AccountTransfers");
        }

        public IEnumerable<RequiredTransferPayment> RequiredTransferPayments()
        {
            var command = "SELECT RequiredPaymentId, " +
                          "AccountId, " +
                          "AccountVersionId, " +
                          "Uln, " +
                          "LearnRefNumber, " +
                          "AimSeqNumber, " +
                          "Ukprn, " +
                          "PriceEpisodeIdentifier, " +
                          "CommitmentId, " +
                          "CommitmentVersionId, " +
                          "StandardCode, " +
                          "ProgrammeType, " +
                          "FrameworkCode, " +
                          "PathwayCode, " +
                          "ApprenticeshipContractType, " +
                          "DeliveryMonth, " +
                          "DeliveryYear, " +
                          "TransactionType, " +
                          "AmountDue, " +
                          "SfaContributionPercentage, " +
                          "FundingLineType, " +
                          "UseLevyBalance, " +
                          "LearnAimRef, " +
                          "LearningStartDate, " +
                          "TransferSendingEmployerAccountId, " +
                          "TransferApprovedDate, " +
                          "CollectionPeriodName, " +
                          "CollectionPeriodMonth, " +
                          "CollectionPeriodYear " +
                          "FROM " +
                          "RequiredTransferPayments.vw_RequiredTransferPayment";
            var results = Query<RequiredTransferPayment>(command);
            return results;
        }
    }
}
