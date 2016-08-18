using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryHandler : IAsyncRequestHandler<GetPaymentsForAccountInPeriodQueryRequest, GetPaymentsForAccountInPeriodQueryResponse>
    {
        public Task<GetPaymentsForAccountInPeriodQueryResponse> Handle(GetPaymentsForAccountInPeriodQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
