using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.GetPaymentsRequiringReversalQuery;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.SetAdjustmentAsReversedCommand;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments
{
    public class ManualAdjustmentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly string _yearOfCollection;

        public ManualAdjustmentsProcessor(ILogger logger, IMediator mediator, string yearOfCollection)
        {
            _logger = logger;
            _mediator = mediator;
            _yearOfCollection = yearOfCollection;
        }
        protected ManualAdjustmentsProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started the Manual Adjustments Processor.");

            var paymentsToReverseResponse = _mediator.Send(new GetPaymentsRequiringReversalQueryRequest());
            if (!paymentsToReverseResponse.IsValid)
            {
                throw paymentsToReverseResponse.Exception;
            }

            if (paymentsToReverseResponse.HasAnyItems())
            {
                foreach (var requiredPaymentIdToReverse in paymentsToReverseResponse.Items)
                {
                    _logger.Info($"Started processing adjustment for {requiredPaymentIdToReverse}");

                    var reversalResponse = _mediator.Send(new ReversePaymentCommandRequest
                    {
                        RequiredPaymentIdToReverse = requiredPaymentIdToReverse,
                        YearOfCollection = _yearOfCollection
                    });
                    if (!reversalResponse.IsValid)
                    {
                        throw reversalResponse.Exception;
                    }

                    var setReversedResponse = _mediator.Send(new SetAdjustmentAsReversedCommandRequest
                    {
                        RequiredPaymentIdToReverse = requiredPaymentIdToReverse,
                        RequiredPaymentIdForReversal = reversalResponse.RequiredPaymentIdForReversal
                    });
                    if (!setReversedResponse.IsValid)
                    {
                        throw setReversedResponse.Exception;
                    }
                }
            }
            else
            {
                _logger.Info("No adjustments found to process.");
            }

            _logger.Info("Finished the Manual Adjustments Processor.");
        }
    }
}
