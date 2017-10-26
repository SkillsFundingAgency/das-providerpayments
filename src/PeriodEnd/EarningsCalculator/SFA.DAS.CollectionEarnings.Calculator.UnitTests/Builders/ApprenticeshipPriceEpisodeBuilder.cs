using System;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders
{
    public class ApprenticeshipPriceEpisodeBuilder : IEntityBuilder<ApprenticeshipPriceEpisode>
    {
        private string _id = "550-20-6-2016-09-01";
        private string _learnerReferenceNumber = "Lrn001";
        private int _aimSequenceNumber = 1;
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime? _plannedEndDate = new DateTime(2018, 12, 31);
        private DateTime? _actualEndDate;
        private decimal _negotiatedPrice = 15000m;
        private int? _tnp1 = 12000;
        private int? _tnp2 = 3000;
        private int? _tnp3;
        private int? _tnp4;
        private decimal _monthlyAmount = 1000m;
        private decimal? _completionAmount = 3000m;
        private decimal? _balancingAmount;
        private bool? _completed;
        private bool _lastEpisode;

        public ApprenticeshipPriceEpisode Build()
        {
            return new ApprenticeshipPriceEpisode
            {
                Id = _id,
                LearnerReferenceNumber = _learnerReferenceNumber,
                AimSequenceNumber = _aimSequenceNumber,
                StartDate = _startDate,
                PlannedEndDate = _plannedEndDate,
                ActualEndDate = _actualEndDate,
                NegotiatedPrice = _negotiatedPrice,
                Tnp1 = _tnp1,
                Tnp2 = _tnp2,
                Tnp3 = _tnp3,
                Tnp4 = _tnp4,
                MonthlyAmount = _monthlyAmount,
                CompletionAmount = _completionAmount,
                BalancingAmount = _balancingAmount,
                Completed = _completed,
                LastEpisode = _lastEpisode
            };
        }

        public ApprenticeshipPriceEpisodeBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnerReferenceNumber = learnRefNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSequenceNumber = aimseqNumber;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithStartDate(DateTime episodeStartDate)
        {
            _startDate = episodeStartDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithId(string priceEpisodeIdentifier)
        {
            _id = priceEpisodeIdentifier;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithActualEndDate(DateTime? priceEpisodeActualEndDate)
        {
            _actualEndDate = priceEpisodeActualEndDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithBalancingAmount(decimal? priceEpisodeBalanceValue)
        {
            _balancingAmount = priceEpisodeBalanceValue;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithCompleted(bool? priceEpisodeCompleted)
        {
            _completed = priceEpisodeCompleted;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithCompletionAmount(decimal? priceEpisodeCompletionElement)
        {
            _completionAmount = priceEpisodeCompletionElement;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithMonthlyAmount(decimal priceEpisodeInstalmentValue)
        {
            _monthlyAmount = priceEpisodeInstalmentValue;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithPlannedEndDate(DateTime? priceEpisodePlannedEndDate)
        {
            _plannedEndDate = priceEpisodePlannedEndDate;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithNegotiatedPrice(decimal ppriceEpisodeTotalTnpPrice)
        {
            _negotiatedPrice = ppriceEpisodeTotalTnpPrice;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithTnp1(int? tnp1)
        {
            _tnp1 = tnp1;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithTnp2(int? tnp2)
        {
            _tnp2 = tnp2;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithTnp3(int? tnp3)
        {
            _tnp3 = tnp3;

            return this;
        }


        public ApprenticeshipPriceEpisodeBuilder WithTnp4(int? tnp4)
        {
            _tnp4 = tnp4;

            return this;
        }

        public ApprenticeshipPriceEpisodeBuilder WithLastEpisode(bool lastEpisode)
        {
            _lastEpisode = lastEpisode;

            return this;
        }
    }
}