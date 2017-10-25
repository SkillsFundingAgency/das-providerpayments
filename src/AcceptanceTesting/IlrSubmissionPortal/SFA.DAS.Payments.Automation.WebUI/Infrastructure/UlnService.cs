using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;
using MediatR;
using SFA.DAS.Payments.Automation.WebUI.Models;
using SFA.DAS.Payments.Automation.Application.GetAllUsedUlns;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public class UlnService : IUlnService
    {
        private readonly IMediator _mediator;

        public UlnService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<long> GetNewUlns(int count)
        {
            var result = new List<long>();

            for (var i = 0; i < count; i++)
            {
                var response = _mediator.Send(new GetNextUlnQueryRequest
                {
                    LearnerKey = $"Learner {i}" ,
                    Scenarioname= $"Scenario {Guid.NewGuid().ToString()}",
                    Ukprn=10000
                });

                if (response.IsSuccess)
                {
                    result.Add(response.Value);
                }
            }

            return result;
        }
        public List<UsedUlnModel> GetAllUsedUlns()
        {
            var response = _mediator.Send(new GetAllUsedUlnsQueryRequest
            {
            });

            if (!response.IsSuccess)
            {
                throw new Exception("Error creating content response", response.Error);
            }
            else
            {
                var items = new List<UsedUlnModel>();

                response.UsedUlns.ForEach(x =>
                        items.Add(
                            new UsedUlnModel
                            {
                                LearnRefNumber = x.LearnRefNumber,
                                Ukprn = x.Ukprn,
                                ScenarioName = x.ScenarioName,
                                Uln = x.Uln
                            }
                            )
                    );
                return items;
            }

        }
    }
}