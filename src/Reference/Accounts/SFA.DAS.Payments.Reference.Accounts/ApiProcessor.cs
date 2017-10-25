using System;
using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery;

namespace SFA.DAS.Payments.Reference.Accounts
{
    public class ApiProcessor
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ApiProcessor(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        protected ApiProcessor()
        {
            // For mocking
        }

        public virtual void Process()
        {
            _logger.Info("Started Accounts API Processor.");

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
            _logger.Info("Finished Accounts API Processor.");
        }
    }
}
