using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders
{
    public class ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder : IEntityBuilder<ApprenticeshipPriceEpisodePeriodisedValuesEntity>
    {
        private string _learnRefNumber = "Lrn001";
        private string _priceEpisodeIdentifier = "550-20-6-2016-09-01";
        private string _attributeName = AttributeNames.PriceEpisodeOnProgPayment.ToString();
        private decimal _period1 = 1000m;
        private decimal _period2 = 1000m;
        private decimal _period3 = 1000m;
        private decimal _period4 = 1000m;
        private decimal _period5 = 1000m;
        private decimal _period6 = 1000m;
        private decimal _period7 = 1000m;
        private decimal _period8 = 1000m;
        private decimal _period9 = 1000m;
        private decimal _period10 = 1000m;
        private decimal _period11 = 1000m;
        private decimal _period12 = 1000m;

        public ApprenticeshipPriceEpisodePeriodisedValuesEntity Build()
        {
            return new ApprenticeshipPriceEpisodePeriodisedValuesEntity
            {
                LearnRefNumber = _learnRefNumber,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier,
                AttributeName = _attributeName,
                Period_1 = _period1,
                Period_2 = _period2,
                Period_3 = _period3,
                Period_4 = _period4,
                Period_5 = _period5,
                Period_6 = _period6,
                Period_7 = _period7,
                Period_8 = _period8,
                Period_9 = _period9,
                Period_10 = _period10,
                Period_11 = _period11,
                Period_12 = _period12
            };
        }

        public ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier = priceEpisodeIdentifier;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder WithAttributeName(string attributeName)
        {
            _attributeName = attributeName;

            return this;
        }

        public ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder WithPeriodValue(decimal period)
        {
            _period1 = period;
            _period2 = period;
            _period3 = period;
            _period4 = period;
            _period5 = period;
            _period6 = period;
            _period7 = period;
            _period8 = period;
            _period9 = period;
            _period10 = period;
            _period11 = period;
            _period12 = period;

            return this;
        }
    }
}