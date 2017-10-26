using System;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application
{
    public class PriceEpisodeBuilder : IBuilder<PriceEpisode>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long? _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private long? _aimSeqNumber = 1;
        private long? _standardCode;
        private long? _programmeType = 20;
        private long? _frameworkCode = 550;
        private long? _pwayCode = 6;
        private long? _negotiatedPrice = 15000;
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime _endDate = new DateTime(2018, 12, 31);
        private DateTime? _firstIncentiveThreshholdDate = null;
        private DateTime? _secondIncentiveThreshholdDate = null;

        public PriceEpisode Build()
        {
            return new PriceEpisode
            {
                Ukprn = _ukprn,
                LearnerReferenceNumber = _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSequenceNumber = _aimSeqNumber,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pwayCode,
                NegotiatedPrice = _negotiatedPrice,
                StartDate = _startDate,
                EndDate = _endDate,
                FirstAdditionalPaymentThresholdDate=_firstIncentiveThreshholdDate,
                SecondAdditionalPaymentThresholdDate=_secondIncentiveThreshholdDate
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

        public PriceEpisodeBuilder WithUln(long? uln)
        {
            _uln = uln;

            return this;
        }

        public PriceEpisodeBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public PriceEpisodeBuilder WithAimSeqNumber(long? aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public PriceEpisodeBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public PriceEpisodeBuilder WithProgrammeType(long? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public PriceEpisodeBuilder WithFrameworkCode(long? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public PriceEpisodeBuilder WithPathwayCode(long? pathwayCode)
        {
            _pwayCode = pathwayCode;

            return this;
        }

        public PriceEpisodeBuilder WithNegotiatedPrice(long? negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }

        public PriceEpisodeBuilder WithStartDate(DateTime learnStartDate)
        {
            _startDate = learnStartDate;

            return this;
        }

        public PriceEpisodeBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

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