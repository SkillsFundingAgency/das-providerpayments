using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
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

        public List<DatalockOutput> GetSuccessfulDatalocks(
            List<DatalockOutputEntity> datalocks, 
            List<DatalockValidationError> datalockValidationErrors,
            List<Commitment> commitments)
        {
            var commitmentDictionary = GetLatestCommitmentVersions(commitments);

            var output = new HashSet<DatalockOutput>();

            var invalidPriceEpisodeIdentifiers = GetPriceEpisodeIdentifiersFromDataLockValidationErrorsExcludingEmployerStoppedDataLocks(datalockValidationErrors)
                .ToList();

            foreach (var datalockOutputEntity in datalocks)
            {
                if (datalockOutputEntity.Payable == false)
                {
                    continue;
                }

                if (datalockOutputEntity.HasDatalockValidationError(invalidPriceEpisodeIdentifiers))
                {
                    continue;
                }

                if (commitmentDictionary.ContainsKey(datalockOutputEntity.CommitmentId))
                {
                    var commitment = commitments.FirstOrDefault(x => x.CommitmentId == datalockOutputEntity.CommitmentId &&
                                                                     x.CommitmentVersionId == datalockOutputEntity.VersionId);
                    if (commitment == null)
                    {
                        commitment = commitmentDictionary[datalockOutputEntity.CommitmentId];
                    }
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

        private static IEnumerable<string> GetPriceEpisodeIdentifiersFromDataLockValidationErrorsExcludingEmployerStoppedDataLocks(List<DatalockValidationError> datalockValidationErrors)
        {
            return datalockValidationErrors
                .Where(x => x.RuleId != DataLockErrorCodes.EmployerStopped)
                .Select(x => x.PriceEpisodeIdentifier);
        }

        private static Dictionary<long, Commitment> GetLatestCommitmentVersions(List<Commitment> commitments)
        {
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

            return commitmentDictionary;
        }
    }
}
