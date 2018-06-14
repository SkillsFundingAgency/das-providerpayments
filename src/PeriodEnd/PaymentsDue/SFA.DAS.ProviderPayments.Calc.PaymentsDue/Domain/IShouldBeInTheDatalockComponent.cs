using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    // ReSharper disable once InconsistentNaming
    public class IShouldBeInTheDatalockComponent : IIShouldBeInTheDataLockComponent
    {
        public List<PriceEpisode> ValidatePriceEpisodes(
            List<Commitment> commitments,
            List<DatalockOutput> dataLocks,
            DateTime lastDayOfAcademicYear)
        {
            // ASSUMPTIONS from Looking at the live data.
            //  Datalocks are 'keyed' by UKPRN, LearnRefNumber, PriceEpisodeIdentifier and CommitmentId
            //  Where there are multiple commitmentids, one is payable and the rest are not
            //  Per unique key above, payable are either all true or all false
            // More data:
            //  When looking at the data it's possible to have multiple commitments for a learner,
            //  one is payable for a period, the others not
            // More data:
            //  We need to ignore payments and refund any price episodes for future academic years

            var priceEpisodes = new List<PriceEpisode>();

            // Process dataLocks by price episode
            var dataLocksByPriceEpisode = dataLocks.ToLookup(x => x.PriceEpisodeIdentifier);

            foreach (var datalockByPriceEpisode in dataLocksByPriceEpisode)
            {
                // Break it down by commitment
                var priceEpisodesByCommitment = datalockByPriceEpisode.ToLookup(x => x.CommitmentId);
                foreach (var priceEpisodeByCommitment in priceEpisodesByCommitment)
                {
                    // Not sure about this one...
                    //  We are checking that there's a commitment with a status of 'Active'
                    var commitment = commitments
                        .OrderByDescending(x => x.CommitmentVersionId)
                        .FirstOrDefault(x => x.CommitmentId == priceEpisodeByCommitment.Key);

                    // Check that it's for this academic year
                    var datePortion = datalockByPriceEpisode.Key.Substring(datalockByPriceEpisode.Key.Length - 10);
                    DateTime dateEpisodeStarted;
                    if (DateTime.TryParseExact(datePortion, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out dateEpisodeStarted))
                    {
                        if (dateEpisodeStarted > lastDayOfAcademicYear)
                        {
                            // Next academic year
                            var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, new List<int>(),
                                commitment?.CommitmentId ?? 0, commitment?.CommitmentVersionId,
                                commitment?.AccountId ?? 0, commitment?.AccountVersionId,
                                mustRefundEpisode: true); // We want to refund this. It's a future academic year
                            priceEpisodes.Add(priceEpisode);
                        }
                        else if(commitment == null ||
                               !commitment.IsLevyPayer ||
                               priceEpisodeByCommitment.All(x => !x.Payable))
                        {
                            // No commitment or the datalock output doesn't have any payable periods
                            var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, new List<int>(),
                                commitment?.CommitmentId ?? 0, commitment?.CommitmentVersionId,
                                commitment?.AccountId ?? 0, commitment?.AccountVersionId); 
                            priceEpisodes.Add(priceEpisode);
                        }
                        else
                        {
                            var payablePeriods = priceEpisodeByCommitment
                                .Where(x => x.Payable)
                                .Select(x => x.Period)
                                .ToList();

                            var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, payablePeriods,
                                commitment.CommitmentId ?? 0, commitment.CommitmentVersionId,
                                commitment.AccountId ?? 0, commitment.AccountVersionId);
                            priceEpisodes.Add(priceEpisode);
                        }
                    }
                }
            }

            return priceEpisodes;
        }
    }
}
