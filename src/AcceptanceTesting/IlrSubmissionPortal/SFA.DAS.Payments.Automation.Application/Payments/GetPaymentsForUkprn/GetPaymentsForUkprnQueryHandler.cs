using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Payments.Automation.Infrastructure.Data;

namespace SFA.DAS.Payments.Automation.Application.Payments.GetPaymentsForUkprn
{
    public class GetPaymentsForUkprnQueryHandler : IAsyncRequestHandler<GetPaymentsForUkprnRequest, GetPaymentsForUkprnResponse>
    {
        private readonly IPaymentsClient _paymentsClient;

        public GetPaymentsForUkprnQueryHandler(IPaymentsClient paymentsClient)
        {
            _paymentsClient= paymentsClient;
        }

        public async Task<GetPaymentsForUkprnResponse> Handle(GetPaymentsForUkprnRequest message)
        {
            try
            {
                var items = await _paymentsClient.GetPayments(message.Ukprn).ConfigureAwait(false);

                return new GetPaymentsForUkprnResponse
                {
                    Payments = items
                };
            }
            catch (Exception ex)    
            {
                return new GetPaymentsForUkprnResponse
                {
                    Error = ex
                };
            }
        }
    }
}
