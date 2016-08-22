using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Dto.Hal;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class AccountsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILinkBuilder _linkBuilder;
        private readonly ILogger _logger;

        public AccountsOrchestrator(IMediator mediator, IMapper mapper, ILinkBuilder linkBuilder, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _linkBuilder = linkBuilder;
            _logger = logger;
        }

        protected AccountsOrchestrator()
        {

        }


        public virtual async Task<HalPage<AccountDto>> GetPageOfAccountsAffectedInPeriod(string periodCode, int pageNumber)
        {
            try
            {
                var response = await _mediator.SendAsync(new GetAccountsAffectedInPeriodQueryRequest
                {
                    PeriodCode = periodCode,
                    PageNumber = pageNumber
                });
                if (!response.IsValid)
                {
                    if (response.ValidationFailures.Any(f => f is PeriodNotFoundFailure))
                    {
                        throw new PeriodNotFoundException();
                    }
                    if (response.ValidationFailures.Any(f => f is PageNotFoundFailure))
                    {
                        throw new PageNotFoundException();
                    }
                    throw new BadRequestException(response.ValidationFailures);
                }

                return new HalPage<AccountDto>
                {
                    Links = new HalPageLinks
                    {
                        Next = GetAccountsAffectedPageLink(periodCode, pageNumber + 1, response.TotalNumberOfPages),
                        Prev = GetAccountsAffectedPageLink(periodCode, pageNumber - 1, response.TotalNumberOfPages),
                        First = GetAccountsAffectedPageLink(periodCode, 1, response.TotalNumberOfPages),
                        Last = GetAccountsAffectedPageLink(periodCode, response.TotalNumberOfPages, response.TotalNumberOfPages),
                    },
                    Count = response.TotalNumberOfItems,
                    Content = new HalPageItems<AccountDto>
                    {
                        Items = MapAccounts(response.Items, periodCode, response.TotalNumberOfPages)
                    }
                };
            }
            catch (Exception ex) when (!(ex is BadRequestException || ex is NotFoundException))
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }

        public virtual async Task<HalPage<PaymentDto>> GetPageOfPaymentsForAccountInPeriod(string periodCode, string accountId, int pageNumber)
        {
            try
            {
                var response = await _mediator.SendAsync(new GetPaymentsForAccountInPeriodQueryRequest
                {
                    PeriodCode = periodCode,
                    AccountId = accountId,
                    PageNumber = pageNumber
                });
                if (!response.IsValid)
                {
                    if (response.ValidationFailures.Any(f => f is PeriodNotFoundFailure))
                    {
                        throw new PeriodNotFoundException();
                    }
                    if (response.ValidationFailures.Any(f => f is AccountNotFoundFailure))
                    {
                        throw new AccountNotFoundException();
                    }
                    if (response.ValidationFailures.Any(f => f is PageNotFoundFailure))
                    {
                        throw new PageNotFoundException();
                    }
                    throw new BadRequestException(response.ValidationFailures);
                }

                return new HalPage<PaymentDto>
                {
                    Links = new HalPageLinks
                    {
                        Next = GetPaymentsPageLink(periodCode, accountId, pageNumber + 1, response.TotalNumberOfPages),
                        Prev = GetPaymentsPageLink(periodCode, accountId, pageNumber - 1, response.TotalNumberOfPages),
                        First = GetPaymentsPageLink(periodCode, accountId, 1, response.TotalNumberOfPages),
                        Last = GetPaymentsPageLink(periodCode, accountId, response.TotalNumberOfPages, response.TotalNumberOfPages),
                    },
                    Count = response.TotalNumberOfItems,
                    Content = new HalPageItems<PaymentDto>
                    {
                        Items = _mapper.Map<Payment,PaymentDto>(response.Items)
                    }
                };
            }
            catch (Exception ex) when (!(ex is BadRequestException))
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }


        private IEnumerable<AccountDto> MapAccounts(IEnumerable<Account> accounts, string periodCode, int numberOfPages)
        {
            var dtos = _mapper.Map<Account, AccountDto>(accounts);
            foreach (var item in dtos)
            {
                item.Links = new PaymentEntityLinks
                {
                    Payments = GetPaymentsPageLink(periodCode, item.Id, 1, numberOfPages)
                };
            }
            return dtos;
        }
        private HalLink GetAccountsAffectedPageLink(string periodCode, int pageNumber, int numberOfPages)
        {
            if (pageNumber < 1 || pageNumber > numberOfPages)
            {
                return null;
            }
            return new HalLink { Href = _linkBuilder.GetPeriodEndAccountsPageLink(periodCode, pageNumber) };
        }
        private HalLink GetPaymentsPageLink(string periodCode, string accountId, int pageNumber, int numberOfPages)
        {
            if (pageNumber < 1 || pageNumber > numberOfPages)
            {
                return null;
            }
            return new HalLink { Href = _linkBuilder.GetAccountPaymentsLink(periodCode, accountId, pageNumber) };
        }
    }
}