using System;
using MediatR;
using SFA.DAS.Payments.Automation.Infrastructure.Data;

namespace SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUkprn
{
    public class GetNextUkprnQueryHandler : IRequestHandler<GetNextUkprnQueryRequest, GetNextUkprnQueryApplicationScalarResponse>
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public GetNextUkprnQueryHandler(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public GetNextUkprnQueryApplicationScalarResponse Handle(GetNextUkprnQueryRequest message)
        {
            try
            {
                var item = _referenceDataRepository.GetNextUkprn();

                return new GetNextUkprnQueryApplicationScalarResponse
                {
                    Value = item
                };
            }
            catch (Exception ex)
            {
                return new GetNextUkprnQueryApplicationScalarResponse
                {
                    Error = ex
                };
            }
        }
    }
}
