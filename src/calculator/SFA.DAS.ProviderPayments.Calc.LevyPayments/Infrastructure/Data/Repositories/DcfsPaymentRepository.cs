using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string AddPaymentCommand = "LevyPayments.AddPayment @RequiredPaymentId, @DeliveryMonth, @DeliveryYear, @CollectionPeriodMonth, @CollectionPeriodYear, @FundingSource, @TransactionType, @Amount, @CollectionPeriodName";

        private const string LevyPaymentsHistorySource = "Reference.LevyPaymentsHistory";

        private const string PaymentsHistoryColumns = "RequiredPaymentId," +
                                                  "DeliveryMonth," +
                                                  "DeliveryYear," +
                                                  "TransactionType," +
                                                  "FundingSource," +
                                                  "Amount";
        private const string FilterPaymentsHistory = " WHERE DeliveryMonth = @deliveryMonth AND DeliveryYear = @deliveryYear AND TransactionType = @transactionType AND CommitmentId = @commitmentId";

        private const string SelectPaymentsHistory = "SELECT " + PaymentsHistoryColumns + " FROM " + LevyPaymentsHistorySource + FilterPaymentsHistory;

        public DcfsPaymentRepository(string connectionString) 
            : base(connectionString)
        {
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

        public PaymentHistoryEntity[] GetLevyPaymentsHistory(int deliveryMonth, 
                                                    int deliveryYear, 
                                                    int transactionType,
                                                    long commitmentId)
        {
            return Query<PaymentHistoryEntity>(SelectPaymentsHistory,new { deliveryMonth,deliveryYear,transactionType,commitmentId});
        }
    }
}
