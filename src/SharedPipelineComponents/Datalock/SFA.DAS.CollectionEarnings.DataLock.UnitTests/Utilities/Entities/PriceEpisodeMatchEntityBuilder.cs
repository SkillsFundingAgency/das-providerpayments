using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class PriceEpisodeMatchEntityBuilder : IBuilder<PriceEpisodeMatchEntity>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _aimSeqNumber = 1;
        private long _commitmentId = 1;
        private string _priceEpisodeIdentifier = "27-25-2016-09-01";

        public PriceEpisodeMatchEntity Build()
        {
            return new PriceEpisodeMatchEntity
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                CommitmentId = _commitmentId,
                PriceEpisodeIdentifier = _priceEpisodeIdentifier
            };
        }

        public PriceEpisodeMatchEntityBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public PriceEpisodeMatchEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public PriceEpisodeMatchEntityBuilder WithAimSeqNumber(long aimSeqNumber)
        {
            _aimSeqNumber = aimSeqNumber;

            return this;
        }

        public PriceEpisodeMatchEntityBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }
    }
}