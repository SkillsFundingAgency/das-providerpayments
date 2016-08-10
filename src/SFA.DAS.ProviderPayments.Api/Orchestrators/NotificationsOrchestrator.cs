using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Api.Dto;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class NotificationsOrchestrator
    {
        private readonly IMediator _mediator;

        protected NotificationsOrchestrator()
        {
        }
        public NotificationsOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public virtual Task<PageOfPeriodEnds> GetPageOfPeriodEndNotifications(int pageNumber)
        {
            throw new NotImplementedException();
        }
    }
}