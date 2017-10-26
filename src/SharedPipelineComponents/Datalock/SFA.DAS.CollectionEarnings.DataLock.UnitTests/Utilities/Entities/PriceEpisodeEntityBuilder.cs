using System;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class PriceEpisodeEntityBuilder : IBuilder<PriceEpisodeEntity>
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
        private DateTime _learnStartDate = new DateTime(2016, 9, 1);

        public PriceEpisodeEntity Build()
        {
            return new PriceEpisodeEntity
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSeqNumber = _aimSeqNumber,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pwayCode,
                NegotiatedPrice = _negotiatedPrice,
                StartDate = _learnStartDate
            };
        }

        public PriceEpisodeEntityBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public PriceEpisodeEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public PriceEpisodeEntityBuilder WithUln(long? uln)
        {
            _uln = uln;

            return this;
        }

        public PriceEpisodeEntityBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public PriceEpisodeEntityBuilder WithAimSeqNumber(long? aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public PriceEpisodeEntityBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public PriceEpisodeEntityBuilder WithProgrammeType(long? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public PriceEpisodeEntityBuilder WithFrameworkCode(long? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public PriceEpisodeEntityBuilder WithPathwayCode(long? pathwayCode)
        {
            _pwayCode = pathwayCode;

            return this;
        }

        public PriceEpisodeEntityBuilder WithNegotiatedPrice(long? negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }

        public PriceEpisodeEntityBuilder WithLearnStartDate(DateTime learnStartDate)
        {
            _learnStartDate = learnStartDate;

            return this;
        }
    }
}