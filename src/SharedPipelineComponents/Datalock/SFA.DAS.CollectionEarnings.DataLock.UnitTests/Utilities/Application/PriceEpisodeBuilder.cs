using System;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application
{
    public class PriceEpisodeBuilder : IBuilder<RawEarning>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private int _aimSeqNumber = 1;
        private int? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pwayCode = 6;
        private decimal _negotiatedPrice = 15000;
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime? _firstIncentiveThreshholdDate = null;
        private DateTime? _secondIncentiveThreshholdDate = null;

        public RawEarning Build()
        {
            return new RawEarning
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                Uln = _uln,
                AimSeqNumber = _aimSeqNumber,
                StandardCode = _standardCode ?? 0,
                ProgrammeType = _programmeType ?? 0,
                FrameworkCode = _frameworkCode ?? 0,
                PathwayCode = _pwayCode ?? 0,
                AgreedPrice = _negotiatedPrice,
                EpisodeStartDate = _startDate,
                FirstIncentiveCensusDate= _firstIncentiveThreshholdDate,
                SecondIncentiveCensusDate= _secondIncentiveThreshholdDate
            };
        }

        public PriceEpisodeBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public PriceEpisodeBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public PriceEpisodeBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public PriceEpisodeBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public PriceEpisodeBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public PriceEpisodeBuilder WithStandardCode(int? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public PriceEpisodeBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public PriceEpisodeBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public PriceEpisodeBuilder WithPathwayCode(int? pathwayCode)
        {
            _pwayCode = pathwayCode;

            return this;
        }

        public PriceEpisodeBuilder WithNegotiatedPrice(decimal negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }

        public PriceEpisodeBuilder WithStartDate(DateTime learnStartDate)
        {
            _startDate = learnStartDate;

            return this;
        }

        public PriceEpisodeBuilder WithFirstIncentiveThreshholdDate(DateTime firstIncentiveThreshholdDate)
        {
            _firstIncentiveThreshholdDate = firstIncentiveThreshholdDate;

            return this;
        }

        public PriceEpisodeBuilder WithSecondIncentiveThreshholdDate(DateTime secondIncentiveThreshholdDate)
        {
            _secondIncentiveThreshholdDate = secondIncentiveThreshholdDate;

            return this;
        }
    }
}