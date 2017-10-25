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
                RequiredPaymentId = message.Payment.RequiredPaymentId,
                DeliveryMonth = message.Payment.DeliveryMonth,
                DeliveryYear = message.Payment.DeliveryYear,
                CollectionPeriodName = message.Payment.CollectionPeriodName,
                CollectionPeriodMonth = message.Payment.CollectionPeriodMonth,
                CollectionPeriodYear = message.Payment.CollectionPeriodYear,
                FundingSource = (int)message.Payment.FundingSource,
                TransactionType = (int)message.Payment.TransactionType,
                Amount = message.Payment.Amount
            });

            return Unit.Value;
        }
    }
}