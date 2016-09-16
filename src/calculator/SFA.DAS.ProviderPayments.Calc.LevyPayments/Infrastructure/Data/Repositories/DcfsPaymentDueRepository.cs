﻿using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsPaymentDueRepository : DcfsRepository, IPaymentDueRepository
    {
        private const string DuePaymentsSource = "PaymentsDue.RequiredPayments";

        private const string DuePaymentsColumns = "CommitmentId," +
                                                       "LearnRefNumber," +
                                                       "AimSeqNumber," +
                                                       "Ukprn," +
                                                       "DeliveryMonth," +
                                                       "DeliveryYear," +
                                                       "TransactionType," +
                                                       "AmountDue";
        private const string SelectDuePayments = "SELECT " + DuePaymentsColumns + " FROM " + DuePaymentsSource;
        private const string SelectDuePaymentsForCommitment = SelectDuePayments + " WHERE CommitmentId = @CommitmentId";

        public DcfsPaymentDueRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentDueEntity[] GetPaymentsDueForCommitment(string commitmentId)
        {
            return Query<PaymentDueEntity>(SelectDuePaymentsForCommitment, new {commitmentId});
        }
    }
}