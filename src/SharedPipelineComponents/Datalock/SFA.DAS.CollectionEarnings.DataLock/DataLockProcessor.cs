﻿using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings;
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

            var dasAccountsQueryResponse = ReturnValidGetDasAccountsQueryResponseOrThrow().Items;

            if (providersQueryResponse.HasAnyItems())
            {
                foreach (var provider in providersQueryResponse.Items)
                {
                    _logger.Info($"Performing Data Lock Validation for provider with ukprn {provider.Ukprn}.");

                    var commitments = CommitmentsForProvider(provider.Ukprn);
                    var providerCommitments = new ProviderCommitments(commitments);

                    var priceEpisodes = EarningsForProvider(provider.Ukprn);
                    
                    if (priceEpisodes.Count > 0)
                    {
                        var dataLockValidationResult = _datalockValidationService.Validate(providerCommitments,
                            priceEpisodes, dasAccountsQueryResponse);

                        _datalockRepository.WriteValidationErrors(dataLockValidationResult.ValidationErrors);
                        _datalockRepository.WritePriceEpisodeMatches(dataLockValidationResult.PriceEpisodeMatches);
                        _datalockRepository.WritePriceEpisodePeriodMatches(dataLockValidationResult.PriceEpisodePeriodMatches);
                        _datalockRepository.WriteDatalockOutput(dataLockValidationResult.DatalockOutputEntities);
                    }
                    else
                    {
                        _logger.Info("No price episodes found.");
                    }
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Data Lock Processor.");
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

        private List<RawEarning> EarningsForProvider(long ukprn)
        {
            _logger.Info($"Reading price episodes for provider with ukprn {ukprn}.");

            var earnings = _rawEarningsRepository.GetAllForProvider(ukprn);

            return earnings;
        }


        private Get16To18IncentiveEarningsQueryResponse ReturnValidGetIncentiveEarningsQueryResponseOrThrow(long ukprn)
        {
            _logger.Info($"Reading incentive earnings for provider with ukprn {ukprn}.");

            var response = _mediator.Send(new Get16To18IncentiveEarningsQueryRequest
            {
                Ukprn = ukprn
            });

            if (!response.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorReadingPriceEpisodesMessage, response.Exception);
            }
            
            return response;
        }

        private RunDataLockValidationQueryResponse ReturnDataLockValidationResultOrThrow(
                                                IEnumerable<CommitmentEntity> commitments,
                                                List<RawEarning> priceEpisodes,
                                                DasAccount[] dasAccounts,
                                                IncentiveEarnings[] incentiveEarnings)
        {
            _logger.Info("Started Data Lock Validation.");

            var dataLockValidationResult =
                _mediator.Send(new RunDataLockValidationQueryRequest
                {
                    Commitments = commitments,
                    PriceEpisodes = priceEpisodes,
                    DasAccounts = dasAccounts,
                    IncentiveEarnings = incentiveEarnings
                });

            _logger.Info("Finished Data Lock Validation.");

            if (!dataLockValidationResult.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorPerformingDataLockMessage, dataLockValidationResult.Exception);
            }

            return dataLockValidationResult;
        }
    }
}
