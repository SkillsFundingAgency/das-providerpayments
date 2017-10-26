using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders
{
    public class ApprenticeshipPriceEpisodePeriodEntityBuilder : IEntityBuilder<ApprenticeshipPriceEpisodePeriodEntity>
    {
        private string _learnRefNumber = "Lrn001";
        private string _priceEpisodeIdentifier = "550-20-6-2016-09-01";
        private int _period = 1;
        private decimal _priceEpisodeOnProgPayment = 1000m;
        private decimal _priceEpisodeCompletionPayment = 3000m;
        private decimal _priceEpisodeBalancePayment = 2000m;

        public ApprenticeshipPriceEpisodePeriodEntity Build()
        {
            return new ApprenticeshipPriceEpisodePeriodEntity
            {
                LearnRefNumber = _learnRefNumber,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier,
                Period = _period,
                PriceEpisodeOnProgPayment = _priceEpisodeOnProgPayment,
                PriceEpisodeCompletionPayment = _priceEpisodeCompletionPayment,
                PriceEpisodeBalancePayment = _priceEpisodeBalancePayment
            };
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier = priceEpisodeIdentifier;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithPeriod(int period)
        {
            _period = period;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithProgrammeAimOnProgPayment(decimal programmeAimOnProgPayment)
        {
            _priceEpisodeOnProgPayment = programmeAimOnProgPayment;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithProgrammeAimCompletionPayment(decimal programmeAimCompletionPayment)
        {
            _priceEpisodeCompletionPayment = programmeAimCompletionPayment;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodEntityBuilder WithProgrammeAimBalPayment(decimal programmeAimBalPayment)
        {
            _priceEpisodeBalancePayment = programmeAimBalPayment;

            return this;
        }
    }
}