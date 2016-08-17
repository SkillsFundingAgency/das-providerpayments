using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Dto.Hal;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class AccountsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILinkBuilder _linkBuilder;

        public AccountsOrchestrator(IMediator mediator, ILinkBuilder linkBuilder)
        {
            _mediator = mediator;
            _linkBuilder = linkBuilder;
        }

        protected AccountsOrchestrator()
        {
            
        }

        public virtual async Task<HalPage<AccountDto>> GetPageOfAccountsAffectedInPeriod(string periodCode, int pageNumber)
        {
            var response = await _mediator.SendAsync(new GetAccountsAffectedInPeriodQueryRequest { PeriodCode = periodCode, PageNumber = pageNumber });

            return new HalPage<AccountDto>
            {
                Links = new HalPageLinks
                {
                    Next = GetPageLink(pageNumber + 1, response.TotalNumberOfPages),
                    Prev = GetPageLink(pageNumber - 1, response.TotalNumberOfPages),
                    First = GetPageLink(1, response.TotalNumberOfPages),
                    Last = GetPageLink(response.TotalNumberOfPages, response.TotalNumberOfPages),
                },
                Count = response.TotalNumberOfItems,
                Content = new HalPageItems<AccountDto>
                {
                    Items = response.Items.Select(x=>
                    new AccountDto
                    {
                        Id = x.Id,
                        Links = new PaymentEntityLinks
                        {
                            Payments = new HalLink { Href = _linkBuilder.GetAccountPaymentsLink(periodCode, x.Id) }
                        }
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
            return new HalLink { Href = _linkBuilder.GetPeriodEndAccountsPageLink(pageNumber) };
        }
    }
}