using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryHandler : IAsyncRequestHandler<GetPaymentsForAccountInPeriodQueryRequest, GetPaymentsForAccountInPeriodQueryResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPaymentsForAccountInPeriodQueryRequest> _validator;

        public GetPaymentsForAccountInPeriodQueryHandler(IPaymentRepository paymentRepository, IMapper mapper, IValidator<GetPaymentsForAccountInPeriodQueryRequest> validator)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetPaymentsForAccountInPeriodQueryResponse> Handle(GetPaymentsForAccountInPeriodQueryRequest message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid())
            {
                return new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = validationResult.Failures
                };
            }

            var entities = await _paymentRepository.GetPageOfPaymentsForAccountInPeriod(message.PeriodCode, message.AccountId, message.PageNumber);
            if (entities == null)
            {
                return new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PageNotFoundFailure()
                    }
                };
            }

            return new GetPaymentsForAccountInPeriodQueryResponse
            {
                IsValid = true,
                TotalNumberOfItems = entities.TotalNumberOfItems,
                TotalNumberOfPages = entities.TotalNumberOfPages,
                Items = _mapper.Map<PaymentEntity, Payment>(entities.Items)
            };
        }
    }
}
