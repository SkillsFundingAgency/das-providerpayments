using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.payments.Automation.Assertions;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Application.Payments.GetPaymentsForUkprn;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;
using SFA.DAS.Payments.Automation.Application.Submission.TransformExpecationsCommand;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure.Assertions
{
    public class AssertionsService : IAssertionsService
    {
        private readonly IMediator _mediator;

        public AssertionsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Dictionary<string, List<AssertionResult>>> AssertPaymentResults(IlrBuilderRequest request)
        {
            var specifications = ParseGherkin(request.Gherkin);
            var paymentResults = await GetPaymentsResults(request.Ukprn).ConfigureAwait(false);

            var learners = TransformationHelpers.TransformLearnerKeysForLearners(specifications);
            TransformExpectations(specifications, request);
            
            var results = new Dictionary<string,List<AssertionResult>>();
            foreach (var specification in specifications)
            {
                var ulns = GetUlns(specification.Name, request.Ukprn, learners.Keys.ToList());
                var payments = paymentResults.Where(x => ulns.Contains(x.Uln)).ToList();
                var result = PaymentAssertions.AssertPayments(specification.Expectations.EarningsAndPayments, payments, specification.Name);

                results.Add(specification.Name,result );
            }

            return results;
        }


        private Specification[] ParseGherkin(string gherkin)
        {
            var response = _mediator.Send(new ParseGherkinQueryRequest
            {
                GherkinSpecs = gherkin
            });

            if (!response.IsSuccess)
            {
                throw new Exception(response.Error.Message, response.Error);
            }

            return response.Results;
        }

        private async Task<List<PaymentResult>> GetPaymentsResults(long ukprn)
        {
            var response = await _mediator.SendAsync(new GetPaymentsForUkprnRequest
            {
                Ukprn = ukprn
            }).ConfigureAwait(false);

            if (!response.IsSuccess)
            {
                throw new Exception(response.Error.Message, response.Error);
            }

            return response.Payments;
        }

        private List<long> GetUlns(string scenario, long ukprn, List<string> learners)
        {
            var ulns = new List<long>();
            foreach (var item in learners)
            {
                var response = _mediator.Send(new GetNextUlnQueryRequest
                {
                    LearnerKey = item,
                    Ukprn = ukprn,
                    Scenarioname = scenario
                });

                if (response.IsSuccess)
                {
                    ulns.Add(response.Value);
                }
            }
            return ulns;
        }

        private void TransformExpectations(Specification[] specifications, IlrBuilderRequest request)
        {
            if (request.ShiftToMonth.HasValue || request.ShiftToYear.HasValue)
            {
                foreach (var spec in specifications)
                {
                    var response = _mediator.Send(new TransformExpecationsCommandRequest
                    {
                        Expectations = spec.Expectations,
                        ShiftToMonth = request.ShiftToMonth,
                        ShiftToYear = request.ShiftToYear
                    });
                    if (!response.IsSuccess)
                    {
                        throw new Exception($"Error transforming expectations - {response.Error.Message}", response.Error);
                    }
                }
            }
        }
    }
}