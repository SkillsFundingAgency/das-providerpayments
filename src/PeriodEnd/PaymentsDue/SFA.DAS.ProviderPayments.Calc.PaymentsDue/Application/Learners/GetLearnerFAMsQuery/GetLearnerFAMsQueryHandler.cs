using System;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Learners.GetLearnerFAMsQuery
{
    public class GetLearnerFAMsQueryHandler : IRequestHandler<GetLearnerFAMsQueryRequest, GetLearnerFAMsQueryResponse>
    {
        private readonly ILearnerFAMRepository _learnerFAMRepository;

        public GetLearnerFAMsQueryHandler(ILearnerFAMRepository learnerFAMRepository)
        {
            _learnerFAMRepository = learnerFAMRepository;
        }

        public GetLearnerFAMsQueryResponse Handle(GetLearnerFAMsQueryRequest message)
        {
            try
            {
                return new GetLearnerFAMsQueryResponse
                {
                    IsValid = true,
                    Items = _learnerFAMRepository.GetLearnerFAMRecords(message.LearnRefNumber)
                };
            }
            catch (Exception exception)
            {
                return new GetLearnerFAMsQueryResponse
                {
                    IsValid = false,
                    Exception = exception
                };
            }
            
        }
    }
}
