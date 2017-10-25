using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryResponse : Response
    {
        public ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode[] PriceEpisodes { get; set; }
        public ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues[] PriceEpisodesPeriodisedValues { get; set; }
        public ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod[] PriceEpisodesPeriodsEarnings { get; set; }
    }
}