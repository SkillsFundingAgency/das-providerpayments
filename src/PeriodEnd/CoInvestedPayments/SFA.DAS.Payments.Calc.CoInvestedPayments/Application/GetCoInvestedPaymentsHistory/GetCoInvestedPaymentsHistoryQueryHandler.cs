using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.GetCoInvestedPaymentsHistory;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calc.CoInvestedPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery
{
    public class GetCoInvestedPaymentsHistoryQueryHandler : IRequestHandler<GetCoInvestedPaymentsHistoryQueryRequest, GetCoInvestedPaymentsHistoryQueryResponse>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetCoInvestedPaymentsHistoryQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public GetCoInvestedPaymentsHistoryQueryResponse Handle(GetCoInvestedPaymentsHistoryQueryRequest message)
        {
            try
            {
                var payments = _paymentRepository.GetCoInvestedPaymentsHistory(
                                    message.DeliveryMonth,
                                    message.DeliveryYear,
                                    message.TransactionType,
                                    message.AimSequenceNumber,
                                    message.Ukprn,
                                    message.Uln,
                                    message.FrameworkCode,
                                    message.PathwayCode,
                                    message.ProgrammeType,
                                    message.StandardCode,
                                    message.LearnRefNumber);

                return new GetCoInvestedPaymentsHistoryQueryResponse
                {
                    IsValid = true,
                    Items = payments?.Select(p => new PaymentHistory
                    {
                        FundingSource =(FundingSource) p.FundingSource,
                        RequiredPaymentId = p.RequiredPaymentId,
                        DeliveryMonth = p.DeliveryMonth,
                        DeliveryYear = p.DeliveryYear,
                        TransactionType = (TransactionType) p.TransactionType,
                        Amount = p.Amount,
                        AimSequenceNumber=p.AimSequenceNumber,
                        FrameworkCode = p.FrameworkCode,
                        PathwayCode = p.PathwayCode,
                        ProgrammeType = p.ProgrammeType,
                        StandardCode = p.StandardCode,
                        Ukprn = p.Ukprn,
                        Uln = p.Uln
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetCoInvestedPaymentsHistoryQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}