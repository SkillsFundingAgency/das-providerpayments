using MediatR;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommandRequest, Unit>
    {
        private readonly IPaymentRepository _paymentRepository;

        public ProcessPaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Unit Handle(ProcessPaymentCommandRequest message)
        {
            _paymentRepository.AddPayment(new Infrastructure.Data.Entities.PaymentEntity
            {
                CommitmentId = message.Payment.CommitmentId,
                LearnerRefNumber = message.Payment.LearnerRefNumber,
                AimSequenceNumber = message.Payment.AimSequenceNumber,
                Ukprn = message.Payment.Ukprn,
                DeliveryMonth = message.Payment.DeliveryMonth,
                DeliveryYear = message.Payment.DeliveryYear,
                CollectionPeriodMonth = message.Payment.CollectionPeriodMonth,
                CollectionPeriodYear = message.Payment.CollectionPeriodYear,
                Source = (int)message.Payment.Source,
                TransactionType = (int)message.Payment.TransactionType,
                Amount = message.Payment.Amount
            });

            return Unit.Value;
        }
    }
}