using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.Payments.Automation.Infrastructure.Data;
using System.Linq;

namespace SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand
{
    public class GetNextUlnQueryHandler : IRequestHandler<GetNextUlnQueryRequest, GetNextUlnQueryApplicationScalarResponse>
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public GetNextUlnQueryHandler(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public GetNextUlnQueryApplicationScalarResponse Handle(GetNextUlnQueryRequest message)
        {
            try
            {
               

                long uln = _referenceDataRepository.GetNextUln(message.Scenarioname, message.LearnerKey,message.Ukprn);
                

                return new GetNextUlnQueryApplicationScalarResponse
                {
                    Value = uln
                };
                
            }
            catch (Exception ex)
            {
                return new GetNextUlnQueryApplicationScalarResponse
                {
                    Error = ex
                };
            }
        }
        
    }
}
