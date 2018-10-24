using System.Collections.Generic;
using System.Linq;
using NLog;
using SFA.DAS.Payments.DCFS.Domain;
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
            var output = new HashSet<DatalockOutput>();
            var commitmentsById = commitments.ToLookup(x => x.CommitmentId);

            var invalidPriceEpisodeIdentifiers = datalockValidationErrors
                .Where(x => x.RuleId != DataLockErrorCodes.EmployerStopped)
                .Select(x => x.PriceEpisodeIdentifier)
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

                Commitment commitment = null;

                if (commitmentsById.Contains(datalockOutputEntity.CommitmentId))
                {
                    commitment = commitmentsById[datalockOutputEntity.CommitmentId]
                        .FirstOrDefault(x => x.CommitmentId == datalockOutputEntity.CommitmentId &&
                                             x.CommitmentVersionId == datalockOutputEntity.VersionId);
                }

                if (commitment != null)
                {
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
