using System;
using System.Linq;
using System.Net;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand
{
    public class ProcessPaymentsCommandHandler : IRequestHandler<ProcessPaymentsCommandRequest, ProcessPaymentsCommandResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger _logger;

        public ProcessPaymentsCommandHandler(
            IPaymentRepository paymentRepository, 
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public ProcessPaymentsCommandResponse Handle(ProcessPaymentsCommandRequest message)
        {
            try
            {
                var paymentEntities = message.Payments
                    .Select(p => new PaymentEntity
                    {
                        RequiredPaymentId = p.RequiredPaymentId,
                        DeliveryMonth = p.DeliveryMonth,
                        DeliveryYear = p.DeliveryYear,
                        CollectionPeriodName = p.CollectionPeriodName,
                        CollectionPeriodMonth = p.CollectionPeriodMonth,
                        CollectionPeriodYear = p.CollectionPeriodYear,
                        FundingSource = (int)p.FundingSource,
                        TransactionType = (int)p.TransactionType,
                        Amount = p.Amount
                    })
                    .ToArray();

                try
                {
                    _paymentRepository.AddPayments(paymentEntities);
                }
                catch (Exception e)
                {
                    _logger.Error($"Error writing payments: {e.Message}");
                    foreach (var paymentEntity in paymentEntities)
                    {
                        if ((paymentEntity.Amount < 0 && paymentEntity.Amount > -1) ||
                            (paymentEntity.Amount > 0 && paymentEntity.Amount < 1) ||
                            (paymentEntity.Amount > 10000) ||
                            (paymentEntity.Amount < -10000))
                        {
                            _logger.Warn(
                                $"Odd amount for payment with required payment id: {paymentEntity.RequiredPaymentId} amount: {paymentEntity.Amount}");
                        }
                    }

                    if (Dns.GetHostName()?.Equals("DCS-SI-PROC-01") ?? false)
                    {
                        // Running on shadow
                    }
                    else
                    {
                        // Stop everything!
                        throw;
                    }
            }
                
                return new ProcessPaymentsCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new ProcessPaymentsCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}