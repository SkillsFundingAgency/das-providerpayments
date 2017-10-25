using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand
{
    public class AddPaymentsCommandHandler : IRequestHandler<AddPaymentsCommandRequest, Unit>
    {
        private readonly IPaymentRepository _paymentRepository;

        public AddPaymentsCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Unit Handle(AddPaymentsCommandRequest message)
        {
            var paymentEntities = message.Payments
                .Select(p =>
                    new PaymentEntity
                    {
                        Ukprn = p.Ukprn,
                        SubmissionId = p.SubmissionId,
                        SubmissionCollectionPeriod = p.SubmissionCollectionPeriod,
                        SubmissionAcademicYear = p.SubmissionAcademicYear,
                        PaymentType = p.PaymentType,
                        PaymentTypeName = p.PaymentTypeName,
                        Amount = p.Amount,
                        CollectionPeriodName = p.CollectionPeriodName,
                        CollectionPeriodMonth = p.CollectionPeriodMonth,
                        CollectionPeriodYear = p.CollectionPeriodYear
                    })
                .ToArray();

            _paymentRepository.AddProviderAdjustments(paymentEntities);

            return Unit.Value;
        }
    }
}