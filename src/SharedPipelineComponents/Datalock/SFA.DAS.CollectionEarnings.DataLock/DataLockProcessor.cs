using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.RemovePriceEpisodePeriodMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public DataLockProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected DataLockProcessor()
        {
        }

        public virtual void Process()
        {
            _logger.Info("Started Data Lock Processor.");
            _logger.Info($"Using timeout of: {DataLockTask.CommandTimeout}");

            var providersQueryResponse = ReturnValidGetProvidersQueryResponseOrThrow();

            var dasAccountsQueryResponse = ReturnValidGetDasAccountsQueryResponseOrThrow();

            if (providersQueryResponse.HasAnyItems())
            {
                foreach (var provider in providersQueryResponse.Items)
                {
                    _logger.Info($"Performing Data Lock Validation for provider with ukprn {provider.Ukprn}.");

                    var commitments = ReturnProviderCommitmentsOrThrow(provider.Ukprn);
                    var priceEpisodes = ReturnValidGetProviderPriceEpisodesQueryResponseOrThrow(provider.Ukprn);

                    if (priceEpisodes.HasAnyItems())
                    {
                        var dataLockValidationResult = ReturnDataLockValidationResultOrThrow(commitments, priceEpisodes.Items, dasAccountsQueryResponse.Items);

                        WriteDataLockValidationErrorsOrThrow(dataLockValidationResult);
                        WriteDataLockPriceEpisodeMatches(dataLockValidationResult);
                        WriteDataLockPriceEpisodePeriodMatches(dataLockValidationResult);
                    }
                    else
                    {
                        _logger.Info("No price episodes found.");
                    }
                }

                //remove extra price episode period matches
                try
                {
                    _mediator.Send(new RemovePriceEpisodePeriodMatchesCommandRequest
                    {
                        //
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockException(DataLockException.ErrorDeletingPriceEpisodePeriodMatchesMessage, ex);
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


        private Commitment[] ReturnProviderCommitmentsOrThrow(long ukprn)
        {
            _logger.Info($"Reading commitments for provider with ukprn {ukprn}.");

            var commitments =
                _mediator.Send(new GetProviderCommitmentsQueryRequest
                {
                    Ukprn = ukprn
                });

            if (!commitments.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorReadingCommitmentsMessage, commitments.Exception);
            }

            return commitments.Items;
        }

        private GetProviderPriceEpisodesQueryResponse ReturnValidGetProviderPriceEpisodesQueryResponseOrThrow(long ukprn)
        {
            _logger.Info($"Reading price episodes for provider with ukprn {ukprn}.");

            var priceEpisodesQueryResponse = _mediator.Send(new GetProviderPriceEpisodesQueryRequest
            {
                Ukprn = ukprn
            });

            if (!priceEpisodesQueryResponse.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorReadingPriceEpisodesMessage, priceEpisodesQueryResponse.Exception);
            }

            return priceEpisodesQueryResponse;
        }

        private RunDataLockValidationQueryResponse ReturnDataLockValidationResultOrThrow(Commitment[] commitments, PriceEpisode[] priceEpisodes, DasAccount[] dasAccounts )
        {
            _logger.Info("Started Data Lock Validation.");

            var dataLockValidationResult =
                _mediator.Send(new RunDataLockValidationQueryRequest
                {
                    Commitments = commitments,
                    PriceEpisodes = priceEpisodes,
                    DasAccounts = dasAccounts
                });

            _logger.Info("Finished Data Lock Validation.");

            if (!dataLockValidationResult.IsValid)
            {
                throw new DataLockException(DataLockException.ErrorPerformingDataLockMessage, dataLockValidationResult.Exception);
            }

            return dataLockValidationResult;
        }

        private void WriteDataLockValidationErrorsOrThrow(RunDataLockValidationQueryResponse dataLockValidationResponse)
        {
            if (dataLockValidationResponse.HasAnyValidationErrors())
            {
                _logger.Info("Started writing Data Lock Validation Errors.");

                try
                {
                    _mediator.Send(new AddValidationErrorsCommandRequest
                    {
                        ValidationErrors = dataLockValidationResponse.ValidationErrors
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockException(DataLockException.ErrorWritingDataLockValidationErrorsMessage, ex);
                }

                _logger.Info("Finished writing Data Lock Validation Errors.");
            }
        }

        private void WriteDataLockPriceEpisodeMatches(RunDataLockValidationQueryResponse dataLockValidationResponse)
        {
            if (dataLockValidationResponse.HasAnyPriceEpisodeMatches())
            {
                _logger.Info("Started writing price episode matches.");

                try
                {
                    _mediator.Send(new AddPriceEpisodeMatchesCommandRequest
                    {
                        PriceEpisodeMatches = dataLockValidationResponse.PriceEpisodeMatches
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockException(DataLockException.ErrorWritingPriceEpisodeMatchesMessage, ex);
                }

                _logger.Info("Finished writing price episode matches.");
            }
        }

        private void WriteDataLockPriceEpisodePeriodMatches(RunDataLockValidationQueryResponse dataLockValidationResponse)
        {
            if (dataLockValidationResponse.HasAnyPriceEpisodePeriodMatches())
            {
                _logger.Info("Started writing price episode period matches.");

                try
                {
                    _mediator.Send(new AddPriceEpisodePeriodMatchesCommandRequest
                    {
                        PriceEpisodePeriodMatches = dataLockValidationResponse.PriceEpisodePeriodMatches
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockException(DataLockException.ErrorWritingPriceEpisodePeriodMatchesMessage, ex);
                }

                _logger.Info("Finished writing price episode period matches.");
            }
        }
    }
}
