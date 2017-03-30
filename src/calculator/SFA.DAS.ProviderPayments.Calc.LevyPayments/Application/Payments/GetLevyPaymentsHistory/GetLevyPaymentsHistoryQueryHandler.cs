using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery
{
    public class GetLevyPaymentsHistoryQueryHandler : IRequestHandler<GetLevyPaymentsHistoryQueryRequest, GetLevyPaymentsHistoryQueryResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetLevyPaymentsHistoryQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public GetLevyPaymentsHistoryQueryResponse Handle(GetLevyPaymentsHistoryQueryRequest message)
        {
            try
            {
                var levyPayments = _paymentRepository.GetLevyPaymentsHistory(message.DeliveryMonth,message.DeliveryYear,message.TransactionType,message.CommitmentId);

                return new GetLevyPaymentsHistoryQueryResponse
                {
                    IsValid = true,
                    Items = levyPayments == null
                        ? null
                        : levyPayments.Select(p => new PaymentHistory
                        {
                            CommitmentId = p.CommitmentId,
                            RequiredPaymentId = p.RequiredPaymentId,
                            DeliveryMonth = p.DeliveryMonth,
                            DeliveryYear = p.DeliveryYear,
                            TransactionType = (TransactionType) p.TransactionType,
                            Amount = p.Amount
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetLevyPaymentsHistoryQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}