using System;
using System.Net;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommandRequest, Unit>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger _logger;

        public ProcessPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public Unit Handle(ProcessPaymentCommandRequest message)
        {
            try
            {
                _paymentRepository.AddPayment(new Infrastructure.Data.Entities.PaymentEntity
                {
                    RequiredPaymentId = message.Payment.RequiredPaymentId,
                    DeliveryMonth = message.Payment.DeliveryMonth,
                    DeliveryYear = message.Payment.DeliveryYear,
                    CollectionPeriodName = message.Payment.CollectionPeriodName,
                    CollectionPeriodMonth = message.Payment.CollectionPeriodMonth,
                    CollectionPeriodYear = message.Payment.CollectionPeriodYear,
                    FundingSource = (int) message.Payment.FundingSource,
                    TransactionType = (int) message.Payment.TransactionType,
                    Amount = message.Payment.Amount
                });
            }
            catch (Exception e)
            {
                _logger.Error($"Error writing payment for RequiredPaymentId: {message.Payment.RequiredPaymentId}, Amount: {message.Payment.Amount} exception: {e.Message}");

                if (Dns.GetHostName()?.Equals("DCS-SI-PROC-01") ?? false)
                {
                    // Running on shadow - continue
                }
                else
                {
                    // Stop everything!
                    throw;
                }
            }

            return Unit.Value;
        }
    }
}