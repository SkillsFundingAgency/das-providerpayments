using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Services;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments
{
    public class ProviderAdjustmentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly int _yearOfCollection;
        private readonly ICalculateProviderPayments _providerPayments;
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ProviderAdjustmentsProcessor(
            ILogger logger, 
            IMediator mediator, 
            string yearOfCollection, 
            ICalculateProviderPayments providerPayments, 
            IAdjustmentRepository adjustmentRepository, 
            IPaymentRepository paymentRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _yearOfCollection = int.Parse(yearOfCollection);
            _providerPayments = providerPayments;
            _adjustmentRepository = adjustmentRepository;
            _paymentRepository = paymentRepository;
        }
        protected ProviderAdjustmentsProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started the Provider Adjustments Processor.");

            var previousPayments = _adjustmentRepository.GetPreviousProviderAdjustments();
            var earnings = _adjustmentRepository.GetCurrentProviderAdjustments();

            var payments = _providerPayments.CalculatePayments(previousPayments, earnings);

            PopulateCollectionPeriod(payments);
            _paymentRepository.AddProviderAdjustments(payments.ToArray());

            _logger.Info("Finished the Provider Adjustments Processor.");
        }

        private void PopulateCollectionPeriod(IEnumerable<PaymentEntity> payments)
        {
            var collectionPeriod = ReturnValidGetCurrentCollectionPeriodQueryResponseOrThrow().Period;
            foreach (var payment in payments)
            {
                payment.CollectionPeriodMonth = collectionPeriod.Month;
                payment.CollectionPeriodYear = collectionPeriod.Year;
                payment.CollectionPeriodName = collectionPeriod.Name;
                payment.SubmissionAcademicYear = _yearOfCollection;
            }
        }

        private GetCurrentCollectionPeriodQueryResponse ReturnValidGetCurrentCollectionPeriodQueryResponseOrThrow()
        {
            _logger.Info("Reading current collection period.");

            var response = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!response.IsValid)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorReadingCollectionPeriodMessage, response.Exception);
            }

            if (response.Period == null)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorNoCollectionPeriodMessage);
            }

            return response;
        }
    }
}