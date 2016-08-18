using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Dto.Hal;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;
using SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class NotificationsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILinkBuilder _linkBuilder;

        protected NotificationsOrchestrator()
        {
        }
        public NotificationsOrchestrator(IMediator mediator, ILinkBuilder linkBuilder)
        {
            _mediator = mediator;
            _linkBuilder = linkBuilder;
        }

        public virtual async Task<HalPage<PeriodEndDto>> GetPageOfPeriodEndNotifications(int pageNumber)
        {
            var response = await _mediator.SendAsync(new GetPageOfPeriodEndsQueryRequest { PageNumber = pageNumber });

            return new HalPage<PeriodEndDto>
            {
                Links = new HalPageLinks
                {
                    Next = GetPageLink(pageNumber + 1, response.TotalNumberOfPages),
                    Prev = GetPageLink(pageNumber - 1, response.TotalNumberOfPages),
                    First = GetPageLink(1, response.TotalNumberOfPages),
                    Last = GetPageLink(response.TotalNumberOfPages, response.TotalNumberOfPages),
                },
                Count = response.TotalNumberOfItems,
                Content = new HalPageItems<PeriodEndDto>
                {
                    Items = response.Items.Select(x => new PeriodEndDto
                    {
                        Period = new PeriodDto
                        {
                            Code = x.Period.Code,
                            PeriodType = (PeriodType)((int)x.Period.PeriodType)
                        },
                        TotalValue = 12345.67m,
                        NumberOfProviders = 23,
                        NumberOfEmployers = 93,
                        PaymentRunDate = new DateTime(2017, 5, 5)
                    })
                }
            };
        }

        private HalLink GetPageLink(int pageNumber, int numberOfPages)
        {
            if (pageNumber < 1 || pageNumber > numberOfPages)
            {
                return null;
            }
            return new HalLink { Href = _linkBuilder.GetPeriodEndNotificationPageLink(pageNumber) };
        }
    }
}