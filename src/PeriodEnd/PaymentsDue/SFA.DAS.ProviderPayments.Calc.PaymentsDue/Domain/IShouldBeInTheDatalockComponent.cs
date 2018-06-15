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
            // More data:
            //  The changes to the source data reader repo means that there will not ever be more 
            //  than 1 payable DatalockOutput per period

            var priceEpisodes = new List<PriceEpisode>();

            // Process dataLocks by price episode
            var dataLocksByPriceEpisode = dataLocks.ToLookup(x => x.PriceEpisodeIdentifier);

            foreach (var datalockByPriceEpisode in dataLocksByPriceEpisode)
            {
                // find the commitment
                // if there are more than 1 commitment, then find the latest by commitmentid
                // Use that one

                var datalocks = datalockByPriceEpisode.ToList();
                var commitmentIds = datalocks.Select(x => x.CommitmentId).ToList();

                var commitment = commitments
                    .Where(x => commitmentIds.Contains(x.CommitmentId ?? -1))
                    .OrderByDescending(x => x.CommitmentId)
                    .FirstOrDefault();


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
                            mustRefundEpisode: true,
                            successfulDatalock: false); // We want to refund this. It's a future academic year
                        priceEpisodes.Add(priceEpisode);
                    }
                    else if (commitment == null ||
                             !commitment.IsLevyPayer ||
                             datalocks.All(x => !x.Payable))
                    {
                        var periodsToIgnore = datalocks
                            .Where(x => !x.Payable)
                            .Select(x => x.Period)
                            .ToList();

                        // No commitment or the datalock output doesn't have any payable periods
                        var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, periodsToIgnore,
                            commitment?.CommitmentId ?? 0, commitment?.CommitmentVersionId,
                            commitment?.AccountId ?? 0, commitment?.AccountVersionId, false);
                        priceEpisodes.Add(priceEpisode);
                    }
                    else
                    {
                        var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, new List<int>(),
                            commitment.CommitmentId ?? 0, commitment.CommitmentVersionId,
                            commitment.AccountId ?? 0, commitment.AccountVersionId, true);
                        priceEpisodes.Add(priceEpisode);
                    }
                }
            }

            return priceEpisodes;
        }
    }
}
