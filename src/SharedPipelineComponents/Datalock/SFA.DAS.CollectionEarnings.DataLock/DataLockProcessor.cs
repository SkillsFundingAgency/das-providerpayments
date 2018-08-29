using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ICommitmentRepository _commitmentRepository;
        private readonly IValidateDatalocks _datalockValidationService;
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IDatalockRepository _datalockRepository;

        public DataLockProcessor(
            ILogger logger, 
            IMediator mediator, 
            ICommitmentRepository commitmentRepository, 
            IValidateDatalocks datalockValidationService, 
            IRawEarningsRepository rawEarningsRepository, 
            IDatalockRepository datalockRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _commitmentRepository = commitmentRepository;
            _datalockValidationService = datalockValidationService;
            _rawEarningsRepository = rawEarningsRepository;
            _datalockRepository = datalockRepository;
        }

        public virtual void Process()
        {
            _logger.Info("Started Data Lock Processor.");
            _logger.Info($"Using timeout of: {DataLockTask.CommandTimeout}");

            var providersQueryResponse = ReturnValidGetProvidersQueryResponseOrThrow();

            var accountIdsThatHaveNonPayableFlagSet = AccountIdsThatHaveNonPayableFlagSet();

            if (providersQueryResponse.HasAnyItems())
            {
                Parallel.ForEach(providersQueryResponse.Items, provider =>
                {
                    _logger.Info($"Performing Data Lock Validation for provider with ukprn {provider.Ukprn}.");

                    var commitments = CommitmentsForProvider(provider.Ukprn);
                    var providerCommitments = new ProviderCommitments(commitments);

                    var priceEpisodes = EarningsForProvider(provider.Ukprn);

                    var dataLockValidationResult = _datalockValidationService.ValidateDatalockForProvider(
                        providerCommitments,
                        priceEpisodes, accountIdsThatHaveNonPayableFlagSet);

                    _datalockRepository.WriteValidationErrors(dataLockValidationResult.ValidationErrors);
                    _datalockRepository.WritePriceEpisodeMatches(dataLockValidationResult.PriceEpisodeMatches);
                    _datalockRepository.WritePriceEpisodePeriodMatches(dataLockValidationResult
                        .PriceEpisodePeriodMatches);
                    _datalockRepository.WriteDatalockOutput(dataLockValidationResult.DatalockOutputEntities);
                    _datalockRepository.WriteValidationErrorsByPeriod(dataLockValidationResult
                        .ValidationErrorsByPeriod);
                });
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Data Lock Processor.");
        }

        private ImmutableHashSet<long> AccountIdsThatHaveNonPayableFlagSet()
        {
            var dasAccountsQueryResponse = ReturnValidGetDasAccountsQueryResponseOrThrow().Items;
            var dasAccountIdsThatHaveNonPayableFlagSet = ImmutableHashSet
                .Create(dasAccountsQueryResponse
                    .Where(x => x.IsLevyPayer == false)
                    .Select(x => x.AccountId)
                    .ToArray());
            return dasAccountIdsThatHaveNonPayableFlagSet;
        }

        private GetProvidersQueryResponse ReturnValidGetProvidersQueryResponseOrThrow()
        {
            var providersQueryResponse = _mediator.Send(new GetProvidersQueryRequest());

            if (!providersQueryResponse.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorReadingProvidersMessage, providersQueryResponse.Exception);
            }

            return providersQueryResponse;
        }

        private GetDasAccountsQueryResponse ReturnValidGetDasAccountsQueryResponseOrThrow()
        {
            var queryResponse = _mediator.Send(new GetDasAccountsQueryRequest());

            if (!queryResponse.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorReadingAccountsMessage, queryResponse.Exception);
            }

            return queryResponse;
        }


        private IEnumerable<CommitmentEntity> CommitmentsForProvider(long ukprn)
        {
            _logger.Info($"Reading commitments for provider with ukprn {ukprn}.");

            var commitments = _commitmentRepository.GetProviderCommitments(ukprn);

            return commitments;
        }

        private IEnumerable<RawEarning> EarningsForProvider(long ukprn)
        {
            _logger.Info($"Reading price episodes for provider with ukprn {ukprn}.");

            var earnings = _rawEarningsRepository.GetAllAct1ForProvider(ukprn);

            return earnings;
        }
    }
}
