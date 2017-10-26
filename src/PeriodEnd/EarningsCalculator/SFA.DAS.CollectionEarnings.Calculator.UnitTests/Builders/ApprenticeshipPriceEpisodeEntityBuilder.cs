using System;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders
{
    public class ApprenticeshipPriceEpisodeEntityBuilder : IEntityBuilder<ApprenticeshipPriceEpisodeEntity>
    {
        private string _learnRefNumber = "Lrn001";
        private int _aimSeqNumber = 1;
        private DateTime _episodeStartDate = new DateTime(2016, 9, 1);
        private string _priceEpisodeIdentifier = "550-20-6-2016-09-01";
        private DateTime? _priceEpisodeActualEndDate;
        private bool? _priceEpisodeCompleted;
        private decimal? _priceEpisodeCompletionElement = 3000m;
        private decimal? _priceEpisodeInstalmentValue = 1000m;
        private DateTime? _priceEpisodePlannedEndDate = new DateTime(2018, 12, 31);
        private decimal? _priceEpisodeTotalTnpPrice = 15000m;
        private decimal? _tnp1 = 12000m;
        private decimal? _tnp2 = 3000m;
        private decimal? _tnp3;
        private decimal? _tnp4;


        public ApprenticeshipPriceEpisodeEntity Build()
        {
            return new ApprenticeshipPriceEpisodeEntity
            {
                LearnRefNumber = _learnRefNumber,
                PriceEpisodeAimSeqNumber = _aimSeqNumber,
                EpisodeStartDate = _episodeStartDate,
                EpisodeEffectiveTNPStartDate = _episodeStartDate,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier,
                PriceEpisodeActualEndDate = _priceEpisodeActualEndDate,
                PriceEpisodeCompleted = _priceEpisodeCompleted,
                PriceEpisodeCompletionElement = _priceEpisodeCompletionElement,
                PriceEpisodeInstalmentValue = _priceEpisodeInstalmentValue,
                PriceEpisodePlannedEndDate = _priceEpisodePlannedEndDate,
                PriceEpisodeTotalTNPPrice = _priceEpisodeTotalTnpPrice,
                TNP1 = _tnp1,
                TNP2 = _tnp2,
                TNP3 = _tnp3,
                TNP4 = _tnp4
            };
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithEpisodeStartDate(DateTime episodeStartDate)
        {
            _episodeStartDate = episodeStartDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeIdentifier(string priceEpisodeIdentifier)
        {
            _priceEpisodeIdentifier = priceEpisodeIdentifier;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeActualEndDate(DateTime? priceEpisodeActualEndDate)
        {
            _priceEpisodeActualEndDate = priceEpisodeActualEndDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeCompleted(bool? priceEpisodeCompleted)
        {
            _priceEpisodeCompleted = priceEpisodeCompleted;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeCompletionElement(decimal? priceEpisodeCompletionElement)
        {
            _priceEpisodeCompletionElement = priceEpisodeCompletionElement;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeInstalmentValue(decimal? priceEpisodeInstalmentValue)
        {
            _priceEpisodeInstalmentValue = priceEpisodeInstalmentValue;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodePlannedEndDate(DateTime? priceEpisodePlannedEndDate)
        {
            _priceEpisodePlannedEndDate = priceEpisodePlannedEndDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithPriceEpisodeTotalTnpPrice(decimal? ppriceEpisodeTotalTnpPrice)
        {
            _priceEpisodeTotalTnpPrice = ppriceEpisodeTotalTnpPrice;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithTnp1(decimal? tnp1)
        {
            _tnp1 = tnp1;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithTnp2(decimal? tnp2)
        {
            _tnp2 = tnp2;

            return this;
        }

        public ApprenticeshipPriceEpisodeEntityBuilder WithTnp3(decimal? tnp3)
        {
            _tnp3 = tnp3;

            return this;
        }


        public ApprenticeshipPriceEpisodeEntityBuilder WithTnp4(decimal? tnp4)
        {
            _tnp4 = tnp4;

            return this;
        }
    }
}
