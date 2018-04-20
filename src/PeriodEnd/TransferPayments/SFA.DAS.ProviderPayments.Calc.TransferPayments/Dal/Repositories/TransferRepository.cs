using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories
{
    class TransferRepository : DcfsRepository
    {
        public TransferRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddAccountLevyTransfers(IEnumerable<AccountLevyTransfer> accountLevyTransfers)
        {
            ExecuteBatch(accountLevyTransfers.ToArray(), "RequiredTransferPayments.Payments");
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
                          "TransferSendingEmployerAccountId " +
                          "FROM " +
                          "RequiredTransferPayments.vw_TransfersLearners";
            var results = Query<RequiredTransferPayment>(command);
            return results;
        }
    }
}
