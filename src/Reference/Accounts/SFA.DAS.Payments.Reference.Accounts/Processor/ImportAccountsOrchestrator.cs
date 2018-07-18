using System;
using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ImportAccountsOrchestrator : IImportAccountsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ImportAccountsOrchestrator(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        public void ImportAccounts()
        {
            _logger.Info("Started importing accounts.");

            var correlationDate = DateTime.Today;
            var pageNumber = 1;
            var hasMorePages = true;
            var numberOfAccounts = 0l;
            while (hasMorePages)
            {
                _logger.Info($"Starting to process page {pageNumber}");

                var response = _mediator.Send(new GetPageOfAccountsQueryRequest { PageNumber = pageNumber, CorrelationDate = correlationDate });

                if (!response.IsValid)
                {
                    throw response.Exception;
                }

                foreach (var account in response.Items)
                {
                    _mediator.Send(new AddOrUpdateAccountCommandRequest { Account = account, CorrelationDate = correlationDate });
                }

                _logger.Info($"Finished processing {pageNumber}. More pages = {response.HasMorePages}");
                numberOfAccounts += response.Items.Length;
                pageNumber++;
                hasMorePages = response.HasMorePages;
            }

            _mediator.Send(new AddAuditCommandRequest
            {
                CorrelationDate = correlationDate,
                AccountRead = numberOfAccounts,
                CompletedSuccessfully = true
            });
            _logger.Info("Finished importing accounts.");
        }
    }
}