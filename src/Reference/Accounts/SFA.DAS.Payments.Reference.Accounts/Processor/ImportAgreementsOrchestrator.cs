using MediatR;
using NLog;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAgreementsQuery;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ImportAgreementsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ImportAgreementsOrchestrator(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public void ImportAccounts()
        {
            _logger.Info("Started importing agreements.");

            var pageNumber = 1;
            var hasMorePages = true;
            var numberOfAgreements = 0l;
            while (hasMorePages)
            {
                _logger.Info($"Starting to process page {pageNumber}");

                var response = _mediator.Send(new GetPageOfAgreementsQueryRequest { PageNumber = pageNumber });

                if (!response.IsValid)
                {
                    throw response.Exception;
                }



                _logger.Info($"Finished processing {pageNumber}. More pages = {response.HasMorePages}");
                numberOfAgreements += response.Items.Length;
                pageNumber++;
                hasMorePages = response.HasMorePages;
            }

            _logger.Info("Finished importing agreements.");
        }
    }
}