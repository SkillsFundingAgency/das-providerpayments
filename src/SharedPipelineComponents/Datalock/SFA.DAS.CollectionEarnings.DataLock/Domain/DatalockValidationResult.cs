using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services.Extensions;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class DatalockValidationResult
    {
        public List<DatalockValidationError> ValidationErrors { get; } = new List<DatalockValidationError>();
        public List<DatalockValidationErrorByPeriod> ValidationErrorsByPeriod { get; } = new List<DatalockValidationErrorByPeriod>();
        public List<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; } = new List<PriceEpisodePeriodMatchEntity>();
        public List<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; } = new List<PriceEpisodeMatchEntity>();
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();
        
        public void Add(RawEarning earning, List<string> errors, TransactionTypesFlag paymentType, CommitmentEntity commitment)
        {
            var payable = false;
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    if (ValidationErrors.DoesNotAlreadyContainEarningForThisError(earning, error) &&
                        PriceEpisodePeriodMatches.DoesNotContainEarningForCommitmentAndPaymentType(earning, commitment, paymentType))
                    {
                        ValidationErrors.Add(new DatalockValidationError
                        {
                            LearnRefNumber = earning.LearnRefNumber,
                            AimSeqNumber = earning.AimSeqNumber,
                            PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                            RuleId = error,
                            Ukprn = earning.Ukprn,
                        });
                    }

                    if (ValidationErrorsByPeriod.DoesNotAlreadyContainEarningForThisError(earning, error))
                    {
                        ValidationErrorsByPeriod.Add(new DatalockValidationErrorByPeriod()
                        {
                            LearnRefNumber = earning.LearnRefNumber,
                            AimSeqNumber = earning.AimSeqNumber,
                            PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                            RuleId = error,
                            Ukprn = earning.Ukprn,
                            Period = earning.Period,
                        });
                    }
                }
            }
            else
            {
                payable = true;
            }

            if (commitment == null)
            {
                return;
            }

            PriceEpisodePeriodMatches.Add(new PriceEpisodePeriodMatchEntity
            {
                AimSeqNumber = earning.AimSeqNumber,
                CommitmentId = commitment.CommitmentId,
                LearnRefNumber = earning.LearnRefNumber,
                PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                Period = earning.Period,
                TransactionTypesFlag = paymentType,
                Payable = payable,
                Ukprn = earning.Ukprn,
                VersionId = commitment.VersionId,
            });

            if (payable)
            {
                var validationErrorsToRemove = ValidationErrors.Where(x =>
                        x.Ukprn == earning.Ukprn &&
                        x.LearnRefNumber == earning.LearnRefNumber &&
                        x.PriceEpisodeIdentifier == earning.PriceEpisodeIdentifier)
                    .ToList();

                foreach (var validationError in validationErrorsToRemove)
                {
                    ValidationErrors.Remove(validationError);
                }
            }

            if (PriceEpisodeMatches.DoesNotAlreadyContainEarningForCommitment(earning, commitment, payable))
            {
                PriceEpisodeMatches.Add(new PriceEpisodeMatchEntity
                {
                    CommitmentId = commitment.CommitmentId,
                    IsSuccess = payable,
                    LearnRefNumber = earning.LearnRefNumber,
                    PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                    Ukprn = earning.Ukprn,
                    AimSeqNumber = earning.AimSeqNumber,
                });
            }
        }
    }
}