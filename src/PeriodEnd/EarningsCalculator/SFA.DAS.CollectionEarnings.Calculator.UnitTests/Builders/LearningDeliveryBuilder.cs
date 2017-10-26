using System;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders
{
    public class LearningDeliveryBuilder : IEntityBuilder<LearningDelivery>
    {
        private int _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private int _aimSeqNumber = 1;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;
        private DateTime _learnStartDate = new DateTime(2017, 9, 1);
        private DateTime? _origLearnStartDate;
        private DateTime _learnPlanEndDate = new DateTime(2018, 9, 8);
        private DateTime? _learnActEndDate;
        private int? _completionStatus = 2;
        private PriceEpisode[] _priceEpisodes =
        {
            new PriceEpisode
            {
                Id = "550-20-6-2017-09-01",
                StartDate = new DateTime(2017, 9, 1),
                NegotiatedPrice = 15000,
                Tnp1 = 12000,
                Tnp2 = 3000,
                Type = PriceEpisodeType.Initial,
                LastEpisode = true
            }
        };

        public LearningDelivery Build()
        {
            return new LearningDelivery
            {
                Ukprn = _ukprn,
                LearnerReferenceNumber =  _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSequenceNumber = _aimSeqNumber,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode,
                LearningStartDate = _learnStartDate,
                OriginalLearningStartDate = _origLearnStartDate,
                LearningPlannedEndDate = _learnPlanEndDate,
                LearningActualEndDate = _learnActEndDate,
                CompletionStatus = _completionStatus,
                PriceEpisodes = _priceEpisodes
            };
        }

        public LearningDeliveryBuilder WithUkprn(int ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public LearningDeliveryBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public LearningDeliveryBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public LearningDeliveryBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public LearningDeliveryBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public LearningDeliveryBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public LearningDeliveryBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public LearningDeliveryBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public LearningDeliveryBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }

        public LearningDeliveryBuilder WithLearnStartDate(DateTime learnStartDate)
        {
            _learnStartDate = learnStartDate;

            _priceEpisodes[0].StartDate = _learnStartDate;

            return this;
        }

        public LearningDeliveryBuilder WithOrigLearnStartDate(DateTime? origLearnStartDate)
        {
            _origLearnStartDate = origLearnStartDate;

            return this;
        }

        public LearningDeliveryBuilder WithLearnPlanEndDate(DateTime learnPlanEndDate)
        {
            _learnPlanEndDate = learnPlanEndDate;

            return this;
        }

        public LearningDeliveryBuilder WithLearnActEndDate(DateTime? learnActEndDate)
        {
            _learnActEndDate = learnActEndDate;

            return this;
        }

        public LearningDeliveryBuilder WithCompletionStatus(int? completionStatus)
        {
            _completionStatus = completionStatus;

            return this;
        }

        public LearningDeliveryBuilder WithNegotiatedPrice(int negotiatedPrice)
        {
            _priceEpisodes[0].NegotiatedPrice = negotiatedPrice;

            return this;
        }
    }
}