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
using SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators
{
    public class NotificationsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILinkBuilder _linkBuilder;
        private readonly ILogger _logger;

        protected NotificationsOrchestrator()
        {
        }
        public NotificationsOrchestrator(IMediator mediator, IMapper mapper, ILinkBuilder linkBuilder, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _linkBuilder = linkBuilder;
            _logger = logger;
        }

        public virtual async Task<HalPage<PeriodEndDto>> GetPageOfPeriodEndNotifications(int pageNumber)
        {
            try
            {
                var response = await _mediator.SendAsync(new GetPageOfPeriodEndsQueryRequest { PageNumber = pageNumber });
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
                        Items = MapPeriodEnds(response.Items)
                    }
                };
            }
            catch (Exception ex) when (!(ex is BadRequestException || ex is NotFoundException))
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }

        private IEnumerable<PeriodEndDto> MapPeriodEnds(IEnumerable<Domain.PeriodEnd> periodEnds)
        {
            var dtos = _mapper.Map<Domain.PeriodEnd, PeriodEndDto>(periodEnds).ToArray();
            foreach (var item in dtos)
            {
                item.Links = new PeriodLinks
                {
                    Accounts = new HalLink { Href = _linkBuilder.GetPeriodEndAccountsPageLink(item.Period.Code, 1) }
                };
            }
            return dtos;
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