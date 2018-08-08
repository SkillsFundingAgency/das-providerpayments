using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class DatalockValidationService : IValidateRawDatalocks
    {
        private readonly ILogger _logger;

        public DatalockValidationService(ILogger logger)
        {
            _logger = logger;
        }

        public List<DatalockOutput> ProcessDatalocks(
            List<DatalockOutputEntity> datalocks, 
            List<DatalockValidationError> datalockValidationErrors,
            List<Commitment> commitments)
        {
            // We want the account id from the commitment id, different versions 
            //  of a commitment will have the same account id

            // Generally there are more datalocks than commitments by an order of 10
            var commitmentDictionary = new Dictionary<long, Commitment>();
            var orderedCommitments = commitments
                .OrderByDescending(x => x.CommitmentId)
                .ThenByDescending(x => x.CommitmentVersionId);

            foreach (var commitment in orderedCommitments)
            {
                if (!commitmentDictionary.ContainsKey(commitment.CommitmentId))
                {
                    commitmentDictionary.Add(commitment.CommitmentId, commitment);
                }
            }

            var output = new HashSet<DatalockOutput>();

            var invalidPriceEpisodeIdentifiers = datalockValidationErrors
                .Select(x => x.PriceEpisodeIdentifier)
                .ToList();

            foreach (var datalockOutputEntity in datalocks)
            {
                // Reject any where payable != true
                //  or the price episode id is in the validation error list
                if (datalockOutputEntity.Payable == false ||
                    invalidPriceEpisodeIdentifiers.Contains(datalockOutputEntity.PriceEpisodeIdentifier))
                {
                    continue;
                }

                if (commitmentDictionary.ContainsKey(datalockOutputEntity.CommitmentId))
                {
                    var commitment = commitmentDictionary[datalockOutputEntity.CommitmentId];
                    var processedValueObject = new DatalockOutput(datalockOutputEntity, commitment);
                    output.Add(processedValueObject);
                }
                else
                {
                    _logger.Error("Could not find a matching commitment for datalock. " +
                                  $"Commitment ID: {datalockOutputEntity.CommitmentId}, " +
                                  $"Price Episode Identifier: {datalockOutputEntity.PriceEpisodeIdentifier}, " +
                                  $"Period: {datalockOutputEntity.Period}");
                }
            }

            return output.ToList();
        }
    }
}
