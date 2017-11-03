using System;
using System.Linq;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using MediatR;
using SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand;
using SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using System.Collections.Generic;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public class IlrBuilderService : IIlrBuilderService
    {
        private readonly IMediator _mediator;

        public IlrBuilderService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IlrBuilderResponse BuildIlrWithRefenceData(IlrBuilderRequest request)
        {
            try
            {
                var specifications = ParseGherkin(request.Gherkin);

                TransformLearnerKeysForCommitments(specifications);
                TransformSpecifications(specifications, request);

                var learnerScenarios = TransformLearnerKeysForLearners(specifications);

                ValidateScenarios(specifications);

                var contentResponse = CreateSubmissionContent(specifications, request.Ukprn, learnerScenarios, request.AcademicYear);

                return new IlrBuilderResponse
                {
                    FileName = contentResponse.FileName,
                    IlrContent = contentResponse.IlrContent,
                    CommitmentsContent = contentResponse.CommitmentsSql,
                    AccountsContent = contentResponse.AccountsSql,
                    IsSuccess = true,
                    UsedUlnCSV = contentResponse.UsedUlnCsv,
                    CommitmentsBulkCsv = contentResponse.CommitmentsBulkCsv
                };
            }
            catch (Exception ex)
            {
                return new IlrBuilderResponse
                {
                    Exception = ex,
                    IsSuccess = false
                };
            }
        }

        private void ValidateScenarios(Specification[] specifications)
        {
            var validationResponse = _mediator.Send(new ValidateSpecificationsQueryRequest { Specifications = specifications });
            if (!validationResponse.IsValid)
            {
                throw new InvalidSpecificationsException(validationResponse.Errors);
            }
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
        private void TransformSpecifications(Specification[] specifications, IlrBuilderRequest request)
        {
            if (request.ShiftToMonth.HasValue || request.ShiftToYear.HasValue)
            {
                foreach (var spec in specifications)
                {
                    var response = _mediator.Send(new TransformSubmissionCommandRequest
                    {
                        Specification = spec,
                        ShiftToMonth = request.ShiftToMonth,
                        ShiftToYear = request.ShiftToYear
                    });
                    if (!response.IsSuccess)
                    {
                        throw new Exception($"Error transforming specifications - {response.Error.Message}", response.Error);
                    }
                }
            }
        }

        private Dictionary<string, string> TransformLearnerKeysForLearners(Specification[] specifications)
        {
            var index = 1;

            var learnerScenarios = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var processedLearners = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var spec in specifications)
            {
                processedLearners = new Dictionary<string, string>();

                foreach (var learner in spec.Arrangement.LearnerRecords)
                {
                    if (!processedLearners.ContainsKey(learner.LearnerKey))
                    {
                        var key = $"{learner.LearnerKey}{index}";
                        processedLearners.Add(learner.LearnerKey, key);
                        learnerScenarios.Add(key, spec.Name);

                        learner.LearnerKey = $"{learner.LearnerKey}{index}";
                    }
                    else
                    {
                        learner.LearnerKey = processedLearners[learner.LearnerKey];
                    }
                }
                index += 1;

            }
            return learnerScenarios;
        }

        private void TransformLearnerKeysForCommitments(Specification[] specifications)
        {
            var index = 1;
            var processedLearners = new Dictionary<string, string>();

            foreach (var spec in specifications)
            {
                processedLearners = new Dictionary<string, string>();

                if (spec.Arrangement.Commitments != null || spec.Arrangement.Commitments.Any())
                {

                    foreach (var c in spec.Arrangement.Commitments)
                    {
                        if (!processedLearners.ContainsKey(c.LearnerKey))
                        {
                            var key = $"{c.LearnerKey}{index}";
                            processedLearners.Add(c.LearnerKey, key);

                            c.LearnerKey = $"{c.LearnerKey}{index}";

                        }
                        else
                        {
                            c.LearnerKey = processedLearners[c.LearnerKey];
                        }

                    }
                }
                index += 1;
            }

        }

        private CreateSubmissionCommandResponse CreateSubmissionContent(Specification[] specifications, long ukprn, Dictionary<string, string> learnerScenarios, string academicYear)
        {

            var contentResponse = _mediator.Send(new CreateSubmissionCommandRequest
            {
                Ukprn = ukprn,
                AcademicYear = academicYear,
                LearnerScenarios = learnerScenarios,
                LearnerRecords = specifications.SelectMany(x => x.Arrangement.LearnerRecords).ToList(),
                Commitments = specifications.SelectMany(x => x.Arrangement.Commitments).ToList(),
                Accounts = specifications.SelectMany(x => x.Arrangement.LevyBalances).ToList(),
                EmploymentStatuses = specifications.SelectMany(x => x.Arrangement.EmploymentStatuses).ToList()
            });

            if (!contentResponse.IsSuccess)
            {
                throw new Exception($"Error creating content response - {contentResponse.Error.Message}", contentResponse.Error);
            }

            return contentResponse;
        }

    }
}
