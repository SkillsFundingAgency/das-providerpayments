using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Learners.GetLearnerFAMsQuery
{
    public class GetLearnerFAMsQueryRequest : IRequest<GetLearnerFAMsQueryResponse>
    {
        public string LearnRefNumber { get; set; }
    }
}
