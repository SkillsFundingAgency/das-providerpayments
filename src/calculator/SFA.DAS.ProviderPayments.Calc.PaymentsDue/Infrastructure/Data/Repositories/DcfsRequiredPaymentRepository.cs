﻿using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentsDue.RequiredPayments";

        private const string PaymentHistorySource = "PaymentsDue.vw_PaymentHistory";
        private const string PaymentHistoryColumns = "CommitmentId, "
                                                    + "CommitmentVersionId,"
                                                   + "AccountId,"
                                                   + "AccountVersionId,"
                                                   + "LearnRefNumber, "
                                                   + "AimSeqNumber, "
                                                   + "Ukprn, "
                                                   + "DeliveryMonth, "
                                                   + "DeliveryYear, "
                                                   + "AmountDue, "
                                                   + "TransactionType,"
                                                   + "Uln,"
                                                   + "StandardCode ,"
                                                   + "ProgrammeType,"
                                                   + "FrameworkCode,"
                                                   + "PathwayCode,"
                                                   + "LearnAimRef,"
                                                   + "LearningStartDate";

        private const string SelectPayments = "SELECT " + PaymentHistoryColumns + " FROM " + PaymentHistorySource;
        private const string SelectProviderPayments = SelectPayments + " WHERE Ukprn = @ukprn";
        private const string SelectLearnerPayments = SelectProviderPayments + " AND LearnRefNumber = @LearnRefNumber";

        public DcfsRequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddRequiredPayments(RequiredPaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }

        public RequiredPaymentEntity[] GetPreviousPayments(long ukprn, string learnRefNumber)
        {
            return Query<RequiredPaymentEntity>(SelectLearnerPayments, new { ukprn, learnRefNumber });
        }
    }
}