using System;
using System.Data;
using Dapper;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string PaymentsDestination = "CoInvestedPayments.Payments";
        private const string PaymentsAddStoredProcedure = "CoInvestedPayments.AddPayment";
        private const string AddPaymentCommand = PaymentsAddStoredProcedure + " @RequiredPaymentId, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @FundingSource, @TransactionType, @Amount, @CollectionPeriodName";

        private const string CoInvestedPaymentHistory = "Reference.CoInvestedPaymentsHistory";

        private const string PaymentsHistoryColumns = "RequiredPaymentId," +
                                                "DeliveryMonth," +
                                                "DeliveryYear," +
                                                "TransactionType," +
                                                "Amount," +
                                                "FundingSource";
                                               

        private const string FilterPaymentsHistory = " WHERE DeliveryMonth = @deliveryMonth AND " + 
                                                    " DeliveryYear = @deliveryYear AND " + 
                                                    "TransactionType = @transactionType AND " +
                                                     "Uln = @uln AND " +
                                                    "Ukprn = @Ukprn  AND " +
                                                    "AimSeqNumber = @aimSequenceNumber AND " +
                                                    "(StandardCode Is Null Or StandardCode = @standardCode) AND " +
                                                    "(ProgrammeType  Is Null Or ProgrammeType = @programmeType)  AND " +
                                                    "(FrameworkCode Is Null OR FrameworkCode = @frameworkCode) AND " +
                                                    "(PathwayCode Is Null Or PathwayCode = @pathwayCode)";

        private const string SelectPaymentsHistory = "SELECT " + PaymentsHistoryColumns + " FROM " + CoInvestedPaymentHistory + FilterPaymentsHistory;

        public PaymentRepository(string connectionString) 
            : base(connectionString)
        {
        }
        public void AddPayments(PaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }

        public void AddPayment(PaymentEntity payment)
        {
            Execute(AddPaymentCommand, new
            {
                RequiredPaymentId = payment.RequiredPaymentId,
                DeliveryMonth = payment.DeliveryMonth,
                DeliveryYear = payment.DeliveryYear,
                CollectionPeriodMonth = payment.CollectionPeriodMonth,
                CollectionPeriodYear = payment.CollectionPeriodYear,
                FundingSource = payment.FundingSource,
                TransactionType = payment.TransactionType,
                Amount = payment.Amount,
                CollectionPeriodName = payment.CollectionPeriodName
            });
        }

        public PaymentHistoryEntity[] GetCoInvestedPaymentsHistory(int deliveryMonth, int deliveryYear, int transactionType, int aimSequenceNumber, long ukprn, long uln, int? frameworkCode, int? pathwayCode, int? programmeType, long? standardCode, string learnRefNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("deliveryMonth", deliveryMonth, DbType.Int32);
            parameters.Add("deliveryYear", deliveryYear, DbType.Int32);
            parameters.Add("transactionType", transactionType, DbType.Int32);
            parameters.Add("ukprn", ukprn, DbType.Int64);
            parameters.Add("learnRefNumber", learnRefNumber, DbType.String, ParameterDirection.Input, 12);
            parameters.Add("aimSeqNumber", aimSequenceNumber, DbType.Int32);

            if (standardCode.HasValue)
            {
                parameters.Add("standardCode", standardCode.Value, DbType.Int64);
            }

            if (programmeType.HasValue)
            {
                parameters.Add("programmeType", programmeType.Value, DbType.Int32);
            }

            if (frameworkCode.HasValue)
            {
                parameters.Add("frameworkCode", frameworkCode.Value, DbType.Int64);
            }

            if (pathwayCode.HasValue)
            {
                parameters.Add("pathwayCode", pathwayCode.Value, DbType.Int32);
            }

            return QueryByProc<PaymentHistoryEntity>("CoInvestedPayments.GetPaymentHistory", parameters);
        }
    }
}
