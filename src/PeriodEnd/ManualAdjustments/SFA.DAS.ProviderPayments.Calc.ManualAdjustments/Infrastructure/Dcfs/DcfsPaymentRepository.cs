using System;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string HistoryLevySource = "Reference.LevyPaymentsHistory";
        private const string HistoryCoInvestedSource = "Reference.CoInvestedPaymentsHistory";
        private const string CurrentLevySource = "LevyPayments.Payments";
        private const string CurrentCoInvestedSource = "CoInvestedPayments.Payments";

        public DcfsPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PaymentEntity[] GetPaymentsForRequiredPayment(string requiredPaymentId)
        {
            return Query<PaymentEntity>($"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount,CommitmentId FROM {HistoryLevySource} WHERE FundingSource = 1 AND RequiredPaymentId = @requiredPaymentId " +
                                        $" UNION ALL " +
                                        $"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount,CommitmentId FROM {HistoryCoInvestedSource} WHERE RequiredPaymentId = @requiredPaymentId ",
                new { requiredPaymentId });
        }

        private void CreatePayment(PaymentEntity payment, string destinationTable)
        {
            
            Execute($"INSERT INTO {destinationTable} (PaymentId,RequiredPaymentId,DeliveryMonth,DeliveryYear," +
                     "CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear,FundingSource,TransactionType,Amount) " +
                     "VALUES (@PaymentId,@RequiredPaymentId,@DeliveryMonth,@DeliveryYear," +
                     "@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear,@FundingSource,@TransactionType,@Amount)", payment);
        }

        private void CreateLevyHistoryPayment(PaymentEntity payment, string destinationTable)
        {

            Execute($"INSERT INTO {destinationTable} (RequiredPaymentId,DeliveryMonth,DeliveryYear,FundingSource,TransactionType,Amount,CommitmentId) " +
                     "VALUES (@RequiredPaymentId,@DeliveryMonth,@DeliveryYear,@FundingSource,@TransactionType,@Amount,@CommitmentId)", payment);
        }

        private void CreateCoInvestmentHistoryPayment(PaymentEntity payment,RequiredPaymentEntity requiredPayment, string destinationTable)
        {

            Execute($"INSERT INTO {destinationTable} (RequiredPaymentId,DeliveryMonth,DeliveryYear,FundingSource,TransactionType,Amount,ULN,Ukprn,AimSeqNumber,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,CommitmentId) " +
                     "VALUES (@RequiredPaymentId,@DeliveryMonth,@DeliveryYear,@FundingSource,@TransactionType,@Amount,@ULN,@Ukprn,@AimSeqNumber,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@CommitmentId)", 
                     new {
                         RequiredPaymentId = payment.RequiredPaymentId,
                         DeliveryMonth = payment.DeliveryMonth,
                         DeliveryYear =payment.DeliveryYear,
                         FundingSource = payment.FundingSource,
                         TransactionType =payment.TransactionType,
                         Amount = payment.Amount,
                         ULN =requiredPayment.Uln,
                         Ukprn = requiredPayment.Ukprn,
                         AimSeqNumber = requiredPayment.AimSeqNumber,
                         StandardCode =requiredPayment.StandardCode,
                         ProgrammeType =requiredPayment.ProgrammeType,
                         FrameworkCode =requiredPayment.FrameworkCode,
                         PathwayCode =requiredPayment.PathwayCode,
                         CommitmentId = requiredPayment.CommitmentId
                     } );
        }

        public void CreatePayment(PaymentEntity payment, RequiredPaymentEntity requiredPayment)
        {

            var currentDestinationTable = payment.FundingSource == 1 ? CurrentLevySource : CurrentCoInvestedSource;

            CreatePayment(payment, currentDestinationTable);

            if (requiredPayment.CommitmentId.HasValue && requiredPayment.CommitmentId > 0)
            {
                CreateLevyHistoryPayment(payment, HistoryLevySource);
            }

            if (payment.FundingSource != 1)
                CreateCoInvestmentHistoryPayment(payment, requiredPayment, HistoryCoInvestedSource);
        }


    }
}
