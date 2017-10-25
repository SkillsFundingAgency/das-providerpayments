using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface IPaymentReportService
    {
        Task<IEnumerable<PaymentReport>> GetAllReportPayments();
    }

    public class PaymentReportService : IPaymentReportService
    {
        private readonly IPaymentReportRepository _paymentReportRepository;
        private readonly IMapper _mapper;

        public PaymentReportService(IPaymentReportRepository paymentReportRepository,
                                    IMapper mapper)
        {
            _paymentReportRepository = paymentReportRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentReport>> GetAllReportPayments()
        {
            var paymentReportEntities = await _paymentReportRepository.All();
            var payments = _mapper.Map<PaymentReport[]>(paymentReportEntities);

            return payments;
        }
    }
}