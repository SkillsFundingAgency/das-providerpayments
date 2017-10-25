using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.Payments.Automation.Infrastructure.Data;
using System.Linq;
using SFA.DAS.Payments.Automation.Application.Entities;

namespace SFA.DAS.Payments.Automation.Application.GetAllUsedUlns
{
    public class GetAllUsedUlnsQueryHandler : IRequestHandler<GetAllUsedUlnsQueryRequest, GetAllUsedUlnsQueryResponse>
    {
        private readonly ILearnersRepository _repository;

        public GetAllUsedUlnsQueryHandler(ILearnersRepository repository)
        {
            _repository = repository;
        }

        public GetAllUsedUlnsQueryResponse Handle(GetAllUsedUlnsQueryRequest message)
        {
            try
            {

                var ulns = _repository.GetAllUsedUlns();
                var items = new List<UsedUlnRecord>();
                if (ulns != null)
                {
                    ulns.ForEach(x =>
                            items.Add(
                                new UsedUlnRecord
                                {
                                    LearnRefNumber = x.LearnRefNumber,
                                    Ukprn = x.Ukprn,
                                    ScenarioName = x.ScenarioName,
                                    Uln = x.Uln
                                }
                                )
                        );
                }
                return new GetAllUsedUlnsQueryResponse
                {
                    UsedUlns = items
                };

            }
            catch (Exception ex)
            {
                return new GetAllUsedUlnsQueryResponse
                {
                    Error = ex
                };
            }
        }

       
    }
}
