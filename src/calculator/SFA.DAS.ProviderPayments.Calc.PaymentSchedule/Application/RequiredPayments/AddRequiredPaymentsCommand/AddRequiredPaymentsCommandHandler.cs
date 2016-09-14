using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.RequiredPayments.AddRequiredPaymentsCommand
{
    public class AddRequiredPaymentsCommandHandler : IRequestHandler<AddRequiredPaymentsCommandRequest, AddRequiredPaymentsCommandResponse>
    {
        private readonly IRequiredPaymentRepository _requiredPaymentRepository;

        public AddRequiredPaymentsCommandHandler(IRequiredPaymentRepository requiredPaymentRepository)
        {
            _requiredPaymentRepository = requiredPaymentRepository;
        }

        public AddRequiredPaymentsCommandResponse Handle(AddRequiredPaymentsCommandRequest message)
        {
            try
            {
                var paymentEntities = message.Payments
                    .Select(
                        p => new RequiredPaymentEntity
                        {
                            LearnRefNumber = p.LearnerRefNumber,
                            AimSeqNumber = p.AimSequenceNumber,
                            Ukprn = p.Ukprn,
                            DeliveryMonth = p.DeliveryMonth,
                            DeliveryYear = p.DeliveryYear,
                            TransactionType = p.TransactionType,
                            AmountDue = p.AmountDue
                        })
                    .ToArray();

                _requiredPaymentRepository.AddRequiredPayments(paymentEntities);

                return new AddRequiredPaymentsCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddRequiredPaymentsCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}