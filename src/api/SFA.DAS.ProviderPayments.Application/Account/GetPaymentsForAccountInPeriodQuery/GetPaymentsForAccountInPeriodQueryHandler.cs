using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryHandler : PagedQueryHandler<GetPaymentsForAccountInPeriodQueryRequest, GetPaymentsForAccountInPeriodQueryResponse, PaymentEntity, Payment>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentsForAccountInPeriodQueryHandler(IPaymentRepository paymentRepository, IMapper mapper, IValidator<GetPaymentsForAccountInPeriodQueryRequest> validator)
            : base(mapper, validator)
        {
            _paymentRepository = paymentRepository;
        }

        protected override async Task<PageOfEntities<PaymentEntity>> GetData(GetPaymentsForAccountInPeriodQueryRequest request)
        {
            return await _paymentRepository.GetPageOfPaymentsForAccountInPeriod(request.PeriodCode, request.AccountId, request.PageNumber);
        }
    }
}
