using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand
{
    public class ProcessPaymentsCommandHandler : IRequestHandler<ProcessPaymentsCommandRequest, ProcessPaymentsCommandResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public ProcessPaymentsCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public ProcessPaymentsCommandResponse Handle(ProcessPaymentsCommandRequest message)
        {
            try
            {
                var paymentEntities = message.Payments
                    .Select(p => new PaymentEntity
                    {
                        RequiredPaymentId = p.RequiredPaymentId,
                        CommitmentId = p.CommitmentId,
                        LearnRefNumber = p.LearnerRefNumber,
                        AimSeqNumber = p.AimSequenceNumber,
                        Ukprn = p.Ukprn,
                        DeliveryMonth = p.DeliveryMonth,
                        DeliveryYear = p.DeliveryYear,
                        CollectionPeriodMonth = p.CollectionPeriodMonth,
                        CollectionPeriodYear = p.CollectionPeriodYear,
                        FundingSource = (int)p.FundingSource,
                        TransactionType = (int)p.TransactionType,
                        Amount = p.Amount
                    })
                    .ToArray();

                _paymentRepository.AddPayments(paymentEntities);

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