using System;
using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ImportAccountLegalEntitiesOrchestrator : IImportAccountLegalEntitiesOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ImportAccountLegalEntitiesOrchestrator(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public void ImportAccountLegalEntities()
        {
            _logger.Info("Started importing account-legal-entities.");

            var pageNumber = 1;
            var hasMorePages = true;
            var numberOfAgreements = 0L;
            while (hasMorePages)
            {
                _logger.Info($"Starting to process page {pageNumber}");

                var response = _mediator.Send(new GetPageOfAccountLegalEntitiesQueryRequest { PageNumber = pageNumber });

                if (!response.IsValid)
                {
                    throw response.Exception;
                }

                _mediator.Send(new AddManyAccountLegalEntitiesCommandRequest {AccountLegalEntityViewModels = response.Items});

                _logger.Info($"Finished processing {pageNumber}. More pages = {response.HasMorePages}");
                numberOfAgreements += response.Items.Length;
                pageNumber++;
                hasMorePages = response.HasMorePages;
            }

            _mediator.Send(new AddAuditCommandRequest
            {
                CorrelationDate = DateTime.Today,
                AccountsRead = numberOfAgreements,
                CompletedSuccessfully = true,
                AuditType = AuditType.AccountLegalEntity
            });

            _logger.Info("Finished importing account-legal-entities.");
        }
    }
}