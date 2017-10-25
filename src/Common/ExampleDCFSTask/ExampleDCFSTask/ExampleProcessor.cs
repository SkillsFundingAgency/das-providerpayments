using ExampleDCFSTask.Application.GetSomeDataQuery;
using MediatR;
using NLog;

namespace ExampleDCFSTask
{
    public class ExampleProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ExampleProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public void Process()
        {
            _logger.Debug("Getting data");
            // Use Mediatr to execute queries and/or commands
            var data = _mediator.Send(new GetSomeDataQueryRequest());
            
            // Do other work
            _logger.Debug("Doing the work on that data");
        }
    }
}
