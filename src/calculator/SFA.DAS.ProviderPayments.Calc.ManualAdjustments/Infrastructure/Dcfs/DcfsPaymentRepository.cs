﻿using System;
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
            return Query<PaymentEntity>($"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount,CommitmentId FROM {HistoryLevySource} WHERE RequiredPaymentId = @requiredPaymentId " +
                                        $"UNION ALL " +
                                        $"SELECT RequiredPaymentId,DeliveryMonth,DeliveryYear,TransactionType,FundingSource,Amount, NULL as CommitmentId FROM {HistoryCoInvestedSource} WHERE RequiredPaymentId = @requiredPaymentId ",
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

        private void CreateCoInvestmentHistoryPayment(PaymentEntity payment, string destinationTable)
        {

            Execute($"INSERT INTO {destinationTable} (RequiredPaymentId,DeliveryMonth,DeliveryYear,FundingSource,TransactionType,Amount) " +
                     "VALUES (@RequiredPaymentId,@DeliveryMonth,@DeliveryYear,@FundingSource,@TransactionType,@Amount)", payment);
        }

        public void CreatePayment(PaymentEntity payment)
        {

            var currentDestinationTable = payment.FundingSource == 1 ? CurrentLevySource : CurrentCoInvestedSource;

            CreatePayment(payment, currentDestinationTable);

            if (payment.FundingSource == 1)
                CreateLevyHistoryPayment(payment, HistoryLevySource);
            else
                CreateCoInvestmentHistoryPayment(payment, HistoryCoInvestedSource);
        }


    }
}
