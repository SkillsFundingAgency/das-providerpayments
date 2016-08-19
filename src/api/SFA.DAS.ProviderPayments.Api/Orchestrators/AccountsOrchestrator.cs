using System;
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

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class AccountsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILinkBuilder _linkBuilder;
        private readonly ILogger _logger;

        public AccountsOrchestrator(IMediator mediator, ILinkBuilder linkBuilder, ILogger logger)
        {
            _mediator = mediator;
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
                        Next = GetAccountsAffectedPageLink(pageNumber + 1, response.TotalNumberOfPages),
                        Prev = GetAccountsAffectedPageLink(pageNumber - 1, response.TotalNumberOfPages),
                        First = GetAccountsAffectedPageLink(1, response.TotalNumberOfPages),
                        Last = GetAccountsAffectedPageLink(response.TotalNumberOfPages, response.TotalNumberOfPages),
                    },
                    Count = response.TotalNumberOfItems,
                    Content = new HalPageItems<AccountDto>
                    {
                        Items = response.Items.Select(x =>
                            new AccountDto
                            {
                                Id = x.Id,
                                Links = new PaymentEntityLinks
                                {
                                    Payments = new HalLink { Href = _linkBuilder.GetAccountPaymentsLink(periodCode, x.Id, 1) }
                                }
                            })
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
                        Items = response.Items.Select(x =>
                            new PaymentDto
                            {
                                Account = new AccountDto
                                {
                                    Id = x.Account.Id
                                },
                                Provider = new ProviderDto
                                {
                                    Ukprn = x.Provider.Ukprn
                                },
                                Apprenticeship = new ApprenticeshipDto
                                {
                                    Learner = new LearnerDto
                                    {
                                        Uln = x.Apprenticeship.Learner.Uln
                                    },
                                    Course = new CourseDto
                                    {
                                        StandardCode = x.Apprenticeship.Course.StandardCode,
                                        PathwayCode = x.Apprenticeship.Course.PathwayCode,
                                        FrameworkCode = x.Apprenticeship.Course.FrameworkCode,
                                        ProgrammeType = x.Apprenticeship.Course.ProgrammeType
                                    }
                                },
                                DeliveryPeriod = new PeriodDto
                                {
                                    Code = x.DeliveryPeriod.Code,
                                    PeriodType = (PeriodType)(int)x.DeliveryPeriod.PeriodType
                                },
                                ReportedPeriod = new PeriodDto
                                {
                                    Code = x.ReportedPeriod.Code,
                                    PeriodType = (PeriodType)(int)x.DeliveryPeriod.PeriodType
                                },
                                Amount = x.Amount,
                                TransactionType = (TransactionType)(int)x.TransactionType,
                                FundingType = (FundingType)(int)x.FundingType
                            })
                    }
                };
            }
            catch (Exception ex) when (!(ex is BadRequestException))
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }


        private HalLink GetAccountsAffectedPageLink(int pageNumber, int numberOfPages)
        {
            if (pageNumber < 1 || pageNumber > numberOfPages)
            {
                return null;
            }
            return new HalLink { Href = _linkBuilder.GetPeriodEndAccountsPageLink(pageNumber) };
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